using System;
using System.Linq;
using System.Reflection;
using DangEasy.Crud.Attributes;

namespace DangEasy.Crud.Reflection
{
    public static class PropertyInfoExtension
    {
        public static string GetJsonPropertyName(this PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof(UniqueKeyAttribute), true);

            if (attributes.Any())
            {
                if (attributes.First() is UniqueKeyAttribute jsonProperty)
                {
                    return jsonProperty.PropertyName;
                }
            }

            return null;
        }
    }
}
