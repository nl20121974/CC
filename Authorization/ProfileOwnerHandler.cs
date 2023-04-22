using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CC.Authorization
{
    public class ProfileOwnerHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (context.User != null)
            {
                var pendingRequirements = context.PendingRequirements.ToList();

                foreach (var requirement in pendingRequirements)
                {
                    if (requirement is ProfileOwnerRequirement)
                    {
                        // get profile id from resource, passed in from blazor 
                        //  page component
                        var resource = context.Resource?.ToString();
                        var hasParsed = int.TryParse(resource, out int profileID);
                        if (hasParsed)
                        {
                            if (IsOwner(context.User, profileID))
                            {
                                context.Succeed(requirement);
                            }
                        }
                    }
                }

            }
            return Task.CompletedTask;
        }
        private bool IsOwner(ClaimsPrincipal user, int profileID)
        {
            // compare the requested memberId to the user's actual claim of 
            // memberId
            //  var isAuthorized = context.User.GetMemberIdClaim();
            // now we know if the user is authorized or not, and can act 
            // accordingly

            var _profileID = user.GetMemberIDClaim();


            return _profileID == profileID;
        }

    }
}
