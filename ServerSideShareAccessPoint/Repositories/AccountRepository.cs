using ServerSideShareAccessPoint.Helper;
using ServerSideShareAccessPoint.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ServerSideShareAccessPoint.Repositories
{
    public class AccountRepository : IDisposable, IAccountRepository
    {
        private AuthContext _ctx;

        public AccountRepository(AuthContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> TransferMoney(AccountBalance fromAccount, AccountBalance destinationAccount, double amount)
        {
            using (var dbcxtransaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    // Update data for From Account
                    fromAccount.Balance -= amount;
                    fromAccount.GuidVersion = Guid.NewGuid().ToByteArray();
                    _ctx.Entry(fromAccount).OriginalValues["RowVersion"] = fromAccount.RowVersion;
                    _ctx.Entry(fromAccount).State = EntityState.Modified;

                    // Update data for Destination Account
                    destinationAccount.Balance += amount;
                    destinationAccount.GuidVersion = Guid.NewGuid().ToByteArray();
                    _ctx.Entry(fromAccount).OriginalValues["RowVersion"] = fromAccount.RowVersion;
                    _ctx.Entry(fromAccount).State = EntityState.Modified;

                    await _ctx.SaveChangesAsync();
                    dbcxtransaction.Commit();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    dbcxtransaction.Rollback();
                    throw new DbUpdateConcurrencyException("Atlest 1 account had been changed by another user. Please try again!");
                }
            }
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}