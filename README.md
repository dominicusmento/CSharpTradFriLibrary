This is a .NET Framework (4.7) library to communicate with the [IKEA Trådfri](http://www.ikea.com/us/en/catalog/products/00337813/) (Tradfri) ZigBee-based Gateway. Using this library you can, by communicating with the gateway, control IKEA lights (including the RGB ones). 

This library is still in development. Current version: 0.1.0.

Latest Gateway version tested and working - 1.2.42.

- Get information on the gateway
- Observe lights, groups and other resources (and in later versions get notified when they change)
- List all devices connected to gateway
- List all lights and get attributes of lights (name, state, color temp, dimmer level etc)
- Change attribute values of lights (currently only turn them on/off)
- Restart and reset gateway

## 1. Usage
Clone the repository. Open the solution and then the app.config file under the TradFriGui project. 
Edit the values in the app.config file:
- **GatewayName** is your nickname to your gateway, currently this doesn't have an effect. It is here if you have access to multiple gateways so you can easily differentiate them.
- **GatewayIP** is the IP-address to your gateway.
- **GatewaySecret** is written on the back of your IKEA Tradfri Gateway.

After editing app.config you can hit F5 and run the project. You should have a grid displaying devices connected to your Gateway. Upon selecting a row(s) (row!! and not cell) you can turn On or Off your selected devices.
If everything works, you can proceed with investigating the code written in TradFriGui because that's the way how you will use the library.

## 2. TradFriGui
This project serves only as a demo project with an example on how to use the library, it is not intended to be a complete application but you can use it as a starting point for your main project.

## 3. TradFriLibrary
This project contains all the models and controllers needed for communication with the Gateway. You don't need to mess with this project unless you want to further investigate it or to participate in library development.
For now, you can build the .dll of the project in release version and add it as a reference to your project.

## 4. Acknowledgements
This is an implementation based on analysis [I](https://github.com/tomidix/) found [here](https://github.com/ggravlingen/pytradfri) by [ggravlingen](https://github.com/ggravlingen/) and [here](https://bitsex.net/software/2017/coap-endpoints-on-ikea-tradfri/) by [vidarlo](https://bitsex.net/).
