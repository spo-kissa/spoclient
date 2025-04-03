---
name: 0001_create_servers
description: Initialize Servers table
depends_on: 
---

CREATE TABLE Servers (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	Entry TEXT NOT NULL,
	Server TEXT NOT NULL,
	User TEXT NOT NULL,
	Password TEXT,
	Port TEXT NOT NULL,
	PrivateKey TEXT,
	SudoPassword TEXT
);
