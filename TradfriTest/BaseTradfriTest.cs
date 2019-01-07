using NUnit.Framework;
using System.Threading.Tasks;
using Tradfri;
using Tradfri.Controllers;

namespace TradfriTest
{
    public abstract class BaseTradfriTest
    {
        internal TradfriController tradfriController;

        [SetUp]
        public virtual void BaseSetup()
        {
            tradfriController = new TradfriController("NAME", "IP", "PSK");
        }
    }
}