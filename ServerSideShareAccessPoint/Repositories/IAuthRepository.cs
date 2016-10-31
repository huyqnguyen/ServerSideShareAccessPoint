using Microsoft.AspNet.Identity;
using ServerSideShareAccessPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ServerSideShareAccessPoint.Repositories
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterUser(string userName, string password, string fullname, string address, string phone);
        Task<AccountInfo> FindUser(string userName, string password);
        Task<Account> FindUserById(string id, string currentUserName);
        Task<Account> FindUserById(string id);
        Task<bool> PutUser(string id, AccountInfo accountInfo, string currentUserName);
    }
}