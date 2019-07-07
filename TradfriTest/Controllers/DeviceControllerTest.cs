using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tomidix.NetStandard.Tradfri.Controllers;
using Tomidix.NetStandard.Tradfri.Models;

namespace Tomidix.NetCore.TradfriTest.Controllers
{
    internal class DeviceControllerTest : BaseTradfriTest
    {
        private DeviceController controller;
        private List<TradfriDevice> devices;
        private List<TradfriDevice> lights;
        private TradfriDevice colorLight;
        private TradfriDevice light;
        private TradfriDevice controlOutlet;

        [OneTimeSetUp]
        public async Task Setup()
        {
            BaseSetup();
            controller = tradfriController.DeviceController;
            devices = new List<TradfriDevice>(await tradfriController.GatewayController.GetDeviceObjects());
            lights = devices.Where(i => i.DeviceType.Equals(DeviceType.Light)).ToList();
            light = lights.FirstOrDefault();
            colorLight = lights.FirstOrDefault(i => i.LightControl != null && i.LightControl[0]?.ColorHex != null);
            controlOutlet = devices.FirstOrDefault(i => i.DeviceType == DeviceType.ControlOutlet);
        }


        [Test]
        public async Task SetColorTest()
        {
            if (colorLight == null)
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

        [Test]
        public async Task SetOutletTest()
        {
            if (controlOutlet == null)
            {
                throw new InconclusiveException("You have no outlets");
            }
            await controller.SetOutlet(controlOutlet, false);
        }
    }
}
