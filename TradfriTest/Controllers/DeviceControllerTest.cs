using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tradfri.Controllers;
using Tradfri.Models;

namespace TradfriTest.Controllers
{
    class DeviceControllerTest : BaseTradfriTest
    {
        DeviceController controller;
        List<TradfriDevice> devices;
        TradfriDevice colorLight;
        TradfriDevice light;

        [OneTimeSetUp]
        public async Task Setup()
        {
            BaseSetup();
            controller = tradfriController.DeviceController;
            devices = new List<TradfriDevice>(await tradfriController.GatewayController.GetDeviceObjects());
            light = devices.FirstOrDefault(i => i.LightControl != null);
            colorLight = devices.FirstOrDefault(i => i.LightControl != null && i.LightControl[0]?.ColorHex != null);
        }


        [Test]
        public async Task SetColorTest()
        {
            if(colorLight == null)
            {
                throw new InconclusiveException("There is no light with colors");
            }
            await controller.SetColor(colorLight, TradfriColors.CoolWhite);
        }

        [Test]
        public async Task SetDimmerTest()
        {
            if (light == null)
            {
                throw new InconclusiveException("You have no lights");
            }
            await controller.SetDimmer(light, 125);
        }

        [Test]
        public async Task SetLightTest()
        {
            if (light == null)
            {
                throw new InconclusiveException("You have no lights");
            }
            await controller.SetLight(light, false);
        }


    }
}
