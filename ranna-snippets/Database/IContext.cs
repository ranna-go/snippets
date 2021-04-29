using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ranna_snippets.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace ranna_snippets.Database
{
    public interface IContext : IDisposable
    {
        DbSet<Snippet> Snippets { get; set; }

        DatabaseFacade Database { get; }

        EntityEntry Add([NotNull] object entity);
        EntityEntry Remove([NotNull] object entity);
        void RemoveRange([NotNullAttribute] IEnumerable<object> entities);
        DbSet<TEntity> Set<TEntity>([NotNull] string name) where TEntity : class;
        EntityEntry<TEntity> Update<TEntity>([NotNull] TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
