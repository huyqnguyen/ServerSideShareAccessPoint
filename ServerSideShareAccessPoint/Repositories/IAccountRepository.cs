using ServerSideShareAccessPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ServerSideShareAccessPoint.Repositories
{
    public interface IAccountRepository
    {
        Task<bool> TransferMoney(AccountBalance fromAccount, AccountBalance destinationAccount, double amount);
    }
}