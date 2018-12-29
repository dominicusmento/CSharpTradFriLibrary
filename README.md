## C# Tradfri Library 
This is a .NET Standard (2.0) library to communicate with the [IKEA Trådfri](http://www.ikea.com/us/en/catalog/products/00337813/) (Tradfri) ZigBee-based Gateway. Using this library you can, by communicating with the gateway, control IKEA lights (including the RGB ones). 
![Build Status](https://mmustapic.visualstudio.com/_apis/public/build/definitions/596f4816-2b07-4869-bcd5-50b9df973ce6/1/badge) [![GitHub last commit](https://img.shields.io/github/last-commit/tomidix/CSharpTradFriLibrary.svg)]() [![NuGet downloads](https://img.shields.io/nuget/v/Tomidix.CSharpTradFriLibrary.svg)](https://www.nuget.org/packages/Tomidix.CSharpTradFriLibrary) [![NuGet downloads](https://img.shields.io/nuget/dt/Tomidix.CSharpTradFriLibrary.svg)](https://www.nuget.org/packages/Tomidix.CSharpTradFriLibrary) 

This library is still in development. Current version: 0.3.0.x (x-build number, references only minor changes and fixes)

Latest Gateway version tested and working - 1.3.14.

- Get information on the gateway
- Observe lights, groups and other resources (and in later versions get notified when they change)
- List all devices connected to gateway
- List all lights and get attributes of lights (name, state, color temp, dimmer level etc)
- Change attribute values of lights (currently only turn them on/off)
- Restart and reset gateway

## 1. Usage
Download the [nuget package](https://www.nuget.org/packages/Tomidix.CSharpTradFriLibrary). You will need the following values:
- **GatewayName** is your nickname to your gateway, currently this doesn't have an effect. It is here if you have access to multiple gateways so you can easily differentiate them.
- **GatewayIP** is the IP-address to your gateway.
- **GatewaySecret** is written on the back of your IKEA Tradfri Gateway.


## 2. Example
```csharp
    var controller = new TradfriController("GatewayName", "GatewayIP");
    controller.Connect("GatewaySecret");

    GatewayController gatewayController = controller.GatewayController;
    var devices = await gatewayController.GetDeviceObjects();

    DeviceController deviceController = controller.DeviceController;
    await deviceController.SetLight(devices[0], true);
    await deviceController.SetColor(devices[0], TradfriColors.SaturatedRed);

    //same works for `controller.GroupController`
```

## 3. Acknowledgements
This is an implementation based on analysis [I](https://github.com/tomidix/) found [here](https://github.com/ggravlingen/pytradfri) by [ggravlingen](https://github.com/ggravlingen/) and [here](https://bitsex.net/software/2017/coap-endpoints-on-ikea-tradfri/) by [vidarlo](https://bitsex.net/).
