import datetime
from sqlalchemy import Column, Integer, String, Boolean, DateTime, Text
from . import Base

class Tenant(Base):
    __tablename__ = "tenants"

    id = Column(Integer, primary_key=True, index=True)
    name = Column(String, nullable=False)
    api_key_hash = Column(String, unique=True, index=True, nullable=False)
    active = Column(Boolean, default=True, nullable=False)
    created_at = Column(DateTime, default=datetime.datetime.utcnow)
    updated_at = Column(DateTime, default=datetime.datetime.utcnow, onupdate=datetime.datetime.utcnow)

# Users model (global users; no tenant scoping for now)
class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True, index=True)
    username = Column(String(100), unique=True, nullable=False, index=True)
    display_name = Column(String(200), nullable=True)
    email = Column(String(255), nullable=True)
    is_admin = Column(Boolean, nullable=False, default=False)
    tags = Column(Text, nullable=True)
