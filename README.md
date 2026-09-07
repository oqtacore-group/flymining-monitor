# FlyMining Monitor

A Windows desktop application for running a Bitcoin mining farm: it discovers ASIC miners on the
local network, polls each one over its control API, records a time series of hashrate, temperature
and uptime, and reconfigures pools across the fleet remotely. Published as a reference for how we
operate SHA-256 mining hardware at the protocol/API level.

## What it does

### Discovers the fleet

The tool sweeps configurable IP ranges (see the add-range dialog) to find miners on the LAN,
rather than being handed a static list. New units are picked up as they come online.

### Talks to the miner control API

Each ASIC (Antminer-class hardware) exposes a **CGMiner / bmminer-style JSON API** — the same
interface configured by the `bmmminer.conf` in this repo (`api-listen`, `api-network`,
`api-groups`, `api-allow`). The monitor speaks that API directly. It authenticates to the miner's
web/control endpoint and issues the standard command set:

- **read** — `summary`, `stats`, `pools`, `devs`, `version` — to pull hashrate (instantaneous and
  average), per-board temperatures, fan speeds, elapsed uptime and pool status;
- **control** — `addpool`, `removepool`, `enablepool`, `disablepool`, `switchpool` — to change
  where the whole farm points, one unit at a time or in bulk.

Understanding this API is the core competence the tool demonstrates: it manages Stratum pool
configuration and worker naming across many machines, which is exactly what running a mining
operation requires.

### Records a time series

Readings are written to a local SQLite database (miner MAC / IP as the key, plus hashrate,
temperatures, fan speeds, pool and worker fields, and a timestamp), so the farm's health and
output can be tracked over time and problems traced back.

### Reports to the FlyMining backend

The monitor synchronises status to the FlyMining platform's API (hashrate history, program status,
table data and problem reports), which is how per-user mining revenue was reconciled against real
hardware output.

## Repository layout

The application is a C# WinForms project (`WindowsFormsApplication1/` in the original layout, and a
service-style `src/` variant). Notable files:

| File | Role |
|---|---|
| `Kernel.cs` | Builds the authenticated miner-API requests and parses `summary` / `pools` / `stats` responses. |
| `MinerFunc.cs`, `MinerChangeFunc.cs` | Fleet-wide read and pool-reconfiguration logic. |
| `MinerDB.cs`, `Sql.cs` | SQLite persistence of the reading time series. |
| `Wallets.cs` | Balance and exchange-order helpers. |
| `MyHTTPServer.cs` | Small embedded HTTP endpoint exposing current status. |
| `bmmminer.conf` | Reference miner-side API configuration. |

## Technology

C# / .NET, WinForms, SQLite, the CGMiner/bmminer JSON API over HTTP.

## Status

Archived snapshot, kept as a reference. Not maintained. The default `root` miner login it uses is
the vendor default for this hardware, not a secret.
