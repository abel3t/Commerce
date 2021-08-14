using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Commerce.Core.Models
{
    public class User
    {
        public User(string name, string email, string imageUrl)
        {
            Name = name;
            Email = email;
            ImageUrl = imageUrl;
        }
    
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        
        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }
    }
}