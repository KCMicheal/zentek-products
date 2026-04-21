# ZenTek Products API

A .NET 8 Web API for managing products with JWT authentication.

## Features

- **Anonymous Health Check** (`GET /health`)
- **JWT Authentication** (`POST /api/auth/login`)
- **Products CRUD** (secured with JWT)
  - `GET /api/products` - List all products
  - `GET /api/products?colour={colour}` - Filter by colour
  - `POST /api/products` - Create product
- **Database** with Entity Framework Core
  - Local dev defaults to **SQLite** (`src/ZentekProducts.Api/products.db`)
  - Docker Compose uses **SQL Server**
- **Unit Tests** (xUnit with InMemory provider)
- **Integration Tests** (HTTP client tests)
- **React Frontend** with Vite
- **Docker** support

## Getting Started

### Local Development

1. Run the API:
```bash
cd src/ZentekProducts.Api
dotnet run
```

2. Run the frontend (Vite dev server):
```bash
cd src/ZentekProducts.Client
npm install
npm run dev
```

3. Open the app:
- API (Swagger): `http://localhost:5000/swagger/index.html`
- Client: `http://localhost:5173`

#### Notes

- **CORS**: the API is configured for local development so the Vite dev server on localhost can call it.
- **If login fails**, the client surfaces the actual error (CORS/network vs 401) rather than always showing “Invalid credentials”.

### Docker Compose

```bash
docker compose up --build
```

- API: http://localhost:5000
- Frontend: http://localhost:3000

#### Hybrid (Docker + Vite dev server)

If you want a single Docker command that starts the API + DB + a containerized Vite dev server (hot reload):

```bash
docker compose --profile dev up --build
```

- API: `http://localhost:5000`
- Client (Vite): `http://localhost:5173`

Notes:
- The compose file includes an **API healthcheck**, and the clients wait until the API is healthy before starting.
- The production Docker client is served by nginx on port 3000 and proxies `/api/*` to the API container.

### Default Credentials

- Username: `admin`
- Password: `admin123`

## Architecture

```mermaid
flowchart LR
    Client[React Client\nDev: 5173 / Docker: 3000] -->|HTTP/REST| ApiGateway[API\nPort 5000]
    
    subgraph "Products Service"
        ApiGateway -->|JWT Auth| ProductsAPI[Products API\n.NET 8]
        ProductsAPI -->|EF Core| SQL[(SQL Server (Compose)\nPort 1433)]
    end
    
    subgraph "Event Bus"
        EventBus[Event Bus\nKafka/RabbitMQ]
    end
    
    ProductsAPI -->|ProductCreated Event| EventBus
    
    subgraph "Other Services"
        OrdersAPI[Orders Service\n.NET]
        PaymentsAPI[Payments Service\n.NET]
        NotificationAPI[Notification Service\nNode.js]
    end
    
    EventBus -->|Process Order| OrdersAPI
    EventBus -->|Process Payment| PaymentsAPI
    EventBus -->|Send Notification| NotificationAPI
```

## API Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/health` | Anonymous | Health check |
| POST | `/api/auth/login` | Anonymous | Get JWT token |
| GET | `/api/products` | JWT | List all products |
| GET | `/api/products?colour={col}` | JWT | Filter by colour |
| POST | `/api/products` | JWT | Create product |

## Project Structure

```
zentek/
├── src/
│   ├── ZentekProducts.Api/      # .NET 8 Web API
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Services/
│   │   └── Data/
│   └── ZentekProducts.Client/  # React + Vite
├── tests/
│   ├── ZentekProducts.Tests/           # Unit tests
│   └── ZentekProducts.IntegrationTests/  # Integration tests
├── docker-compose.yml
└── README.md
```