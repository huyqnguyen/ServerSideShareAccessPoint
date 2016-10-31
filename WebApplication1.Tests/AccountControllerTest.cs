using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServerSideShareAccessPoint.Controllers;
using ServerSideShareAccessPoint.Models;
using ServerSideShareAccessPoint.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Tests
{
    [TestClass]
    public class AccountControllerTest
    {
        private HttpClient _client;
        private string _productUri = "http://ipv4.fiddler/TestETag/api/Details/11";
        private readonly string jsonMediaType = "application/json";

        private AccountController controller;

        [TestInitialize]
        public void Init()
        {
            controller = new AccountController();
        }

        [TestMethod]
        public async Task Get_ReturnsTheETagInTheHeaders()
        {
            Moq.Mock<IAuthRepository> authRepository = new Moq.Mock<IAuthRepository>();
            authRepository.Setup(e => e.FindUserById("123456789", It.IsAny <string>())).Returns(Task.FromResult(new Account()));


            HttpResponseMessage httpResp = await controller.Details("123456789");
            
            Assert.IsNotNull(httpResp.Headers.ETag, "No ETag was included in the response headers!");
            Assert.IsTrue(!string.IsNullOrEmpty(httpResp.Headers.ETag.Tag), "An empty ETag was received!");

        }

        //[TestMethod]
        //public async Task Put_IfTheProductWasModified_ReturnsPreconditionFailed()
        //{
        //    // first user reads the product 11
        //    HttpResponseMessage getProdUser1 = await _client.GetAsync(_productUri);
        //    getProdUser1.EnsureSuccessStatusCode();
        //    Account prodUser1 = await getProdUser1.Content.ReadAsAsync();

        //    ///////////////
        //    // second user reads the product 11
        //    HttpResponseMessage getProdUser2 = await _client.GetAsync(_productUri);
        //    getProdUser2.EnsureSuccessStatusCode();
        //    Product prodUser2 = await getProdUser2.Content.ReadAsAsync();

        //    //second users modifies (PUT) and persist the product
        //    prodUser2.Description = prodUser2.Description + " modif by user 2";

        //    var putRequest2 = GetPutRequestMessage(prodUser2);
        //    // we have to use SendAsync in order to be able to set any header, including If-Match
        //    HttpResponseMessage putUser2Response = await _client.SendAsync(putRequest2);
        //    putUser2Response.EnsureSuccessStatusCode();

        //    ///////////////
        //    // first user modifies (PUT) and tries to persist the same product
        //    prodUser1.Description = prodUser2.Description + " modif by user 1";

        //    var putRequest1 = GetPutRequestMessage(prodUser1);
        //    HttpResponseMessage putUser1Response = await _client.SendAsync(putRequest1);

        //    /////////////////////
        //    Assert.AreEqual(HttpStatusCode.PreconditionFailed, putUser1Response.StatusCode,
        //                    "The response was not precondition failed!");

        //}

        //private HttpRequestMessage GetPutRequestMessage(Product product)
        //{
        //    var requestMessage = new HttpRequestMessage(HttpMethod.Put, _productUri);
        //    // add an 'If-Match' header
        //    requestMessage.Headers.IfMatch.ParseAdd("\"" + Convert.ToBase64String(product.Version) + "\"");
        //    // add the modified object serialized as JSON
        //    ObjectContent prodObjContent =
        //        requestMessage.CreateContent(
        //        product,
        //        MediaTypeHeaderValue.Parse(jsonMediaType),
        //        new MediaTypeFormatter[] { new JsonMediaTypeFormatter() },
        //        new FormatterSelector());

        //    return requestMessage;
        //}

        [TestCleanup]
        public void Cleanup()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }

    }
}
