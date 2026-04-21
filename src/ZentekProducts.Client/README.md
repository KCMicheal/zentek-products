# Zentek Products Client

React + TypeScript + Vite frontend for the Zentek Products API.

## Local Development

Prerequisites:
- Node.js 20+
- API running on `http://localhost:5000`

```bash
npm install
npm run dev
```

Open `http://localhost:5173`.

## Docker

From the repo root you can start everything with Docker Compose:

```bash
docker compose up --build
```

- API: `http://localhost:5000`
- Client (nginx): `http://localhost:3000`

Or run the Vite dev server in Docker (hot reload):

```bash
docker compose --profile dev up --build
```

- API: `http://localhost:5000`
- Client (Vite): `http://localhost:5173`

## Build

```bash
npm run build
```

## Default Credentials

- Username: `admin`
- Password: `admin123`

## Notes

- The login screen surfaces the underlying error (CORS/network vs 401), which helps with debugging local setup issues.
- In the “Add Product” form, numeric fields (e.g. price) start blank and convert to numbers when submitting.
