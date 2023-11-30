using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubtelWalletModel;

namespace HubtelWalletDatabase.DatabaseAccess
{
    public class WalletDatabaseMock
    {
        readonly static ConcurrentBag<Wallet> s_wallets = new ConcurrentBag<Wallet>();
        static int s_walletId = 0;

        internal async Task<int> GetWalletCount(string user)
        {
            return s_wallets.Where(x => x.Owner.Equals(user)).Count();
        }

        internal async Task<List<Wallet>> GetWallets(string user)
        {
            return s_wallets.Where(x => x.Owner == user).ToList();
        }

        internal async Task<Wallet?> GetWallet(string accountNumber)
        {
            return s_wallets.FirstOrDefault(x => x.AccountNumber == accountNumber);
        }

        internal async Task<Wallet?> GetWallet(int id)
        {
            return s_wallets.FirstOrDefault(x => x.Id == id);
        }

        internal async Task<Wallet> PostWallet(Wallet wallet)
        {
            wallet.Id = Interlocked.Increment(ref s_walletId);
            s_wallets.Add(wallet);
            return wallet;
        }

        internal async Task DeleteWallet(int id)
        {
            var wallet = s_wallets.FirstOrDefault(w => w.Id == id);
            if (wallet == null)
                throw new NullReferenceException();
            s_wallets.TryTake(out wallet);
        }
    }
}
