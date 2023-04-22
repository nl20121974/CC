using Microsoft.AspNetCore.Authorization;

namespace CC.Authorization
{
    public class ProfileOwnerRequirement : IAuthorizationRequirement
    {
        public ProfileOwnerRequirement() { }

    }
}
