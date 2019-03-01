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
    class GroupControllerTest : BaseTradfriTest
    {

        GroupController controller;
        List<TradfriGroup> groups;
        TradfriGroup group;
        TradfriMood mood;

        [OneTimeSetUp]
        public async Task Setup()
        {
            BaseSetup();
            controller = tradfriController.GroupController;
            groups = new List<TradfriGroup>(await tradfriController.GatewayController.GetGroupObjects());
            group = groups.FirstOrDefault();
            mood = (await tradfriController.GatewayController.GetMoods()).FirstOrDefault();
        }


        [Test]
        public async Task SetMoodTest()
        {
            if (group == null || mood == null)
            {
                throw new InconclusiveException("You have no groups");
            }
            await controller.SetMood(group, mood);
        }

        [Test]
        public async Task SetDimmerTest()
        {
            if (group == null)
            {
                throw new InconclusiveException("You have no groups");
            }
            await controller.SetDimmer(group, 125);
        }

        [Test]
        public async Task SetLightTest()
        {
            if (group == null)
            {
                throw new InconclusiveException("You have no groups");
            }
            await controller.SetLight(group, true);
        }
    }
}
