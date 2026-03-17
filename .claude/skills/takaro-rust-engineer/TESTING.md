# Testing

## Deploy & Reload Workflow

```bash
# 1. Edit plugin/TakaroConnector.cs
# 2. Deploy to server
./scripts/deploy.sh

# 3. Hot-reload (no restart needed)
./scripts/reload.sh

# 4. Check logs
./scripts/logs.sh
```

## Verifying Connection

After deploy + reload, check logs for this sequence:

```
[Takaro] Connecting to wss://connect.takaro.io/
[Takaro] WebSocket connected
[Takaro] Received server hello, sending identify...
[Takaro] Identified successfully, server ID: <uuid>
[Takaro] Authentication confirmed
```

If you see auth errors, check `TAKARO_REGISTRATION_TOKEN` in `.env`.

## Testing Actions via RCON

You can test some actions manually via RCON:

```bash
# Check server status
./scripts/reload.sh "status"

# List players
./scripts/reload.sh "playerlist"

# Server info
./scripts/reload.sh "serverinfo"
```

## Testing via Takaro

Once connected, use Takaro dashboard or MCP tools to verify:

1. Server appears as connected (generic connector type)
2. `testReachability` returns connectable
3. `getPlayers` returns player list
4. Actions work: sendMessage, kick, ban, etc.
5. Events flow: connect/disconnect, chat, deaths

## Common Issues

| Symptom | Cause | Fix |
|---------|-------|-----|
| Plugin not loading | Carbon not loaded | Rebuild image: `docker compose build && docker compose up -d` |
| WebSocket won't connect | Bad URL or token | Check `.env` values |
| Auth error (4001/4003) | Invalid registration token | Get new token from Takaro |
| Actions fail | Main thread issue | Check Carbon logs for exceptions |
| No events | Plugin not loaded | Check `c.plugins` via RCON |
