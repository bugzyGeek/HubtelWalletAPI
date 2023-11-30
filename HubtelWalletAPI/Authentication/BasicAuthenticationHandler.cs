using System.Security.Claims;
using System.Text;
using Azure.Core;

namespace HubtelWalletAPI.Authentication
{
    public class BasicAuthenticationHandler
    {
        const string s_unauthorized = "Unauthorized";
        const string s_authorization = "Authorization";
        readonly RequestDelegate _next;
        readonly string _relm;
        Dictionary<string, string> users = new()
        {
            {"0246884848","0246884848"},
            {"0244565768","0244565768" },
            {"0256556676","0256556676" },
        };

        public BasicAuthenticationHandler(RequestDelegate next, string relm)
        {
            this._next = next;
            this._relm = relm;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(!context.Request.Headers.ContainsKey(s_authorization))
            {
                await Unauthorized(context);
                return;
            }

            var header = context.Request.Headers["Authorization"].ToString();
            var encodedCred = header.Substring(6);
            var decodedCred = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCred));
            var cred = decodedCred.Split(':');

            if (!users.ContainsKey(cred[0]) && users[cred[0]] != cred[1])
            {
                await Unauthorized(context);
                return;
            }

            var claimIdentity = (ClaimsIdentity)context.User.Identity;
            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, cred[0]));

            await _next(context);
        }

        async Task Unauthorized(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(s_unauthorized);
        }
    }
}
