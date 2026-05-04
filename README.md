# MyCSharpProject - Doc de hieu nhanh (cho nguoi moi)

Tai lieu nay giai thich tung file chinh trong project va flow chay app theo cach de hieu.

## 1) Muc tieu project

Day la ASP.NET Core Minimal API de quan ly game store.

- API CRUD cho Games
- Luu du lieu bang Entity Framework Core
- Dang chuyen sang SQL Server/Azure SQL
- Tu dong migration khi app start

## 2) Cau truc va y nghia tung file

### 2.1 File goc workspace

- `WebApplication1.sln`
  - Solution file de mo va quan ly project trong Visual Studio/VS Code.

- `.gitignore`
  - Bo qua file build tam (`bin`, `obj`), file local IDE, file database local.

- `README.md`
  - Tai lieu huong dan tong hop (file ban dang doc).

### 2.2 Nhom API app (`WebApplication1/`)

- `Program.cs`
  - Diem vao chinh cua app.
  - Lam cac viec:
    - Tao builder
    - Dang ky validation
    - Dang ky DbContext (`AddGameStoreDb()`)
    - Dang ky OpenAPI (`AddOpenApi()`)
    - Build app
    - Map OpenAPI o moi truong Development
    - Map endpoint games (`MapGamesEndpoints()`)
    - Chay migration tu dong (`MigrationDb()`)
    - `app.Run()` de start web server

- `WebApplication1.csproj`
  - Danh sach package va target framework (`net10.0`).
  - Package quan trong:
    - `Microsoft.EntityFrameworkCore.SqlServer` (provider SQL Server)
    - `Microsoft.EntityFrameworkCore.Design` (ho tro migration)
    - `Microsoft.AspNetCore.OpenApi`
    - `MinimalApis.Extensions`

- `appsettings.json`
  - Cau hinh runtime chung.
  - `ConnectionStrings:GameStore` la chuoi ket noi den SQL Server/Azure SQL.

- `appsettings.Development.json`
  - Cau hinh rieng khi chay Development.

- `WebApplication1.http`
  - Mau request test co san (template mac dinh).

- `games.http`
  - Bo request test API games: GET/POST/PUT/DELETE.

- `WebApplication1.db`
  - File SQLite cu. Hien tai project da doi sang SQL Server.
  - Co the xoa neu khong dung nua.

### 2.3 Nhom Data (`WebApplication1/Data/`)

- `GameStoreContext.cs`
  - DbContext cua EF Core.
  - Khai bao `DbSet<Game>` va `DbSet<Genre>`.
  - Co `OnModelCreating` de set `Price` thanh `decimal(10,2)`.

- `DataExtensions.cs`
  - Extension methods cho setup data:
    - `AddGameStoreDb(this WebApplicationBuilder builder)`
      - Doc connection string
      - Dang ky `AddSqlServer<GameStoreContext>`
      - Seed genre mac dinh neu bang `Genres` dang rong
    - `MigrationDb(this WebApplication app)`
      - Tao scope
      - Lay `GameStoreContext`
      - Goi `db.Database.Migrate()` de apply migration con thieu

#### Migrations (`WebApplication1/Data/Migrations/`)

- `20260504013902_InitialCreate.cs`
  - Lenh tao schema:
    - Bang `Genres`
    - Bang `Games`
    - FK `Games.GenreId -> Genres.Id`
    - Index cho `GenreId`

- `20260504013902_InitialCreate.Designer.cs`
  - File designer auto-generated cho migration tren.

- `GameStoreContextModelSnapshot.cs`
  - Snapshot model hien tai cua EF Core.
  - EF dung file nay de so sanh va tao migration moi khi model thay doi.

### 2.4 Nhom Entity (`WebApplication1/Entities/`)

- `Game.cs`
  - Entity game trong DB:
    - `Id`, `Name`, `GenreId`, `Genre`, `Price`, `ReleaseDate`

- `Genre.cs`
  - Entity genre trong DB:
    - `Id`, `Name`

### 2.5 Nhom DTO (`WebApplication1/Dtos/`)

- `CreateGameDto.cs`
  - Du lieu client gui khi tao game.
  - Co DataAnnotations validate (`Required`, `StringLength`, `Range`).

- `UpdateGameDto.cs`
  - Du lieu client gui khi sua game.
  - Validate tuong tu create.

- `GameDto.cs`
  - Du lieu API tra ve cho client.

### 2.6 Nhom Endpoint (`WebApplication1/Endpoints/`)

- `GamesEndPoints.cs`
  - Khai bao route group `/games`.
  - Endpoint:
    - `GET /games` -> lay tat ca game
    - `GET /games/{id}` -> lay 1 game
    - `POST /games` -> tao game moi
    - `PUT /games/{id}` -> cap nhat game
    - `DELETE /games/{id}` -> xoa game
  - Da dung `GameStoreContext` truc tiep (khong con list fake trong RAM).

### 2.7 Nhom launch profile

- `Properties/launchSettings.json`
  - Cau hinh profile chay local (HTTP/HTTPS) va port.

## 3) Flow chay app (nhin 1 lan la nho)

1. App start tai `Program.cs`.
2. `AddGameStoreDb()` dang ky DbContext + SQL Server + seed.
3. Build app.
4. `MapGamesEndpoints()` dang ky route `/games`.
5. `MigrationDb()` chay `Database.Migrate()` de dong bo schema DB.
6. App listen request.
7. Khi goi API `/games`, endpoint truy van/lưu du lieu qua EF Core vao SQL Server.

## 4) Flow cho tung request API

Vi du `POST /games`:

1. Client gui JSON.
2. ASP.NET bind vao `CreateGameDto`.
3. Validation chay (Required, Range, StringLength).
4. Endpoint tim `Genre` theo ten.
5. Tao `Game` entity, `db.Games.Add()`.
6. `db.SaveChangesAsync()`.
7. Tra ve `201 Created` + du lieu `GameDto`.

## 5) Nhung diem can luu y hien tai

- Connection string trong `appsettings.json` dang la placeholder, can thay gia tri that.
- Khong nen hard-code password production trong file json.
- Nen dung User Secrets (local) hoac Environment Variables/Azure Key Vault (production).
- `WebApplication1.db` la du lieu SQLite cu, co the xoa neu da chot SQL Server.

## 6) Cach chay local nhanh

1. Dien connection string that trong `appsettings.json`.
2. Chay:

```powershell
dotnet restore
dotnet build
dotnet run --project WebApplication1
```

3. Test bang file `games.http`.

## 7) Cach hieu project nhu nguoi moi

- Entity = hinh dang bang trong database.
- DTO = hinh dang du lieu vao/ra API.
- Endpoint = cua API.
- DbContext = cau noi giua C# va database.
- Migration = lich su thay doi cau truc database.

Neu ban muon, minh co the viet tiep phan "Doc theo dong" cho tung endpoint trong `GamesEndPoints.cs` theo kieu 1 dong code = 1 dong giai thich.
