from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession
from sqlalchemy.orm import sessionmaker
from app.core.settings import get_settings

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

async def get_session() -> AsyncSession:
    """
    FastAPI dependency to get a database session.
    """
    async with AsyncSessionFactory() as session:
        yield session
