import sqlite3
from typing import List, Tuple, Optional
from app.core.database import DatabaseManager
from app.models.schemas import CommunicationLogResponse

class CommunicationService:
    def __init__(self):
        self.db_manager = DatabaseManager()

    def _row_to_response(self, row: sqlite3.Row) -> CommunicationLogResponse:
        return CommunicationLogResponse.model_validate(dict(row))

    def list_communication_history(
        self, tenant_id: str, limit: int, offset: int
    ) -> Tuple[List[CommunicationLogResponse], int]:
        """
        Retrieves a paginated list of communication logs for a specific tenant.
        """
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()

            # Get total count
            cursor.execute("SELECT COUNT(*) FROM communication_log WHERE tenant_id = ?", (tenant_id,))
            total_count = cursor.fetchone()[0]

            # Get paginated results
            cursor.execute(
                "SELECT * FROM communication_log WHERE tenant_id = ? ORDER BY created_at DESC LIMIT ? OFFSET ?",
                (tenant_id, limit, offset),
            )
            logs = [self._row_to_response(row) for row in cursor.fetchall()]
            return logs, total_count

    def get_communication_status(self, tenant_id: str, twilio_sid: str) -> Optional[CommunicationLogResponse]:
        """
        Retrieves a specific communication log by its Twilio SID.
        """
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                "SELECT * FROM communication_log WHERE twilio_sid = ? AND tenant_id = ?",
                (twilio_sid, tenant_id),
            )
            row = cursor.fetchone()
            return self._row_to_response(row) if row else None

communication_service = CommunicationService()
