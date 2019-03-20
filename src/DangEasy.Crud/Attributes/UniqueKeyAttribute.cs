using System;
namespace DangEasy.Crud.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueKeyAttribute : Attribute
    {
        public string PropertyName { get; }

        public UniqueKeyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
