from dataclasses import dataclass
import datetime
import uuid

@dataclass
class Base:
    session_id: uuid
    timestamp: datetime
    data: dict