using System.ComponentModel;
using System.Reflection;

namespace Contastic.Models
{
    public class Argument
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public int Index { get; set; }

        public bool Required { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public string Description
        {
            get
            {
                var description = string.Empty;

                var attribute = PropertyInfo.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    description = attribute.Description;
                }

                if (string.IsNullOrEmpty(description))
                {
                    description = PropertyInfo.Name;
                }

                return description;
            }
        }
    }
}
