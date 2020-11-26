using System.Collections.Generic;

namespace WebApiGraphBigBrain.models
{
    public class User
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string givenName { get; set; }
        public string surname { get; set; }
        public string preferredName { get; set; }
        public string userPrincipalName { get; set; }
        public string mail { get; set; }
    }

    public class Users
    {
        public int totalResults { get; set; }
        public List<User> resources { get; set; }
    }
}
