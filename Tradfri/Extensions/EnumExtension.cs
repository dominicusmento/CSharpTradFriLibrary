using Tomidix.NetStandard.Tradfri.Models;

namespace Tomidix.NetStandard.Tradfri.Extensions
{
    public static class EnumExtension
    {
        public static string ValueAsString(this TradfriConstRoot enumerator)
        {
            return ((int)enumerator).ToString();
        }

        public static string ValueAsString(this TradfriConstAttr enumerator)
        {
            return ((int)enumerator).ToString();
        }

        public static string ValueAsString(this TradfriConstMireds enumerator)
        {
            return ((int)enumerator).ToString();
        }
    }
}
