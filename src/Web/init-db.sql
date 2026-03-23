CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL,
    PasswordHash TEXT NOT NULL,
    IsAdmin INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS FileEntries (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FileName TEXT NOT NULL,
    ContentType TEXT NOT NULL,
    Path TEXT NOT NULL,
    FileUrl TEXT NOT NULL,
    FileType TEXT NOT NULL,
    Size INTEGER NOT NULL,
    UploadedAt DATETIME NOT NULL,
    UploadedById INTEGER NOT NULL,
    FOREIGN KEY (UploadedById) REFERENCES Users(Id)
);

CREATE TABLE IF NOT EXISTS AudioFiles (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Author TEXT NOT NULL,
    Album TEXT,
    Duration INTEGER NOT NULL,
    IconUrl TEXT,
    FileUrl TEXT NOT NULL,
    FOREIGN KEY (IconUrl) References FileEntries(FileUrl),
    FOREIGN KEY (FileUrl) References FileEntries(FileUrl)
);
