using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradFriLibrary.Models;

namespace TradFriLibrary.Extensions
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
