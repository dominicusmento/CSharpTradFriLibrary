using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tradfri.Controllers;

namespace TradfriTest.Controllers
{
    class GatewayControllerTest : BaseTradfriTest
    {
        GatewayController controller;

        [SetUp]
        public void Setup()
        {
            BaseSetup();
            controller = tradfriController.GatewayController;
        }


        [Test]
        public async Task GetGatewayInfo()
        {
            var obj = await controller.GetGatewayInfo();
            Assert.NotNull(obj);
            Assert.Greater(obj.CurrentTimeUnix, 1546807755);
        }

        [Test]
        public async Task GetDevicesTest()
        {
            var obj = await controller.GetDevices();
            Assert.NotNull(obj);
            Assert.Greater(obj.Count, 0);
        }

        [Test]
        public async Task GetDeviceObjectsTest()
        {
            var obj = await controller.GetDeviceObjects();
            Assert.NotNull(obj);
            Assert.Greater(obj.Length, 0);
        }

        [Test]
        public async Task GetGroupsTest()
        {
            var obj = await controller.GetGroups();
            Assert.NotNull(obj);
            Assert.Greater(obj.Count, 0);
        }

        [Test]
        public async Task GetGroupObjectsTest()
        {
            var obj = await controller.GetGroupObjects();
            Assert.NotNull(obj);
            Assert.Greater(obj.Length, 0);
        }

        [Test]
        public async Task GetMoodsTest()
        {
            var obj = await controller.GetMoods();
            Assert.NotNull(obj);
            Assert.Greater(obj.Count, 0);
        }

        [Test]
        public async Task GetSmartTasksTest()
        {
            var obj = await controller.GetSmartTasks();
            Assert.NotNull(obj);
            Assert.Greater(obj.Count, 0);

        }

        [Test]
        public async Task GetSmartTaskObjectsTest()
        {
            var obj = await controller.GetSmartTaskObjects();
            Assert.NotNull(obj);
            Assert.Greater(obj.Length, 0);
        }
    }
}
