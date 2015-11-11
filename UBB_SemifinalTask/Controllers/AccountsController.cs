using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using UBB_SemifinalTask.Data;
using UBB_SemifinalTask.Models;

namespace UBB_SemifinalTask.Controllers
{
    [Authorize]
    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
    {
        private readonly AuthRepository _repo;
        public AccountsController()
        {
            _repo = new AuthRepository();
        }
        [HttpGet]
        [Route("auth")]
        public IHttpActionResult AuthConfirm()
        {
            return Ok();
        }
        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(UserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await _repo.RegisterUser(createUserModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }
    }

}
