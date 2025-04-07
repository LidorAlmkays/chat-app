using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gateway.Api.Identity
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    internal sealed class RequiresClaimAttribute : Attribute, IAuthorizationFilter
    {

        private readonly string _claimName;
        private readonly string _claimValue;

        public RequiresClaimAttribute(string claimName, string claimValue)
        {
            _claimName = claimName;
            _claimValue = claimValue;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            if (!context.HttpContext.User.HasClaim(_claimName, _claimValue))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}