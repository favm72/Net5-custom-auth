using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace sampleauth
{
	interface IMyAuth
	{
		string Authenticate(Account account);
	}

	public class MyAuth : IMyAuth
	{
		public string token;

		public string Authenticate(Account account)
		{
			if (account.UserName != "fabio" && account.Password != "123")
			{
				return null;
			}
			token = Guid.NewGuid().ToString();
			return token;
		}
	}

	public class MyOptions : AuthenticationSchemeOptions
	{
		
	}

	public class MyAuthHandler : AuthenticationHandler<MyOptions>
	{
		MyAuth MyAuth;
		public MyAuthHandler(MyAuth myauth, IOptionsMonitor<MyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
		{
			this.MyAuth = myauth;
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			try
			{
				if (!Request.Headers.ContainsKey("auth"))
					throw new Exception("No auth Header");
				
				string token = Request.Headers["auth"];
				
				if (string.IsNullOrWhiteSpace(token))
					throw new Exception("Empty Token");

				if (MyAuth.token != token)
					throw new Exception("Invalid Token");

				var claims = new List<Claim>();
				claims.Add(new Claim(ClaimTypes.Name, "Fabio"));
				claims.Add(new Claim(ClaimTypes.Role, "admin"));
				var identity = new ClaimsIdentity(claims, Scheme.Name);
				var principal = new ClaimsPrincipal(identity);

				var ticket = new AuthenticationTicket(principal, "myschema");
				return AuthenticateResult.Success(ticket);
			}
			catch (Exception)
			{
				return AuthenticateResult.Fail("asda");
			}
			
		}
	}
}
