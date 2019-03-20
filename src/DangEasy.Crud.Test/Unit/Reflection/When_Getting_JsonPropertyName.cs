using DangEasy.Crud.Attributes;
using DangEasy.Crud.Reflection;
using DangEasy.Crud.Test.Unit.Models;
using Newtonsoft.Json;
using Xunit;

namespace DangEasy.Crud.Test.Unit.Reflection
{
    public class When_Getting_JsonPropertyName
    {
        [Fact]
        public void PropertyName_Is_Returned()
        {
            var property = ReflectionHelper.GetFirstPropertyWith<TestClass>(typeof(UniqueKeyAttribute));

            var result = property.GetJsonPropertyName();

            Assert.Equal("database_property_name", result);
        }
    }


    public class TestClass
    {
        [UniqueKey("database_property_name")]
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
