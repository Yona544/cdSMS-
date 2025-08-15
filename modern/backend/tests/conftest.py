import asyncio
import hashlib
from typing import AsyncGenerator, Generator

import pytest
from fastapi.testclient import TestClient
from httpx import AsyncClient
from sqlalchemy.ext.asyncio import AsyncSession, create_async_engine
from sqlalchemy.orm import sessionmaker

from app.core.db import get_session
from app.core.settings import get_settings, Settings
from app.main import app
from app.models import Base
from app.models.tenant import Tenant

# --- Settings and Event Loop ---

@pytest.fixture(scope="session")
def event_loop(request) -> Generator:
    """Create an instance of the default event loop for each test case."""
    loop = asyncio.get_event_loop_policy().new_event_loop()
    yield loop
    loop.close()

@pytest.fixture(scope="session")
def test_settings() -> Settings:
    """Returns a settings object for testing."""
    return Settings(
        env="test",
        database_url="sqlite+aiosqlite:///:memory:",
        secret_key="test-secret",
    )

# --- Database Fixtures ---

@pytest.fixture(scope="session")
async def db_engine(test_settings: Settings):
    """Fixture for a clean database engine."""
    engine = create_async_engine(test_settings.database_url, future=True)
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)
    yield engine
    await engine.dispose()

@pytest.fixture(scope="function")
async def db_session(db_engine) -> AsyncGenerator[AsyncSession, None]:
    """Fixture for a database session."""
    connection = await db_engine.connect()
    transaction = await connection.begin()

    TestingSessionLocal = sessionmaker(
        autocommit=False, autoflush=False, bind=connection, class_=AsyncSession
    )
    session = TestingSessionLocal()

    yield session

    await session.close()
    await transaction.rollback()
    await connection.close()

# --- API Client Fixture ---

@pytest.fixture(scope="function")
async def async_client(db_session: AsyncSession) -> AsyncGenerator[AsyncClient, None]:
    """An async client for testing the API."""
    def override_get_session() -> AsyncGenerator[AsyncSession, None]:
        yield db_session

    app.dependency_overrides[get_session] = override_get_session
    async with AsyncClient(app=app, base_url="http://test") as client:
        yield client
    app.dependency_overrides.clear()

# --- Tenant Fixture ---

@pytest.fixture(scope="function")
async def test_tenant(db_session: AsyncSession) -> Tenant:
    """Fixture to create a test tenant."""
    api_key = "test-api-key"
    key_hash = hashlib.sha256(api_key.encode()).hexdigest()

    tenant = Tenant(
        name="Test Tenant",
        api_key_hash=key_hash,
        active=True
    )
    db_session.add(tenant)
    await db_session.commit()
    await db_session.refresh(tenant)

    # Store the raw key for tests to use
    tenant.raw_api_key = api_key

    return tenant
