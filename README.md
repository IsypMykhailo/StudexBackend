# Studex - Backend

## Overview
The backend for Studex - the AI powered studying assistant. It is built using modern web technologies to ensure scalability, reliability, and efficiency.

## Tech Stack
- **Programming Language**: C#
- **Framework**: ASP.NET Core
- **Database**: PostgreSQL
- **AI Models**: Hugging Face API
- **Containerization**: Docker

## Setup Instructions
### Prerequisites
- .NET SDK installed (v7.0+)
- PostgreSQL database setup
- Docker (if deploying)

### Installation
1. Clone the repository:
   ```sh
   git clone <repository-url>
   ```

2. Install dependencies:
   ```sh
   dotnet restore
   ```

3. Configure environment variables:
   Create a `appsettings.json` file in the root directory with the following values:
   ```json
   "Database": {
    "ConnectionString": "",
    "PoolSize": 10
   },
   "Auth0": {
    "Domain": "",
    "Audience": ""
   },
   "HuggingFace": {
    "ApiKey": "",
    "Model": ""
   }
   ```

4. Run the application:
   ```sh
   dotnet run
   ```

5. (Optional) Run using Docker:
   ```sh
   docker-compose up --build
   ```

## API Documentation
The API endpoints are documented using Swagger. Once the backend is running, access the API docs at:
```
http://localhost:8080/swagger/index.html
```

## Figma Design
For UI/UX reference, check the Figma design:
[Figma Link](https://www.figma.com/design/dxJ0CmTVVQsBqtiyozsY2O/Studex-AI?node-id=0-1&t=xi7D22y6QXbiFcin-1)

---
For further inquiries, contact **mikeisisyp@gmail.com**.

