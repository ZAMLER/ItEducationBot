CREATE TABLE Questions (
Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
Name NVARCHAR(255),
RightAnswer BIGINT NULL,
)



CREATE TABLE Answers (
Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
Name NVARCHAR(255),
QuestionId BIGINT NOT NULL,
FOREIGN KEY (QuestionId) REFERENCES Questions(Id)
)