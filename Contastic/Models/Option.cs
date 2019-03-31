using System;
using System.ComponentModel;
using System.Reflection;

namespace Contastic.Models
{
    public class Option : IOption
    {
        public PropertyInfo PropertyInfo { get; set; }

        public string LongName { get; set; }

        public char ShortName { get; set; }

        public bool Required { get; set; }

        public string Value { get; set; }

        public int Index { get; set; }

        public int Order { get; set; }

        public bool Unnamed { get; set; }

        public string Description
        {
            get
            {
                //if (Unnamed)
                {
                    var attribute = PropertyInfo.GetCustomAttribute<DescriptionAttribute>();
                    if (attribute != null)
                    {
                        return attribute.Description;
                    }

                    return PropertyInfo.PropertyType.Name;
                }
                //else
                {
                    
                }
            }
        }

        public string Name => PropertyInfo.Name;

        public string SwitchString
        {
            get
            {
                var result = string.Empty;

                if (ShortName != '\0')
                {
                    result = $"-{ShortName}";
                }

                if (!string.IsNullOrEmpty(LongName))
                {
                    if (!string.IsNullOrEmpty(result)) result += ", ";

                    result += $"--{LongName}";
                }

                return result;
            }
        }
    }
}
