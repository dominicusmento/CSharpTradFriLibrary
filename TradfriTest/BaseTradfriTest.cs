using System.Threading.Tasks;
using NUnit.Framework;
using Tomidix.NetStandard.Tradfri;
using Tomidix.NetStandard.Tradfri.Models;

namespace Tomidix.NetCore.TradfriTest
{
    public abstract class BaseTradfriTest
    {
        internal TradfriController tradfriController;

        [SetUp]
        public virtual void BaseSetup()
        {
            string applicationName = "UnitTestApp";
            tradfriController = new TradfriController("GatewayName", "IP");
            tradfriController.ConnectAppKey("PSK", applicationName);
        }

        // Real usage example
        public virtual async Task BaseSetupRecommendation()
        {
            // Unique name for the application which communicates with Tradfri gateway
            string applicationName = "UnitTestApp";
            TradfriController controller = new TradfriController("GatewayName", "IP");

            // This line should only be called once per applicationName
            // Gateway generates one appSecret key per applicationName
            TradfriAuth appSecret = await controller.GenerateAppSecret("PSK", applicationName);

            // You should now save programatically appSecret.PSK value and use it
            // when connection to your gateway every other time
            controller.ConnectAppKey(appSecret.PSK, applicationName);
        }
    }
}