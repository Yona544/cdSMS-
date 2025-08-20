from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession
from sqlalchemy.orm import sessionmaker, declarative_base
from app.core.settings import get_settings
import os

settings = get_settings()

# Create the SQLAlchemy async engine
engine = create_async_engine(
    settings.database_url,
    future=True,
    echo=False,  # Set to True to see generated SQL
)

# Create a session factory
AsyncSessionFactory = sessionmaker(
    engine,
    class_=AsyncSession,
    expire_on_commit=False,
)

# Declarative base for SQLAlchemy models
Base = declarative_base()

async def init_db() -> None:
    """
    Initialize the database: ensure path exists for SQLite and create tables if not present.
    Uses Base.metadata.create_all for initial bring-up (Alembic optional later).
    """
    # Ensure ./data exists for sqlite urls like sqlite+aiosqlite:///./data/app.db
    if settings.database_url.startswith("sqlite"):
        os.makedirs("./data", exist_ok=True)

    # Import models so tables are registered on Base.metadata
    from app import models  # noqa: F401

    # Create tables
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)

async def get_session() -> AsyncSession:
    """
    FastAPI dependency to get a database session.
    """
    async with AsyncSessionFactory() as session:
        yield session
