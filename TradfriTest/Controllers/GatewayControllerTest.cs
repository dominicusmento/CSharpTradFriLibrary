using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tomidix.NetStandard.Tradfri.Controllers;
using Tomidix.NetStandard.Tradfri.Models;

namespace Tomidix.NetCore.TradfriTest.Controllers
{
    internal class GatewayControllerTest : BaseTradfriTest
    {
        private GatewayController controller;

        [SetUp]
        public void Setup()
        {
            BaseSetup();
            controller = tradfriController.GatewayController;
        }


        [Test]
        public async Task GetGatewayInfo()
        {
            GatewayInfo obj = await controller.GetGatewayInfo();
            Assert.NotNull(obj);
            Assert.Greater(obj.CurrentTimeUnix, 1546807755);
        }

        [Test]
        public async Task GetDevicesTest()
        {
            List<long> obj = await controller.GetDevices();
            Assert.NotNull(obj);
            Assert.Greater(obj.Count, 0);
        }

        [Test]
        public async Task GetDeviceObjectsTest()
        {
            List<TradfriDevice> obj = await controller.GetDeviceObjects();
            Assert.NotNull(obj);
            Assert.Greater(obj.Count, 0);
        }

        [Test]
        public async Task GetGroupsTest()
        {
            List<long> obj = await controller.GetGroups();
            Assert.NotNull(obj);
            Assert.Greater(obj.Count, 0);
        }

        [Test]
        public async Task GetGroupObjectsTest()
        {
            List<TradfriGroup> obj = await controller.GetGroupObjects();
            Assert.NotNull(obj);
            Assert.Greater(obj.Count, 0);
        }

        [Test]
        public async Task GetMoodsTest()
        {
            List<TradfriMood> obj = await controller.GetMoods();
            Assert.NotNull(obj);
            Assert.Greater(obj.Count, 0);
        }

        [Test]
        public async Task GetSmartTasksTest()
        {
            List<long> obj = await controller.GetSmartTasks();
            Assert.NotNull(obj);
            Assert.Greater(obj.Count, 0);

        }

        [Test]
        public async Task GetSmartTaskObjectsTest()
        {
            List<TradfriSmartTask> obj = await controller.GetSmartTaskObjects();
            Assert.NotNull(obj);
            Assert.Greater(obj.Count, 0);
        }
    }
}
