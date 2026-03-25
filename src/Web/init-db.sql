CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    IsAdmin INTEGER NOT NULL DEFAULT 0,
    
    CHECK (IsAdmin IN (0, 1))
);


CREATE TABLE IF NOT EXISTS FileEntries (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FileName TEXT NOT NULL,
    ContentType TEXT NOT NULL,
    Path TEXT NOT NULL,
    FileUrl TEXT NOT NULL UNIQUE,
    FileType TEXT NOT NULL,
    Size INTEGER NOT NULL CHECK (Size >= 0),
    UploadedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UploadedById INTEGER NOT NULL,

    FOREIGN KEY (UploadedById) REFERENCES Users(Id) ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS idx_fileentries_uploadedby ON FileEntries(UploadedById);
CREATE INDEX IF NOT EXISTS idx_fileentries_filetype ON FileEntries(FileType);
CREATE INDEX IF NOT EXISTS idx_fileentries_url ON FileEntries(FileUrl);


CREATE TABLE IF NOT EXISTS AudioFiles (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Author TEXT NOT NULL,
    Album TEXT,

    Duration INTEGER NOT NULL CHECK (Duration >= 0),

    FileId INTEGER NOT NULL,
    IconId INTEGER,
    AlbumArtId INTEGER,

    FOREIGN KEY (FileId) REFERENCES FileEntries(Id) ON DELETE RESTRICT,
    FOREIGN KEY (IconId) REFERENCES FileEntries(Id) ON DELETE SET NULL,
    FOREIGN KEY (AlbumArtId) REFERENCES FileEntries(Id) ON DELETE SET NULL
);

CREATE INDEX IF NOT EXISTS idx_audiofiles_fileid ON AudioFiles(FileId);
CREATE INDEX IF NOT EXISTS idx_audiofiles_iconid ON AudioFiles(IconId);
CREATE INDEX IF NOT EXISTS idx_audiofiles_albumartid ON AudioFiles(AlbumArtId);

-- CREATE TABLE IF NOT EXISTS RadioStations (
--     Id INTEGER PRIMARY KEY AUTOINCREMENT,
--     AudioFileId INTEGER NOT NULL,
--     Position INTEGER NOT NULL DEFAULT 0,


--     FOREIGN KEY (AudioFileId) REFERENCES AudioFiles(Id) ON DELETE CASCADE,
-- )