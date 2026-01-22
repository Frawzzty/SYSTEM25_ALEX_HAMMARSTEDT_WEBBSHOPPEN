using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    [BsonIgnoreExtraElements]
    internal class UserAction
    {
        //Mongo DB OBJ
        public string Id { get;} = Guid.NewGuid().ToString();
        public int CustomerId { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now; //IN utc? 1h behind


        public UserAction()
        {

        }

        public UserAction(int customerId, Enums.UserActions action)
        {
            CustomerId = customerId;
            Action = action.ToString().Replace('_', ' ');
        }

        public UserAction(int customerId, Enums.UserActions action, string details)
        {
            CustomerId = customerId;
            Action = action.ToString().Replace('_', ' ');
            Details = details;
        }


    }
}
