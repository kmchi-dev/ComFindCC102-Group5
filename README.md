# ComFind

ComFind is a search engine project built using **C#**, **ASP.NET Core**, and **SearXNG**.

The ComFind API handles search requests, while SearXNG communicates with external search engines and provides the search results.

---

## Requirements

Before running ComFind, install the following:

* Git
* .NET 10 SDK
* Docker Desktop
* WSL 2

> **Operating System:** Windows

---

# 1. Install Git

Git is used to download the ComFind source code from GitHub.

### Download

Download Git from:

https://git-scm.com/downloads

Install Git using the default installation options.

### Check the installation

Open **PowerShell** or **Command Prompt** and run:

```powershell
git --version
```

If a Git version is displayed, the installation was successful.

---

# 2. Install .NET 10 SDK

ComFind is built using **.NET 10**.

### Download

Download the **.NET 10 SDK** from:

https://dotnet.microsoft.com/download/dotnet/10.0

Make sure to install the **SDK**, not only the Runtime.

### Check the installation

Open a new PowerShell or Command Prompt window and run:

```powershell
dotnet --version
```

The version should begin with:

```text
10.
```

For example:

```text
10.0.xxx
```

---

# 3. Install Docker Desktop

Docker is used to run the SearXNG search backend and Valkey service.

### Download

Download Docker Desktop from:

https://www.docker.com/products/docker-desktop/

Install Docker Desktop using the recommended settings.

After installation, open Docker Desktop and wait for it to finish starting.

### Check Docker

Open PowerShell or Command Prompt and run:

```powershell
docker --version
```

Then:

```powershell
docker compose version
```

Both commands should display version information.

---

# 4. Install WSL 2

Docker Desktop uses WSL 2 to run Linux-based containers on Windows.

Open **PowerShell as Administrator** and run:

```powershell
wsl --install
```

Restart your computer if Windows asks you to.

After restarting, check WSL:

```powershell
wsl --status
```

You can also run:

```powershell
wsl -l -v
```

WSL distributions should use **version 2**.

---

# 5. Download the ComFind Project

Open PowerShell or Command Prompt.

Go to the location where you want to store the project.

For example:

```powershell
cd Desktop
```

Clone the repository:

```powershell
git clone YOUR_GITHUB_REPOSITORY_LINK
```

Replace `YOUR_GITHUB_REPOSITORY_LINK` with the ComFind GitHub repository URL.

Enter the project folder:

```powershell
cd ComFind
```

---

# 6. Project Structure

The project is organized as follows:

```text
ComFind/
│
├── ComFind.Api/
│   ├── bin/
│   ├── obj/
│   ├── Properties/
│   ├── wwwroot/
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── ComFind.Api.csproj
│   ├── ComFind.Api.http
│   └── Program.cs
│
└── Database/
    └── SearXNG/
        ├── core-config/
        │   └── settings.yml
        ├── .env
        └── docker-compose.yml
```

### Important files

#### `ComFind.Api/`

Contains the ASP.NET Core API.

#### `Database/SearXNG/docker-compose.yml`

Defines the Docker services used by ComFind.

#### `Database/SearXNG/.env`

Contains the SearXNG Docker configuration values.

#### `Database/SearXNG/core-config/settings.yml`

Contains the SearXNG configuration, including the enabled search engines.

---

# 7. Start Docker Desktop

Open **Docker Desktop**.

Wait until Docker has finished starting before continuing.

Keep Docker Desktop running while using ComFind.

---

# 8. Start SearXNG

Open a new PowerShell or Command Prompt window.

Navigate to the SearXNG directory:

```powershell
cd ComFind\Database\SearXNG
```

Start the Docker services:

```powershell
docker compose up -d
```

Docker Compose will automatically download and start the required containers.

ComFind currently uses:

* **SearXNG**
* **Valkey**

You do not need to install either of these separately.

---

# 9. Check the Docker Containers

Run:

```powershell
docker ps
```

You should see containers for:

```text
searxng-core
searxng-valkey
```

If both containers are running, the SearXNG backend is ready.

---

# 10. Test SearXNG

Open a web browser and go to:

```text
http://localhost:8080
```

The SearXNG interface should appear.

The instance should be labeled:

```text
ComFind Search
```

If the page loads successfully, the SearXNG setup is working.

---

# 11. Run the ComFind API

Keep the SearXNG Docker containers running.

Open **another** PowerShell or Command Prompt window.

Navigate to the API:

```powershell
cd ComFind\ComFind.Api
```

Restore the project dependencies:

```powershell
dotnet restore
```

Build the project:

```powershell
dotnet build
```

If the build completes without errors, start the API:

```powershell
dotnet run
```

The terminal will display the URL where the API is running.

For example:

```text
Now listening on: http://localhost:5000
```

The actual port may be different.

---

# 12. Test ComFind

Once the API is running, use the API address shown in the terminal.

For example:

```text
http://localhost:5000/search?q=computer
```

Replace the port with the one displayed by `dotnet run`.

The ComFind API should process the search request and communicate with the SearXNG backend.

---

# 13. Running ComFind Again

After the initial installation, you do not need to reinstall anything.

Every time you want to run the project:

### Start Docker Desktop

Make sure Docker Desktop is running.

### Start SearXNG

```powershell
cd ComFind\Database\SearXNG
docker compose up -d
```

### Start the API

Open another terminal:

```powershell
cd ComFind\ComFind.Api
dotnet run
```

ComFind is now ready to use.

---

# 14. Stopping the Project

To stop the API, go to the terminal running `dotnet run` and press:

```text
Ctrl + C
```

To stop the SearXNG and Valkey containers:

```powershell
cd ComFind\Database\SearXNG
docker compose down
```

---

# Updating the Project

If changes have been pushed to GitHub, download the latest version using:

```powershell
cd ComFind
git pull
```

After updating, rebuild the API:

```powershell
cd ComFind.Api
dotnet build
```

---

# Troubleshooting

## Docker is not running

If Docker commands fail, make sure Docker Desktop is open and has finished starting.

You can test Docker with:

```powershell
docker info
```

---

## SearXNG is not starting

Check the running containers:

```powershell
docker ps
```

If necessary, restart the services:

```powershell
cd ComFind\Database\SearXNG
docker compose down
docker compose up -d
```

To view the container logs:

```powershell
docker compose logs
```

---

## Port 8080 is already in use

SearXNG uses port `8080`.

Check whether another program is using the port:

```powershell
netstat -ano | findstr :8080
```

If another program is using port `8080`, SearXNG may not start correctly.

---

## `dotnet` is not recognized

If PowerShell says:

```text
'dotnet' is not recognized...
```

make sure the **.NET 10 SDK** is installed.

Close and reopen PowerShell, then run:

```powershell
dotnet --version
```

---

## `git` is not recognized

If PowerShell says:

```text
'git' is not recognized...
```

make sure Git is installed.

Close and reopen PowerShell, then run:

```powershell
git --version
```

---

## WSL is not working

Check the WSL installation:

```powershell
wsl --status
```

Then:

```powershell
wsl -l -v
```

If WSL is not installed, open PowerShell as Administrator and run:

```powershell
wsl --install
```

Restart the computer afterward.

If Docker still cannot start, make sure CPU virtualization is enabled in the computer's BIOS/UEFI settings.

---

# Technologies Used

| Technology     | Purpose                                  |
| -------------- | ---------------------------------------- |
| C#             | Main programming language                |
| ASP.NET Core   | Web API                                  |
| .NET 10        | Development framework                    |
| SearXNG        | Search backend                           |
| Docker         | Container platform                       |
| Docker Compose | Runs and manages the services            |
| Valkey         | Supporting data service for SearXNG      |
| WSL 2          | Linux environment used by Docker Desktop |
| Git            | Version control                          |
| GitHub         | Source code hosting                      |

---

# Quick Start

For group members who have already installed all requirements:

```text
1. Open Docker Desktop
        ↓
2. Open ComFind\Database\SearXNG
        ↓
3. Run:
   docker compose up -d
        ↓
4. Open ComFind\ComFind.Api
        ↓
5. Run:
   dotnet run
        ↓
6. Open the API URL shown in the terminal
```

---

# Important

Do not commit passwords, API keys, private credentials, or other sensitive information to GitHub.

Before pushing the project, check the repository and `.gitignore` to make sure generated files and sensitive configuration are not being uploaded.
