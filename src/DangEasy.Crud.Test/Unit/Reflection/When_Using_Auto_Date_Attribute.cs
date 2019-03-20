using System;
using DangEasy.Crud.Attributes;
using DangEasy.Crud.Reflection;
using DangEasy.Crud.Test.Unit.Models;
using Xunit;

namespace DangEasy.Crud.Test.Unit.Reflection
{
    public class When_Using_Auto_Date_Attribute
    {
        public When_Using_Auto_Date_Attribute()
        {
        }


        [Fact]
        public void CreatedProperty_Value_Is_Set()
        {
            var profile = new Profile();

            var datetime = new DateTime(2019, 1, 1);
            profile = ReflectionHelper.SetAutoDateProperties(profile, typeof(AutoCreatedDateAttribute), datetime);

            Assert.Equal(datetime, profile.Created);
        }


        [Fact]
        public void UpdatedProperty_Value_Is_Set()
        {
            var profile = new Profile();

            var datetime = new DateTime(2019, 1, 1);
            profile = ReflectionHelper.SetAutoDateProperties(profile, typeof(AutoUpdatedDateAttribute), datetime);

            Assert.Equal(datetime, profile.Updated);
        }
    }
}
