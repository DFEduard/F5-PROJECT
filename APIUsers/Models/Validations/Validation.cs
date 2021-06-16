using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace APIUsers.Models.Validations
{
    public static class Validation
    {
        public const string emptyField = "This field is required.";

        public static Dictionary<string, object> Fields(object model)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (var prop in model.GetType().GetProperties())
            {
                string propName = prop.Name;
                var value = model.GetType().GetProperty(prop.Name).GetValue(model, null);
                if (value == null)
                {
                    result.Add(propName, emptyField);
                }
                else
                {
                    if (value.GetType() == typeof(string))
                    {
                        if (string.IsNullOrEmpty(value.ToString()))
                        {
                            result.Add(propName, emptyField);
                        }
                    }
                }
            }

            if(result.Count() != 0)
            {
                return result;
            }

            return null;
        }
    }
}
