using ForexDatabase.DAL;
using ForexDatabase.Models;
using Microsoft.AspNet.Identity;
using PostgreSQL.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ForexDatabase.Controllers
{
    public class AuthenticationController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/register")]
        [HttpPost]
        public IdentityResult Register(ForexFunUserModel user)
        {
            var userStore = new UserStore<ForexFunUser>(new ForexFunUserContext());
            var manager = new UserManager<ForexFunUser>(userStore);
            var forexFunUser = new ForexFunUser { UserName = user.Username };
            IdentityResult result = manager.Create(forexFunUser, user.Password);
            return result;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        [Authorize]
        [Route("api/userid")]
        public string GetUserId()
        {
            var identityClaims = (ClaimsIdentity) User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            return identityClaims.FindFirst("Id").Value;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        [Authorize]
        [Route("api/username")]
        public string GetUserName()
        {
            var identityClaims = (ClaimsIdentity) User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            return identityClaims.FindFirst("Username").Value;
        }
    }
}
