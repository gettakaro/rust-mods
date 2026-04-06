# Repository Engineer Skill

This repository has an engineer skill at `.claude/skills/takaro-rust-engineer/`.

Claude will automatically discover and use this skill. The skill contains:
- `SKILL.md` — Overview and quick reference
- `ARCHITECTURE.md` — Plugin structure, event/action flows, Rust game APIs
- `DOCKER.md` — Dev server setup, RCON, hot-reload
- `TESTING.md` — Deploy/reload workflow, verification steps, troubleshooting

IMPORTANT DOCUMENTATION:

https://docs.takaro.io/advanced/adding-support-for-a-new-game
https://docs.takaro.io/advanced/connection-architecture
https://docs.takaro.io/advanced/generic-connector-protocol

## Project Overview

Single Carbon plugin (`plugin/TakaroConnector.cs`) implementing the Takaro Generic Connector Protocol for Rust game servers. Connects outbound via WebSocket to Takaro — no port forwarding needed.

## Dev Workflow

```bash
# First time setup
cp .env.example .env          # Fill in credentials
docker compose up -d rust     # Build and start Rust server (Carbon baked into image)

# Development cycle
# Edit plugin/TakaroConnector.cs
./scripts/deploy.sh           # Copy to server
./scripts/reload.sh           # Hot-reload (no restart)
./scripts/logs.sh             # Check logs
```
