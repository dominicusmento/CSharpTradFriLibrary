# Changelog
All notable changes to this project will be documented in this file, starting with version 1.0.1.x

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [2.0.0] - 2026-08-06
### Changed by [@mjwsteenbergen](https://github.com/mjwsteenbergen)
- Bumped target framework from `net8.0` to `net10.0` (`TradfriTerminalUI` too, since it references Dirigera directly), verified building on the installed .NET 10 SDK; consumers need a .NET 10 runtime
- Added new device support: `Outlet`, `MotionSensor`, `WaterSensor`, `LightSensor` and dimmable lights
- Added a `CO2` attribute plus a large batch of additional device attributes
- Added `EventController` for real-time updates over the Dirigera websocket, including keep-alive ping/pong messages
- Reworked the device model to a generic `DeviceAttributes`-based approach (see **Breaking changes** below)
- Added retry support for `ChangeAttributes` calls
- Added the ability to combine multiple attribute changes into a single request
- Mitigated a `TaskCanceledException` that could surface during long-running requests
- Added a `GetName` helper method on devices
- Updated `TradfriTerminalUI` to exercise the new device types and events
- Cleaned up build warnings: pinned `RestSharp` to `112.0.0` and `Newtonsoft.Json` to `13.0.3` (both fix known vulnerabilities pulled in transitively via ApiLibs), pinned `Microsoft.Bcl.AsyncInterfaces` to `10.0.0` to resolve a version conflict, switched from the deprecated `PackageLicenseUrl` to `PackageLicenseFile`, and fixed a few nullable-reference warnings in `EventController`/`Event`

### Breaking changes
- Namespaces reorganized: controllers moved to `Tomidix.NetStandard.Dirigera.Controller` (was the root namespace) and device models moved to `Tomidix.NetStandard.Dirigera.Model.Devices` (was `...Dirigera.Devices`). Update `using` statements accordingly.
- `Device` and its subclasses were rewritten around the new `DeviceAttributes` pattern; code that accessed the old ad-hoc device properties will need to be updated to read `device.Attributes.*` instead.

### Known issue
- This build depends on unreleased `ApiLibs` APIs (git submodule pinned past the `ApiLibs 2.0.0` NuGet release) and **must not be pushed to the public NuGet feed** until a compatible `ApiLibs` version is published — otherwise the published `.nuspec` would declare a dependency on `ApiLibs 2.0.0`, which is missing APIs this build actually needs, and consumers would hit a `MissingMethodException` at runtime. See root [CHANGELOG.md](../CHANGELOG.md) for details.

## [1.0.1.x] - 2024-07-07
### Changed by [@mjwsteenbergen](https://github.com/mjwsteenbergen) 
- Initial Dirigera project version by [@mjwsteenbergen]