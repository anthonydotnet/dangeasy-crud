using DangEasy.Crud.Attributes;
using Newtonsoft.Json;
using System;

namespace DangEasy.Crud.Test.Unit.Models
{
    public class Profile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [UniqueKey("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [AutoCreatedDate]
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [AutoUpdatedDate]
        [JsonProperty("updated")]
        public DateTime Updated { get; set; }
    }
}
