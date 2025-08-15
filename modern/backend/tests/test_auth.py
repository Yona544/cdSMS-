import pytest
from fastapi.testclient import TestClient

# The 'client' and 'seeded_db' fixtures are auto-injected from conftest.py

def test_authentication_with_header(client):
    """Test authentication with X-API-Key header"""
    response = client.get(
        "/api/tenant/profile",
        headers={"X-API-Key": "acme_api_key_12345"}
    )
    assert response.status_code == 200
    assert response.json()["id"] == "acme-corp"

def test_authentication_with_bearer_token(client):
    """Test authentication with Bearer token"""
    response = client.get(
        "/api/tenant/profile",
        headers={"Authorization": "Bearer acme_api_key_12345"}
    )
    assert response.status_code == 200
    assert response.json()["id"] == "acme-corp"

def test_authentication_with_query_param(client):
    """Test authentication with query parameter"""
    response = client.get("/api/tenant/profile?apiKey=acme_api_key_12345")
    assert response.status_code == 200
    assert response.json()["id"] == "acme-corp"

def test_invalid_api_key(client):
    """Test invalid API key returns 401"""
    response = client.get(
        "/api/tenant/profile",
        headers={"X-API-Key": "invalid_key"}
    )
    assert response.status_code == 401

def test_missing_api_key(client):
    """Test missing API key returns 401"""
    response = client.get("/api/tenant/profile")
    assert response.status_code == 401
