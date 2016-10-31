using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServerSideShareAccessPoint.Helper;
using ServerSideShareAccessPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ServerSideShareAccessPoint.Repositories
{
    public class AuthRepository : IDisposable, IAuthRepository
    {
        private AuthContext _ctx;
        private UserManager<Account> _userManager;

        public AuthRepository(AuthContext ctx)
        {
            _ctx = ctx;
            _userManager = new UserManager<Account>(new UserStore<Account>(_ctx));
        }

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<Account>(new UserStore<Account>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(string userName, string password, string fullname, string address, string phone)
        {
            Account account = new Account()
            {
                UserName = userName,
            };
            account.AccountInfo = new AccountInfo()
            {
                FullName = fullname,
                Phone = phone,
                Address = address
            };

            account.AccountBalance = new AccountBalance()
            {
                Balance = 0,
            };
            var result = await _userManager.CreateAsync(account, password);
            return result;
        }

        public async Task<AccountInfo> FindUser(string userName, string password)
        {
            try
            {
                Account user = await _userManager.FindAsync(userName, password);
                return user.AccountInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Account> FindUserById(string id, string currentUserName)
        {
            try
            {
                Account user = await _userManager.FindByIdAsync(id);
                if(user == null)
                    throw new NullReferenceException();
                if (user.UserName != currentUserName)
                    throw new Exception("Can't find another user information!");
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Account> FindUserById(string id)
        {
            try
            {
                Account user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    throw new NullReferenceException();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PutUser(string id, AccountInfo accountInfo, string currentUserName)
        {
            try
            {
                Account user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    throw new NullReferenceException();
                if (user.UserName != currentUserName)
                    throw new Exception("Can't change another user information!");
                user.AccountInfo.Address = accountInfo.Address;
                user.AccountInfo.Phone = accountInfo.Phone;
                user.AccountInfo.FullName = accountInfo.FullName;
                await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}