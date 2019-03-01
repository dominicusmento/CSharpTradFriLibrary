using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;
using Tradfri;

namespace TradfriTest
{
    class ConnectTest
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
