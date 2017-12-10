using Tomidix.CSharpTradFriLibrary.Models;

namespace Tomidix.CSharpTradFriLibrary.Extensions
{
    public static class EnumExtension
    {
        public static string ValueAsString(this TradFriConstRoot enumerator)
        {
            return ((int)enumerator).ToString();
        }
        public static string ValueAsString(this TradFriConstAttr enumerator)
        {
            return ((int)enumerator).ToString();
        }
        public static string ValueAsString(this TradFriConstMireds enumerator)
        {
            return ((int)enumerator).ToString();
        }
    }
}
