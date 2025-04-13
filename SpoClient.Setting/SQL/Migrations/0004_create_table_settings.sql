---
name: 0004_create_table_settings
description: Initialize Settings table
depends_on:
---

CREATE TABLE Settings (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	Key TEXT NOT NULL,
	Value TEXT
);
