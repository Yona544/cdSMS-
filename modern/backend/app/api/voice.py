from fastapi import APIRouter, Depends, HTTPException, status, Query
from typing import List
from app.models.schemas import (
    VoiceMessageCreate, VoiceMessageUpdate, VoiceMessageResponse,
    TenantResponse, APIResponseWithData, PaginatedResponse, PaginationMeta
)
from app.core.security import get_current_tenant
from app.services.voice_service import voice_service
import math

router = APIRouter(prefix="/voice-messages", tags=["Voice Messages"])

@router.post("/", response_model=APIResponseWithData)
async def create_voice_message(
    message_data: VoiceMessageCreate,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Create a new voice message"""
    try:
        message = voice_service.create_voice_message(tenant.id, message_data)
        return APIResponseWithData(
            data=message,
            message="Voice message created successfully"
        )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail=str(e)
        )

@router.get("/", response_model=PaginatedResponse)
async def list_voice_messages(
    page: int = Query(1, ge=1),
    per_page: int = Query(20, ge=1, le=100),
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """List voice messages with pagination"""
    messages, total_count = voice_service.list_voice_messages(
        tenant.id, limit=per_page, offset=(page - 1) * per_page
    )

    total_pages = math.ceil(total_count / per_page) if total_count > 0 else 0

    return PaginatedResponse(
        data=messages,
        message=f"Retrieved {len(messages)} voice messages",
        pagination=PaginationMeta(
            page=page,
            per_page=per_page,
            total_items=total_count,
            total_pages=total_pages,
            has_next=page < total_pages,
            has_prev=page > 1
        )
    )

@router.get("/{message_id}", response_model=APIResponseWithData)
async def get_voice_message(
    message_id: int,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Get a specific voice message"""
    message = voice_service.get_voice_message(tenant.id, message_id)
    if not message:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Voice message not found"
        )

    return APIResponseWithData(
        data=message,
        message="Voice message retrieved successfully"
    )

@router.put("/{message_id}", response_model=APIResponseWithData)
async def update_voice_message(
    message_id: int,
    update_data: VoiceMessageUpdate,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Update a voice message"""
    try:
        message = voice_service.update_voice_message(tenant.id, message_id, update_data)
        if not message:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Voice message not found"
            )
        return APIResponseWithData(
            data=message,
            message="Voice message updated successfully"
        )
    except ValueError as e:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=str(e)
        )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail=str(e)
        )

@router.delete("/{message_id}", response_model=APIResponseWithData)
async def delete_voice_message(
    message_id: int,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Delete a voice message"""
    success = voice_service.delete_voice_message(tenant.id, message_id)
    if not success:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Voice message not found"
        )

    return APIResponseWithData(
        data={"deleted": True},
        message="Voice message deleted successfully"
    )

@router.post("/{message_id}/duplicate", response_model=APIResponseWithData)
async def duplicate_voice_message(
    message_id: int,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Create a duplicate of an existing voice message"""
    try:
        duplicate = voice_service.duplicate_voice_message(tenant.id, message_id)
        return APIResponseWithData(
            data=duplicate,
            message="Voice message duplicated successfully"
        )
    except ValueError as e:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=str(e)
        )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail=str(e)
        )
