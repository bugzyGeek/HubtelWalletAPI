using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubtelWalletDatabase.DatabaseAccess;
using HubtelWalletModel;

namespace HubtelWalletDatabase.DataAccess
{
    public class WalletDataAccess
    {
        private readonly WalletDatabaseMock _mock;

        public WalletDataAccess(WalletDatabaseMock mock)
        {
            _mock = mock;
        }
        public async Task<int> GetWalletCount(string user)
        {
            return await _mock.GetWalletCount(user);
        }

        public async Task<Wallet> GetWallet(int id)
        {
            return await _mock.GetWallet(id);
        }

        public async Task<List<Wallet>> GetWallets(string user)
        {
            return await _mock.GetWallets(user);
        }
        public async Task<Wallet> GetWallet(string accountNumber)
        {
            return await _mock.GetWallet(accountNumber);
        }

        public async Task<Wallet> PostWallet(Wallet wallet)
        {
                return await _mock.PostWallet(wallet);
        }

        public async Task Delete(int id)
        {
            try
            {
                await _mock.DeleteWallet(id);
                return;
            }
            catch
            {
                throw;
            }
        }
    }
}
