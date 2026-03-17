#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."

LINES="${1:-100}"
docker compose logs --tail="$LINES" -f rust
