# Background polling test mode

This console harness exercises the production `MeltPollingBackgroundService` with
two local SQLite source databases instead of Sybase and Oracle. Every run recreates:

- `test-sybase.db` with two fake melt records;
- `test-oracle.db` with three fake order-position records;
- `melts.db`, the local target, with one deliberately stale record.

The hosted background service polls both source databases, joins records on the melt
number (`SybaseMelt.Me_num == OracleMelt.Nplav`), replaces the stale local row, inserts
the second melt, and preserves multiple Oracle positions as a newline-separated value.

In the WPF application, every successfully committed polling cycle is also published
to the existing three tabs. The Oracle and Sybase tabs display the remote snapshots
used by that cycle, while the local-copy tab reloads the joined records from
`melts.db`. Existing grid filters are retained during automatic refreshes.

The application's **Настройки** window now controls two persisted runtime options:

- **Тестовый режим** switches the remote tabs to SQLite-backed `fake-sybase.db` and
  `fake-oracle.db`, and switches the joined target to `test-melts.db`.
- **Фоновый опрос** enables or disables a polling cycle every three seconds.

The **Обновить данные** button always runs one cycle, even when background polling is
disabled. In test mode it recreates `test-melts.db` before joining the current fake
source rows. Subsequent automatic cycles update that database; the fake Sybase weight
changes each cycle and a third matching melt appears on the third cycle.

Run from this directory:

```powershell
dotnet run --project .\TestMode\BackgroundSync.TestMode.csproj
```

The command fails with a non-zero exit code if the target does not contain the two
expected merged records. Generated databases are written below the build output in
`TestMode/bin/Debug/net6.0/test-data`.
