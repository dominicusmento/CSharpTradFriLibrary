using NUnit.Framework;

namespace Tomidix.NetCore.TradfriTest
{
    internal class ConnectTest
    {
        [Test]
        public void SetColorTest()
        {
            //Fill in the following values
            string ip = null;
            string secret = null;

            if (ip == null || secret == null)
            {
                throw new InconclusiveException("You did not fill in anything");
            }
        }
    }
}
