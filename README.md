# ComFind

**ComFind** is a C#-based hardware search engine project designed to search and organize information from computer hardware websites.

The project uses an ASP.NET Core Web API as its backend, SQLite for storing crawled website data, and a custom web interface for displaying search results.

## Features

* Hardware-focused search
* Website and webpage database
* Web crawler
* `robots.txt` checking before crawling
* HTML title and text extraction
* Duplicate webpage prevention
* Search snippets
* Search API
* Search history database
* Entity Framework Core migrations
* Custom frontend using HTML, CSS, and JavaScript

## Project Structure

```text
ComFind/
├── ComFind.Api/
│   ├── Data/
│   │   └── ComFindDbContext.cs
│   │
│   ├── Migrations/
│   │   ├── InitialCreate.cs
│   │   ├── InitialCreate.Designer.cs
│   │   └── ComFindDbContextModelSnapshot.cs
│   │
│   ├── Models/
│   │   ├── SearchHistory.cs
│   │   ├── Website.cs
│   │   └── WebPage.cs
│   │
│   ├── Services/
│   │   └── CrawlerService.cs
│   │
│   ├── wwwroot/
│   │   ├── index.html
│   │   ├── script.js
│   │   └── style.css
│   │
│   ├── Program.cs
│   └── ComFind.Api.csproj
│
└── Database/
    └── comfind.db
```

## Technologies

### Backend

* C#
* ASP.NET Core
* .NET 10
* Entity Framework Core
* SQLite

### Web

* HTML
* CSS
* JavaScript

### Crawler

* HttpClient
* Html Agility Pack
* `robots.txt` support

## How It Works

ComFind stores websites and crawled webpages in a local SQLite database.

The general process is:

```text
User
 │
 ▼
ComFind Web Interface
 │
 ▼
ComFind API
 │
 ├──► Search Database
 │
 └──► Crawler
       │
       ├── Check robots.txt
       │
       ├── Request website
       │
       ├── Extract HTML content
       │
       └── Store webpage
```

When a user searches for a term, the API checks the stored webpage titles and content and returns matching results with a short snippet.

## Database

ComFind currently uses three main entities:

### Website

Stores information about websites that can be crawled.

```text
Id
Name
Url
RobotsTxtUrl
IsAllowedToCrawl
LastCrawled
```

### WebPage

Stores content collected from a website.

```text
Id
WebsiteId
Url
Title
Content
CrawledAt
```

### SearchHistory

Stores search queries made through the application.

## API Endpoints

### Check API Status

```http
GET /
```

Returns the current API status.

### Get Websites

```http
GET /api/websites
```

Returns the websites currently stored in the database.

### Add Website

```http
POST /api/websites
```

Adds a website to the database.

### Crawl Website

```http
POST /api/websites/{id}/crawl
```

Crawls a stored website after checking its `robots.txt` rules.

### Get Crawled Pages

```http
GET /api/webpages
```

Returns webpages stored in the database.

### Search

```http
GET /api/search?q=GPU
```

Searches the stored webpage titles and content.

Example:

```text
/api/search?q=GPU
```

The API returns the matching pages together with a search snippet.

### Delete Website

```http
DELETE /api/websites/{id}
```

Removes a website and its associated crawled webpages.

### Delete Webpage

```http
DELETE /api/webpages/{id}
```

Removes an individual crawled webpage.

## Database Setup

The project uses Entity Framework Core migrations.

To create/update the database:

```powershell
dotnet ef database update
```

To create a new migration after changing the database models:

```powershell
dotnet ef migrations add MigrationName
```

## Running the Project

Navigate to the API directory:

```powershell
cd ComFind.Api
```

Run the application:

```powershell
dotnet run
```

The API currently runs on:

```text
http://localhost:5298
```

You can test the API by opening:

```text
http://localhost:5298/
```

## Example Search

After crawling a website, search for a hardware-related term:

```powershell
Invoke-RestMethod `
    -Uri "http://localhost:5298/api/search?q=GPU" `
    -Method Get
```

The API returns the matching webpages and snippets.

## Crawler and robots.txt

Before crawling a website, ComFind checks its configured `robots.txt` file.

The crawler:

1. Checks whether crawling is enabled for the website.
2. Retrieves `robots.txt`.
3. Checks applicable `Disallow` rules.
4. Requests the website if crawling is allowed.
5. Extracts the page title.
6. Extracts readable page content.
7. Saves or updates the webpage in the database.

ComFind also prevents duplicate entries for the same webpage URL.

## Current Status

### Completed

* [x] ASP.NET Core API
* [x] SQLite database
* [x] Entity Framework Core
* [x] Database migrations
* [x] Website model
* [x] WebPage model
* [x] SearchHistory model
* [x] Website management endpoints
* [x] Webpage management endpoints
* [x] Web crawler
* [x] `robots.txt` checking
* [x] HTML content extraction
* [x] Duplicate webpage prevention
* [x] Database search
* [x] Search snippets

### In Progress

* [ ] Search history functionality
* [ ] Improved search ranking
* [ ] Multiple hardware websites
* [ ] Improved crawler/link handling
* [ ] Search pagination
* [ ] Frontend integration

### Optional

* [ ] SearXNG integration
* [ ] Additional search features
* [ ] AI/ML-assisted search

## Project Goal

The goal of ComFind is to create a simple, hardware-focused search engine that demonstrates how a search system can collect, store, and retrieve information from websites using a C# backend, database, crawler, and web interface.
