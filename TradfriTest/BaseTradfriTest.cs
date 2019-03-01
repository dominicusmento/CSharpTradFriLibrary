using NUnit.Framework;
using Tradfri;
using Tradfri.Models;

namespace TradfriTest
{
    public abstract class BaseTradfriTest
    {
        internal TradfriController tradfriController;

        [SetUp]
        public virtual void BaseSetup()
        {
            tradfriController = new TradfriController("GatewayName", "IP", "PSK");
        }

        // Real usage example
        public virtual void BaseSetupRecommendation()
        {
            // Unique name for the application which communicates with Tradfri gateway
            string applicationName = "UnitTestApp";
            TradfriController controller = new TradfriController("GatewayName", "IP");

            // This line should only be called once per applicationName
            // Gateway generates one appSecret key per applicationName
            TradfriAuth appSecret = controller.GenerateAppSecret("PSK", applicationName);

            // You should now save programatically appSecret.PSK value and use it
            // when connection to your gateway every other time
            controller.ConnectAppKey(appSecret.PSK, applicationName);
        }
    }
}