# Docker Dev Server

## Service

| Service | Platform | Game Port | RCON Port | Container |
|---------|----------|-----------|-----------|-----------|
| rust | Rust + Carbon | 28015/udp | 28016 | rust-carbon |

## First-Time Setup

```bash
cp .env.example .env
# Fill in TAKARO_WS_URL, TAKARO_REGISTRATION_TOKEN

# Build and start server (first build downloads Rust ~6GB)
docker compose up -d rust
```

The custom Dockerfile (Ubuntu 22.04) installs SteamCMD, Rust server, and Carbon during the image build. No separate Carbon install step needed.

## Configuration

Requires `.env` file with Takaro credentials:

```bash
TAKARO_WS_URL=wss://connect.takaro.io/
TAKARO_REGISTRATION_TOKEN=your-token-here
TAKARO_IDENTITY_TOKEN=takaro-rust-dev
TAKARO_DEBUG=false
RCON_PASSWORD=takaro123
```

### Debug Logging

Set `TAKARO_DEBUG=true` in `.env` to see raw WebSocket messages in logs.

## RCON Access

Rust uses WebSocket RCON (not traditional TCP RCON). Port: 28016, Password: `takaro123`

```bash
# Reload plugin
./scripts/reload.sh

# Send custom RCON command
./scripts/reload.sh "status"
```

## Common Operations

```bash
docker compose logs --tail=50 rust        # View logs
docker compose restart rust               # Restart server
docker compose down                       # Stop everything
docker compose ps                         # Check status
```

## Data Directory

Server data is bind-mounted as separate volumes:
- `_data/plugins/` — Carbon plugins (deploy target, maps to `/rust/carbon/plugins/`)
- `_data/carbon-logs/` — Carbon logs (maps to `/rust/carbon/logs/`)
- `_data/server/` — World saves (maps to `/rust/server/`)

## Carbon Plugin Hot-Reload

Carbon supports reloading plugins without restarting the server:

```bash
./scripts/deploy.sh     # Copy plugin file
./scripts/reload.sh     # RCON: c.reload TakaroConnector
```
