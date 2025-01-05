using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.API.Contract.V1.Responses
{
    public class TagsResult
    {

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }

    public class Result
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

}
