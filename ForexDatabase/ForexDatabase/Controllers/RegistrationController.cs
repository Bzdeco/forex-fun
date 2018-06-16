using ForexDatabase.DAL;
using ForexDatabase.Models;
using Microsoft.AspNet.Identity;
using PostgreSQL.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ForexDatabase.Controllers
{
    public class RegistrationController : ApiController
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
    }
}
