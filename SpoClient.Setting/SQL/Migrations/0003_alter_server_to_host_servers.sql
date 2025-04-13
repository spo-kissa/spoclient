---
name: 0003_alter_server_to_host_servers
description: Alter server column names in Servers table
depends_on: 0001_create_servers
---

ALTER TABLE Servers RENAME COLUMN Server TO Host;
