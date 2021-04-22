using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sampleauth.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		MyAuth myAuth;
		public AccountController(MyAuth myAuth)
		{
			this.myAuth = myAuth;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<Resp> Login(Account account)
		{
			Resp resp = new Resp();
			myAuth.Authenticate(account);
			resp.Status = true;
			resp.Message = "Sucess Login";
			resp.Data = myAuth.token;
			return resp;
		}
	}
}
