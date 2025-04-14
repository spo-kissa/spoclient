---
name: 0002_alter_entry_to_name_servers
description: Alter entry column names in Servers table
depends_on: 0001_create_servers
---

ALTER TABLE Servers RENAME COLUMN Entry TO Name;
