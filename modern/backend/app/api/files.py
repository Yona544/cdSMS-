from fastapi import APIRouter, Depends, HTTPException, status, Query, UploadFile, File
from fastapi.responses import FileResponse
from typing import List
import os

from app.models.schemas import (
    VoiceFileUpdate, VoiceFileResponse, TenantResponse, APIResponseWithData,
    PaginatedResponse, PaginationMeta, FileUploadResponse
)
from app.core.security import get_current_tenant
from app.services.file_service import file_service

router = APIRouter(prefix="/files", tags=["Files"])

@router.post("/upload", response_model=FileUploadResponse)
async def upload_file(
    tenant: TenantResponse = Depends(get_current_tenant),
    file: UploadFile = File(...)
):
    """Upload a new file (e.g., MP3 audio)."""
    try:
        # The service will save the file and create a database record
        voice_file = await file_service.save_upload_file(tenant.id, file)
        return FileUploadResponse(
            data={
                "id": voice_file.id,
                "filename": voice_file.filename,
                "file_path": voice_file.file_path,
                "file_size": voice_file.file_size,
                "file_type": voice_file.file_type
            },
            message="File uploaded successfully"
        )
    except Exception as e:
        raise HTTPException(status_code=status.HTTP_500_INTERNAL_SERVER_ERROR, detail=str(e))

@router.get("/", response_model=PaginatedResponse)
async def list_files(
    page: int = Query(1, ge=1),
    per_page: int = Query(20, ge=1, le=100),
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """List all files for the tenant with pagination."""
    files, total_count = file_service.list_voice_files(
        tenant.id, limit=per_page, offset=(page - 1) * per_page
    )
    total_pages = (total_count + per_page - 1) // per_page

    return PaginatedResponse(
        data=files,
        message=f"Retrieved {len(files)} files",
        pagination=PaginationMeta(
            page=page,
            per_page=per_page,
            total_items=total_count,
            total_pages=total_pages,
            has_next=page < total_pages,
            has_prev=page > 1,
        ),
    )

@router.get("/{file_id}", response_model=APIResponseWithData)
async def get_file_metadata(
    file_id: int,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Get metadata for a specific file."""
    voice_file = file_service.get_voice_file(tenant.id, file_id)
    if not voice_file:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="File not found")
    return APIResponseWithData(data=voice_file)

@router.get("/{file_id}/download")
async def download_file(
    file_id: int,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Download a specific file."""
    voice_file = file_service.get_voice_file(tenant.id, file_id)
    if not voice_file or not os.path.exists(voice_file.file_path):
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="File not found")

    return FileResponse(path=voice_file.file_path, filename=voice_file.filename)

@router.put("/{file_id}", response_model=APIResponseWithData)
async def update_file_metadata(
    file_id: int,
    update_data: VoiceFileUpdate,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Update a file's metadata."""
    updated_file = file_service.update_voice_file(tenant.id, file_id, update_data)
    if not updated_file:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="File not found")
    return APIResponseWithData(data=updated_file, message="File metadata updated successfully")

@router.delete("/{file_id}", response_model=APIResponseWithData)
async def delete_file(
    file_id: int,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Delete a file and its metadata."""
    success = file_service.delete_voice_file(tenant.id, file_id)
    if not success:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="File not found")
    return APIResponseWithData(data={"deleted": True}, message="File deleted successfully")
