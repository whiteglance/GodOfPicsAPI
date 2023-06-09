using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace GodOfPicsAPI.Models
{
    public class Urls
    {
        public string Regular { get; set; }
    }
    public class Photos
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("url")]
        public Urls Url { get; set; }

    }
}
