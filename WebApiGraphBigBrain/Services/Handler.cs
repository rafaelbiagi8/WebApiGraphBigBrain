using WebApiGraphBigBrain.models;

namespace WebApiGraphBigBrain.Services
{
    public class Handler
    {
        public static User UserProperty(Microsoft.Graph.User graphUser)
        {
            User user = new User();
            user.id = graphUser.Id;
            user.displayName = graphUser.DisplayName;
            user.givenName = graphUser.GivenName;
            user.surname = graphUser.Surname;
            user.preferredName = graphUser.PreferredName;
            user.userPrincipalName = graphUser.UserPrincipalName;
            user.mail = graphUser.Mail;
            

            return user;
        }
    }
}
