import sqlite3
from typing import List, Tuple, Optional
import os
from fastapi import UploadFile
from app.core.database import DatabaseManager
from app.models.schemas import VoiceFileCreate, VoiceFileUpdate, VoiceFileResponse
from app.core.config import settings

class FileService:
    def __init__(self):
        self.db_manager = DatabaseManager()
        self.upload_path = settings.upload_path
        os.makedirs(self.upload_path, exist_ok=True)

    def _row_to_response(self, row: sqlite3.Row) -> VoiceFileResponse:
        return VoiceFileResponse.model_validate(dict(row))

    async def save_upload_file(self, tenant_id: str, file: UploadFile) -> VoiceFileResponse:
        # Note: This is a simplified file saving mechanism.
        # In a real application, you'd want to generate a secure, unique filename.
        file_path = os.path.join(self.upload_path, f"{tenant_id}_{file.filename}")

        with open(file_path, "wb") as buffer:
            content = await file.read()
            buffer.write(content)

        file_size = os.path.getsize(file_path)
        file_type = file.content_type.split("/")[1] # e.g., 'audio/mpeg' -> 'mpeg'

        file_data = VoiceFileCreate(
            filename=file.filename,
            file_type=file_type,
            file_path=file_path,
            file_size=file_size,
            description=None,
            tag_list=None,
            caller_number=None
        )
        return self.create_voice_file(tenant_id, file_data)

    def create_voice_file(self, tenant_id: str, file_data: VoiceFileCreate) -> VoiceFileResponse:
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                """
                INSERT INTO voice_files (tenant_id, filename, file_type, file_path, file_size, description, tag_list, caller_number)
                VALUES (?, ?, ?, ?, ?, ?, ?, ?)
                """,
                (
                    tenant_id,
                    file_data.filename,
                    file_data.file_type,
                    file_data.file_path,
                    file_data.file_size,
                    file_data.description,
                    file_data.tag_list,
                    file_data.caller_number
                ),
            )
            new_id = cursor.lastrowid
            conn.commit()
        return self.get_voice_file(tenant_id, new_id)

    def list_voice_files(self, tenant_id: str, limit: int, offset: int) -> Tuple[List[VoiceFileResponse], int]:
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute("SELECT COUNT(*) FROM voice_files WHERE tenant_id = ?", (tenant_id,))
            total_count = cursor.fetchone()[0]
            cursor.execute(
                "SELECT * FROM voice_files WHERE tenant_id = ? ORDER BY created_at DESC LIMIT ? OFFSET ?",
                (tenant_id, limit, offset),
            )
            files = [self._row_to_response(row) for row in cursor.fetchall()]
            return files, total_count

    def get_voice_file(self, tenant_id: str, file_id: int) -> Optional[VoiceFileResponse]:
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                "SELECT * FROM voice_files WHERE id = ? AND tenant_id = ?",
                (file_id, tenant_id),
            )
            row = cursor.fetchone()
            return self._row_to_response(row) if row else None

    def update_voice_file(self, tenant_id: str, file_id: int, update_data: VoiceFileUpdate) -> Optional[VoiceFileResponse]:
        update_fields = {k: v for k, v in update_data.model_dump(exclude_unset=True).items()}
        if not update_fields:
            raise ValueError("No update data provided")

        set_clause = ", ".join([f"{field} = ?" for field in update_fields.keys()])

        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                f"UPDATE voice_files SET {set_clause} WHERE id = ? AND tenant_id = ?",
                (*update_fields.values(), file_id, tenant_id),
            )
            conn.commit()
        return self.get_voice_file(tenant_id, file_id)

    def delete_voice_file(self, tenant_id: str, file_id: int) -> bool:
        file_to_delete = self.get_voice_file(tenant_id, file_id)
        if not file_to_delete:
            return False

        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                "DELETE FROM voice_files WHERE id = ? AND tenant_id = ?",
                (file_id, tenant_id),
            )
            conn.commit()
            if cursor.rowcount > 0:
                # Also delete the physical file
                if os.path.exists(file_to_delete.file_path):
                    os.unlink(file_to_delete.file_path)
                return True
            return False

file_service = FileService()
