using Microsoft.AspNet.Identity;
using ServerSideShareAccessPoint.Helper;
using ServerSideShareAccessPoint.Models;
using ServerSideShareAccessPoint.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServerSideShareAccessPoint.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthContext _ctx;
        private AuthRepository _authRepo;
        private AccountRepository _accountRepo;
        public AccountController()
        {
            _ctx = new AuthContext();
            _authRepo = new AuthRepository(_ctx);
            _accountRepo = new AccountRepository(_ctx);
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _authRepo.RegisterUser(model.UserName, model.Password, model.FullName, model.Address, model.Phone);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [Route("Details/{id}")]
        [Authorize]
        public async Task<HttpResponseMessage> Details(string id)
        {
            try
            {
                if (id == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                Account account = await _authRepo.FindUserById(id, User.Identity.Name);
                var response = Request.CreateResponse(HttpStatusCode.OK, account);
                response.Headers.ETag = new System.Net.Http.Headers.EntityTagHeaderValue("\"" + Convert.ToBase64String(account.AccountBalance.GuidVersion, Base64FormattingOptions.None) + "\"");
                return response;
            }
            catch (NullReferenceException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Account not found");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("Put/{id}")]
        [HttpPut]
        [Authorize]
        public async Task<HttpResponseMessage> Put(string id, AccountInfo accountInfo)
        {
            try
            {
                if (id == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                await _authRepo.PutUser(id, accountInfo, User.Identity.Name);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (NullReferenceException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Account not found");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("Put/{id}")]
        [HttpPut]
        [Authorize]
        public async Task<HttpResponseMessage> TransferMoney(string fromId, string toId, double amount)
        {
            try
            {
                if (fromId == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Please provide from account");
                }
                if (toId == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Please provide destination account");
                }

                var fromAccount = await _authRepo.FindUserById(fromId, User.Identity.Name);
                if (fromAccount != null)
                {
                    // perform concurrency conflict check
                    if (CheckIfWasModified(this.Request.Headers, fromAccount))
                    {
                        // From account are up to date.

                        var desAccount = await _authRepo.FindUserById(fromId);
                        if(desAccount != null)
                        {
                            await _accountRepo.TransferMoney(fromAccount.AccountBalance, desAccount.AccountBalance, amount);
                        }
                        else // Destination Account not found
                        {
                            return Request.CreateResponse(HttpStatusCode.NotModified, "Destination account not found!");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Header is not matched!");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Unable to save changes. Account was deleted by another user.");
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (HttpResponseException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, ex.Message);
            }
            catch (NullReferenceException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Account not found!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public bool CheckIfWasModified(HttpRequestHeaders requestHeaders, Account existingAccount)
        {
            if (requestHeaders.IfMatch != null)
            {
                // if the request contains an If-Match haeder with a non-empty ETag
                EntityTagHeaderValue firstHeaderVal = requestHeaders.IfMatch.FirstOrDefault();
                if ((firstHeaderVal != null) && (!string.IsNullOrEmpty(firstHeaderVal.Tag))
                    && (firstHeaderVal.Tag != "*")
                    )
                {
                    // compare the old and new ETag value (this can be done at DB-level using a WHERE clause)
                    string encodedNewTagValue = firstHeaderVal.Tag.Trim("\"".ToCharArray());
                    string encodeExistingTagValue = Convert.ToBase64String(existingAccount.AccountBalance.GuidVersion, Base64FormattingOptions.None);
                    if (!encodedNewTagValue.Equals(encodeExistingTagValue, StringComparison.Ordinal))
                    {
                        // concurrency conflict if the resource was modified
                        throw new HttpResponseException(HttpStatusCode.PreconditionFailed);
                    }
                }
                return true;
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _authRepo.Dispose();
                _accountRepo.Dispose();
            }

            base.Dispose(disposing);
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
    }
}
