using System;
using Microsoft.FeatureManagement;

namespace XeDotNet.DemoConfigApp
{

    [FilterAlias("Claims")] // How we will refer to the filter in configuration
    public class ClaimsFeatureFilter : IFeatureFilter
    {
        // Used to access HttpContext
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClaimsFeatureFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            // Get the ClaimsFilterSettings from configuration
            var settings = context.Parameters.Get<ClaimsFilterSettings>();

            // Retrieve the current user (ClaimsPrincipal)
            var user = _httpContextAccessor.HttpContext.User;

            // Only enable the feature if the user has ALL the required claims
            var isEnabled = settings.RequiredClaims.Split(",")
                .All(claimType => user.HasClaim(claim => claim.Type == claimType));

            return Task.FromResult(isEnabled);
        }
    }

}

