using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using si2.dal.Context;
using si2.dal.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace si2.dal.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly Si2DbContext _db;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(Si2DbContext db, IServiceProvider serviceProvider)
        {
            _db = db;
            _serviceProvider = serviceProvider;
        }

        public IDataflowRepository Dataflows => _serviceProvider.GetService<IDataflowRepository>();

        public ISimplecohortRepository Simplecohorts => _serviceProvider.GetService<ISimplecohortRepository>();

        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await _db.SaveChangesAsync(ct);
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

    }
}
