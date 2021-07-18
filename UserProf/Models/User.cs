using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserProf.Models
{
   public class User
   {
       [BsonId]
       [BsonRepresentation(BsonType.ObjectId)]
       public string Id {get;set;}
       [BsonElement("userName")]
       [BsonRequired]
       public string UserName{get;set;}
       [BsonElement("password")]
       [BsonRequired]
       public string Password{get;set;}
       [BsonElement("fullName")]
       [BsonRequired]
       public string FullName{get;set;}
       [BsonElement("email")]
       [BsonRequired]
       [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
       public string Email{get;set;}       
   }
}