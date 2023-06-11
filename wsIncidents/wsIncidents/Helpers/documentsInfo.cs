using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace wsFirmaDte.Helpers {


    [AttributeUsage(AttributeTargets.All)]
    public class enumInfo : DescriptionAttribute {

        public enumInfo(string _description,string _version) {
            description = _description;
            version = _version;
        }

        public string description { get; set; }
        public string version { get; set; }
    }


    public static class Extensions {

        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute {
            return value.GetType().GetMember(value.ToString()).First().GetCustomAttribute<TAttribute>();
        }

    }
}
