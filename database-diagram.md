# Database Schema — Escape Room Platform

> Rendered with Obsidian's built-in Mermaid support (switch to **Reading view** / open the diagram).
>
> Status: **User** and **Category** are implemented in the database. The remaining entities are the target schema and will be added model-by-model.

## Entity Relationship Diagram

```mermaid
erDiagram
    USER ||--o{ GAME : creates
    CATEGORY ||--o{ GAME : groups
    GAME ||--o{ PUZZLE : contains
    PUZZLE ||--o{ HINT : offers
    GAME ||--o{ GAME_SESSION : "is played in"
    TEAM }o--o{ GAME_SESSION : "plays (team play)"
    TEAM ||--o{ TEAM_MEMBER : "has"
    USER ||--o{ TEAM_MEMBER : "joins as"
    GAME_SESSION ||--o{ PUZZLE_ATTEMPT : records
    PUZZLE ||--o{ PUZZLE_ATTEMPT : "receives"
    GAME_SESSION ||--o| GAME_RESULT : produces
    USER ||--o{ REPORT : submits

    USER {
        string Id PK "nvarchar(450), GUID"
        string UserName
        string Email
        string PasswordHash
        string PhoneNumber
    }

    CATEGORY {
        string Id PK "nvarchar(450), GUID"
        string Name "unique, max 60"
        string Description "nullable, max 2500"
    }

    GAME {
        string Id PK
        string Title "max 200"
        string Description "max 2500"
        string CoverImageUrl "nullable"
        int Difficulty "enum: Easy / Medium / Hard"
        int Status "enum: Draft / PendingReview / Published / Unpublished / Rejected"
        int TimeLimitMinutes
        string CategoryId FK
        string AuthorId FK "to AspNetUsers"
        datetime CreatedAt
    }

    PUZZLE {
        string Id PK
        string GameId FK
        string Title "max 200"
        string Question "max 2000"
        string AnswerHash "protected, never sent to client"
        int CheckType "enum: ExactMatch / CaseInsensitive / Numeric / NormalizedText"
        int Points
        int Order "unlock sequence"
    }

    HINT {
        string Id PK
        string PuzzleId FK
        string Text "max 1000"
        int PenaltyPoints "deducted from final score"
        int Order
    }

    GAME_SESSION {
        string Id PK
        string GameId FK
        string TeamId FK "nullable — solo play"
        datetime StartedAt
        datetime FinishedAt "nullable"
        int Status "enum: Active / Finished / Expired"
        int CurrentPuzzleOrder "player progress"
    }

    TEAM {
        string Id PK
        string Name
        string JoinCode "code used to join the team"
    }

    TEAM_MEMBER {
        string Id PK
        string TeamId FK
        string UserId FK "to AspNetUsers"
        datetime JoinedAt
    }

    PUZZLE_ATTEMPT {
        string Id PK
        string GameSessionId FK
        string PuzzleId FK
        string SubmittedAnswer
        boolean IsCorrect
        datetime AttemptedAt
    }

    GAME_RESULT {
        string Id PK
        string GameSessionId FK
        int Score "points minus hint penalties"
        int HintsUsed
        int CompletionTimeSeconds
        datetime FinishedAt
    }

    REPORT {
        string Id PK
        string ReporterId FK "to AspNetUsers"
        int TargetType "enum: Game / User"
        string TargetId "Id of the reported entity"
        string Reason "max 1000"
        int Status "enum: Open / Resolved / Dismissed"
        datetime CreatedAt
    }
```

## Implementation status

| Entity        | Table                           | Status                        |
| ------------- | ------------------------------- | ----------------------------- |
| User          | AspNetUsers (+ Identity tables) | Implemented — `InitialCreate` |
| Category      | Categories                      | Implemented — `AddedCategory` |
| Game          | Games                           | Planned                       |
| Puzzle        | Puzzles                         | Planned                       |
| Hint          | Hints                           | Planned                       |
| GameSession   | GameSessions                    | Planned                       |
| Team          | Teams                           | Planned                       |
| TeamMember    | TeamMembers                     | Planned                       |
| PuzzleAttempt | PuzzleAttempts                  | Planned                       |
| GameResult    | GameResults                     | Planned                       |
| Report        | Reports                         | Planned                       |

## Design notes

- All primary keys are **string GUIDs** (`Guid.NewGuid().ToString()` in the constructor), matching ASP.NET Core Identity's string user IDs.
- **AnswerHash** stores the protected answer representation server-side; it is never sent to the client before the puzzle is solved (anti-cheating requirement).
- **Hint.PenaltyPoints** is deducted from the final score when a hint is used (scoring rule).
- **Puzzle.Order** drives the sequential unlock: a correct answer unlocks the puzzle with the next order value.
- Deleting a **Category** must not cascade to its **Games** (Restrict); deleting a **Game** cascades to its **Puzzles** and **Hints**.
