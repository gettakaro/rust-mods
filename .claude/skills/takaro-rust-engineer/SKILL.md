---
name: takaro-rust-engineer
description: "Takaro Rust connector repository knowledge — Carbon plugin architecture, Docker dev server, deploy/reload workflow, and development patterns. Use when working on this codebase."
---

# Takaro Rust Engineer

Carbon plugin that implements the Takaro Generic Connector Protocol for Rust game servers. Single `.cs` file — no build step.

## Quick Reference

| Area | File | Key Command |
|------|------|-------------|
| Architecture | [ARCHITECTURE.md](ARCHITECTURE.md) | — |
| Docker Dev Server | [DOCKER.md](DOCKER.md) | `docker compose up -d rust` |
| Testing | [TESTING.md](TESTING.md) | `./scripts/deploy.sh && ./scripts/reload.sh` |

## Architecture Overview

```
[Takaro Backend]
       ↕ (WebSocket - outbound from plugin)
[TakaroConnector.cs]  ← Single Carbon plugin
       ↕ (Direct C# game API access)
[Rust Dedicated Server + Carbon]
```

## Project Structure

```
takaro-rust/
├── plugin/
│   └── TakaroConnector.cs       # The Carbon plugin (single file)
├── scripts/
│   ├── deploy.sh                # Copy plugin to server
│   ├── reload.sh                # Hot-reload via RCON (requires Node.js v22+)
│   └── logs.sh                  # View server logs
├── Dockerfile                   # Custom Ubuntu 22.04 + SteamCMD + Rust + Carbon
├── start.sh                     # Container entrypoint (sources Carbon env)
├── docker-compose.yml
├── .env.example
├── .gitignore
├── CLAUDE.md
├── README.md
├── _data/                       # Runtime server data (gitignored)
│   ├── plugins/                 # Carbon plugins volume
│   ├── server/                  # World saves volume
│   └── carbon-logs/             # Carbon logs volume
└── .claude/
    └── skills/
        └── takaro-rust-engineer/
            ├── SKILL.md
            ├── ARCHITECTURE.md
            ├── DOCKER.md
            └── TESTING.md
```

## Dev Workflow

1. Edit `plugin/TakaroConnector.cs`
2. `./scripts/deploy.sh` — copies to server
3. `./scripts/reload.sh` — hot-reloads plugin via RCON (no server restart)
4. `./scripts/logs.sh` — check logs
