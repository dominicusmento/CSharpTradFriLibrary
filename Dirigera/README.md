## C# Tradfri Library 
This is a .NET 10 library to communicate with the [IKEA Dirigera Hub](https://www.ikea.com/gb/en/p/dirigera-hub-for-smart-products-white-smart-50503409/) (Dirigera) smart home Gateway. Using this library you can, by communicating with the gateway, control devices. 

[![GitHub last commit](https://img.shields.io/github/last-commit/tomidix/CSharpTradFriLibrary.svg)]() [![NuGet downloads](https://img.shields.io/nuget/dt/Tomidix.Dirigera.svg)](https://www.nuget.org/packages/Tomidix.Dirigera) 

This library is still in development, latest version: 

[![NuGet downloads](https://img.shields.io/nuget/v/Tomidix.Dirigera.svg)](https://www.nuget.org/packages/Tomidix.Dirigera)

> **Preview build notice:** version `1.1.0-preview.1` builds against an unreleased `ApiLibs` commit (pulled in as a git submodule, see [Building from source](#building-from-source) below) and is **not yet pushed to the public NuGet feed**. See the [CHANGELOG](CHANGELOG.md) for details.

- Get information on the Dirigera hub and its users
- List and control devices: lights (on/off, dimming, color temperature), outlets, and dimmable lights
- Read sensor data from environment sensors, motion sensors, light sensors and water sensors, including the CO2 attribute
- Subscribe to real-time device updates over the Dirigera websocket (`EventController`), with automatic keep-alive ping/pong handling
- Automatic retries when changing device attributes

## Nuget
[Tomidix.Dirigera](https://www.nuget.org/packages/Tomidix.Dirigera)

## 1. Usage
Download the [NuGet package](https://www.nuget.org/packages/Tomidix.Dirigera). You will need:
- **hostUrl** the IP address of your Dirigera hub (just the IP, e.g. `192.168.0.123`, no scheme).
- **accessToken** a token obtained once via the pairing flow below. Store it (e.g. in your app settings) and reuse it on every subsequent connection — pairing again is only needed if the token is lost or revoked.

### Pairing (first connection only)
Dirigera requires a PKCE-style pairing flow where you must physically press the action button on the bottom of the hub while the code exchange is in progress:

```csharp
using Tomidix.NetStandard.Dirigera.Controller;

var controller = new DirigeraController(hostUrl); // e.g. "192.168.0.123"

string codeVerifier = DirigeraController.generateCodeVerifier();
string challenge = DirigeraController.calculateCodeChallenge(codeVerifier);

string dirigeraCode = (await controller.Authorize(challenge)).Code;

// Press the action button on the bottom of the Dirigera hub now,
// then exchange the code for an access token (may need a couple of retries
// while you reach for the button).
string accessToken = (await controller.Pair(dirigeraCode, codeVerifier, "MyApplicationName")).AccessToken;

// Persist accessToken and reuse it from now on, see below.
```

See [`TradfriTerminalUI/Program.cs`](../TradfriTerminalUI/Program.cs) for a complete, interactive reference implementation of this flow.

## 2. Example
Once you have an access token, reconnect with it directly:

```csharp
using Tomidix.NetStandard.Dirigera.Controller;
using Tomidix.NetStandard.Dirigera.Model.Devices;

var controller = new DirigeraController(hostUrl, accessToken);

var me = await controller.UserController.GetMe();

var devices = await controller.DeviceController.GetDevices();

foreach (var device in devices)
{
    switch (device)
    {
        case Light light:
            await controller.DeviceController.Toggle(light);
            await controller.DeviceController.SetLightLevel(light, 75);
            break;
        case Outlet outlet:
            await controller.DeviceController.Toggle(outlet);
            break;
        case MotionSensor motionSensor:
            await controller.DeviceController.SetMotionDetectedDelay(motionSensor, 60);
            break;
    }
}

// Subscribe to real-time updates (lights turned on/off elsewhere, sensors triggering, etc.)
controller.EventController.OnEventSent += (sender, e) =>
{
    Console.WriteLine($"Received event: {e.Event}");
};
await controller.EventController.Connect(CancellationToken.None);
```

## 3. Building from source
This project currently depends on an `ApiLibs` commit that has not been published to NuGet yet, so it is referenced as a git submodule rather than a plain package reference. Clone (or update) the repository with submodules initialized before building:

```bash
git submodule update --init --recursive
```

## 4. Acknowledgements
This is an implementation based on analysis [I](https://github.com/tomidix/) found [here](https://github.com/ggravlingen/pytradfri) by [ggravlingen](https://github.com/ggravlingen/) and [here](https://bitsex.net/software/2017/coap-endpoints-on-ikea-tradfri/) by [vidarlo](https://bitsex.net/).

The Dirigera implementation is based on [this Java implementation](https://github.com/dvdgeisler/DirigeraClient) and [this Node implementation](https://github.com/lpgera/dirigera).

## 5. Authors
- [mjwsteenbergen](https://github.com/mjwsteenbergen) - Initial work, device/event model, sensor & outlet support
- [tomidix](https://github.com/tomidix) - setup of build procedures, nuget package publish 

## 6. Changelog
You can check the changelog [here](CHANGELOG.md).