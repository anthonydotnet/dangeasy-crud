using DangEasy.Crud.Attributes;
using DangEasy.Crud.Reflection;
using DangEasy.Crud.Test.Unit.Models;
using Xunit;

namespace DangEasy.Crud.Test.Unit.Reflection
{
    public class When_Using_UniqueKeyAttribute
    {
        [Fact]
        public void CreatedProperty_Value_Is_Set()
        {
            var profile = new Profile() { Email = "test@test.com" };

            var result = ReflectionHelper.GetFirstPropertyWith<Profile>(typeof(UniqueKeyAttribute));

            Assert.Equal("Email", result.Name);
            Assert.Equal(profile.Email, result.GetValue(profile));
        }
    }
}
