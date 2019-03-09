using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DangEasy.UptownFunc.Test.Unit.Models;
using Newtonsoft.Json;
using Xunit;

namespace DangEasy.UptownFunc.Test.Unit
{

    public class When_Getting_By_Sql : BaseTestFixture, IDisposable
    {
        public When_Getting_By_Sql() : base()
        {
        }

        [Fact]
        public void Get_By_Sql_Has_Result()
        {
            //var sql = new SqlQuery
            //{
            //    Sql = $"SELECT * FROM {typeof(Profile)} c WHERE c.firstName = 'Anthony'"
            //};

            //            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //            Assert.NotEmpty(response.Content);

            //var returnedModels = JsonConvert.DeserializeObject<List<Profile>>(response.Content);
            //Assert.Equal(_model.Id, returnedModels.First().Id);
            //Assert.Equal(_model.FirstName, returnedModels.First().FirstName);
        }


        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
