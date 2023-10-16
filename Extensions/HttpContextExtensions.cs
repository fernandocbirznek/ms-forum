using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ms_forum.Extensions
{
    public static class HttpContextExtensions
    {
        internal static string GetToken(this HttpContext httpContext)
        {
            if (httpContext == null)
                return string.Empty;
            return httpContext?
                .Request?
                .Headers["Authorization"];
        }

        internal static Guid GetUsuarioId
        (
            this HttpContext httpContext,
            IConfiguration configuration,
            Guid? usuarioId
        )
        {
            if (usuarioId.HasValue && !usuarioId.Equals(Guid.Empty))
                return usuarioId.Value;

            if (httpContext == null)
                return Guid.Empty;

            return Guid.Parse
            (
                GetValue
                (
                    httpContext,
                    configuration.GetValue<string>("authentication:oidc:standard-claims:sub")
                )
            );
        }

        internal static Guid GetUsuarioId
        (
            this HttpContext httpContext,
            IConfiguration configuration
        )
        {
            if (httpContext == null)
                return Guid.Empty;
            return Guid.Parse(GetValue(httpContext, configuration.GetValue<string>("authentication:oidc:standard-claims:sub")));
        }

        internal static string GetValue
        (
            this HttpContext httpContext,
            string claimType
        )
        {
            if (httpContext == null)
                return string.Empty;

            var claim = GetClaims(httpContext)
                .FirstOrDefault(claim => claim.Type == claimType);

            if (claim is null)
                return string.Empty;

            return claim.Value;
        }

        private static IEnumerable<Claim> GetClaims
        (
            HttpContext httpContext
        )
        {
            string jwtToken = GetToken(httpContext);
            if (string.IsNullOrEmpty(jwtToken))
                return new List<Claim>();

            SecurityToken securityToken = ReadToken(jwtToken);

            return ((JwtSecurityToken)securityToken).Claims;
        }

        public static bool JwtTokenIsInvalid
        (
            this string jwtToken
        )
        {
            if (string.IsNullOrEmpty(jwtToken))
                return false;

            SecurityToken securityToken = ReadToken(jwtToken);

            return (securityToken is null)
                || (securityToken.ValidFrom > DateTime.UtcNow)
                || (securityToken.ValidTo.AddHours(-12) < DateTime.UtcNow);
        }

        private static SecurityToken ReadToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ReadToken(jwtToken.Replace("Bearer", string.Empty).Trim());
        }
    }
}
