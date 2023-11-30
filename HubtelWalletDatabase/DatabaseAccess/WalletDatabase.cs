    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HubtelWalletDatabase.DatabaseAccess
{
    internal class WalletDatabase
    {
        private readonly WalletContext _walletDB;

        protected WalletDatabase(WalletContext pOSDBContext)
        {
            _walletDB = pOSDBContext;
        }
        protected async ValueTask<M> InsertAsync<M>(M client, DbSet<M> dbSet) where M : class
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            _ = await dbSet.AddAsync(client);
            await _walletDB.SaveChangesAsync();
            return client;
        }

        protected async ValueTask<List<M>> InsertAsync<M>(List<M> clients, DbSet<M> dbSet) where M : class
        {
            if (clients is null) throw new ArgumentNullException(nameof(clients));
            await dbSet.AddRangeAsync(clients);
            await _walletDB.SaveChangesAsync();
            return clients;
        }

        protected async ValueTask<int> UpdateAsync<M>(long id, M client, DbSet<M> dbSet) where M : class
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (await dbSet.FindAsync(id) == null)
                throw new NullReferenceException(nameof(client));

            _walletDB.Entry(client).State = EntityState.Modified;
            return await _walletDB.SaveChangesAsync();
        }

        protected async ValueTask<int> UpdateAsync<M>(string id, M client, DbSet<M> dbSet) where M : class
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (await dbSet.FindAsync(id) == null)
                throw new NullReferenceException(nameof(client));

            _walletDB.Entry(client).State = EntityState.Modified;
            return await _walletDB.SaveChangesAsync();
        }

        protected async ValueTask<int> UpdateAsync<M>(long id, M client, List<string> properties, DbSet<M> dbSet) where M : class
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (await dbSet.FindAsync(id) == null)
                throw new NullReferenceException(nameof(client));

            dbSet.Attach(client);
            foreach (var property in properties)
                dbSet.Entry(client).Property(property).IsModified = true;

            return await _walletDB.SaveChangesAsync();
        }

        protected async ValueTask<int> UpdateAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters)
        {
            return await _walletDB.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        protected async Task<(M client, int i)> DeleteAsync<M>(object id, DbSet<M> dbSet) where M : class
        {
            var client = await dbSet.FindAsync(id);
            if (client == null)
                throw new NullReferenceException(nameof(client));

            dbSet.Remove(client);
            int i = await _walletDB.SaveChangesAsync();
            return (client, i);
        }


        protected async IAsyncEnumerable<M> GetAsync<M>(DbSet<M> dbSet, Func<DbSet<M>, Task<List<M>>> select) where M : class
        {
            foreach (var l in await select(dbSet))
            {
                yield return l;
            }
        }

        protected Task<IQueryable<M>> GetAsync<M>(DbSet<M> dbSet, Func<DbSet<M>, IQueryable<M>> select) where M : class => Task.FromResult(select(dbSet));

        protected async Task<M> GetAsync<M>(DbSet<M> dbSet, Func<DbSet<M>, Task<M>> select) where M : class => await select(dbSet);

        protected async ValueTask<M> GetAsync<M>(DbSet<M> dbSet, Func<DbSet<M>, ValueTask<M>> select) where M : class => await select(dbSet);
    }
}
