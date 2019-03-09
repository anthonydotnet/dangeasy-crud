using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DangEasy.UptownFunc.Test.Unit.Models;
using Newtonsoft.Json;
using Xunit;

namespace DangEasy.UptownFunc.Test.Unit
{
    public class When_Getting_By_Filter : BaseTestFixture, IDisposable
    {
        public When_Getting_By_Filter() : base()
        {

        }


        [Fact]
        public void Documents_Are_Returned()
        {

            //            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //            Assert.NotEmpty(response.Content);
            //            Assert.Equal(_model.Id, returnedModels.First().Id);
            //          Assert.Equal(_model.FirstName, returnedModels.First().FirstName);
        }




        [Fact]
        public void Count_Has_Result()
        {
            //            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //            Assert.NotEmpty(response.Content);
            //          Assert.Equal("1", response.Content);
        }



        public override void Dispose()
        {
            base.Dispose();
        }

    }

}

