using Events.Api.Models.General;
using Events.Core.Models;
using Events.Core.Models.Services;
using Events.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Events.Core.Models.General;
using Microsoft.EntityFrameworkCore;

namespace Events.Service.Service.DataServices
{
    public class DbServiceImpl<T, V> : DbService<T, V> where T : Model
    {
        
        protected readonly AppDbContext context;
        protected readonly IOrderedQueryable<T> Query;
        protected readonly IOrderedQueryable<V> ViewQuery;

        public DbServiceImpl(IQuery<T, V> query)
        {
            Query = query.GetQuery();
            ViewQuery = query.GetViewQuery();
            context = query.GetContext();
        }


        public async Task<DbResponse<T>> AddItem(T entity)
        {
            context.Add(entity);
            try
            {
                var saveChangesAsync = await context.SaveChangesAsync();
                return saveChangesAsync > 0
                    ? new DbResponse<T>()
                    {
                        Entity = await GetLastAdded(),
                        IsSuccess = true
                    }
                    : new DbResponse<T>()
                    {
                        Message = "Error while inserting",
                        IsSuccess = false
                    };

            }
            catch (Exception e)
            {
                return new DbResponse<T>()
                {
                    Message = e.Message,
                    IsSuccess = false
                };
            }
            
        }
        
        public async Task<bool> AddConstant<R>(R entity) where R : IConstant
        {
            context.Add(entity);
            var saveChangesAsync = await context.SaveChangesAsync();
            return saveChangesAsync > 0;
        }

        public async Task<T> UpdateEntity(T entity)
        {
            context.Entry(entity).CurrentValues.SetValues(entity);
            var resp = await context.SaveChangesAsync();
            return await Find(entity.Id);
        }

        public async void UpdateEntity(Expression<Func<T, bool>> predicate, T entity)
        {
            T t = await Find(predicate);
            context.Entry(t).CurrentValues.SetValues(entity);
            await context.SaveChangesAsync();
        }

        public async Task<T> GetLastAdded()
        {
            var lastId = await GetLastId();
            return await Find(x => x.Id == lastId);
        }

        public async Task<T> Find(long id)
        =>await Query.SingleOrDefaultAsync(x => x.Id == id);

        public async Task Delete(T entity)
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        => await Query.FirstOrDefaultAsync(predicate);
        public async Task<T> GetFirst(Expression<Func<T, bool>> predicate)
        => await Query.FirstOrDefaultAsync(predicate);
        public async Task<List<V>> GetItems(Expression<Func<V, bool>> predicate,PaggingModel model = null,Func<V,bool> func = null)
        => model?.PageSize > 0 ?
            ViewQuery.Where(predicate).Skip(model.SkippedPages).Take(model.PageSize).ToList() :
            ViewQuery.Where(predicate).AsEnumerable().Where(func??((v)=> true)).ToList();
        public async Task<long> GetLastId()
        => await Query.MaxAsync(x => x.Id);

        public async Task<long> GetCount(Expression<Func<V, bool>> predicate)
        => await ViewQuery.LongCountAsync(predicate);

        public async Task<bool> addRange(List<T> items)
        {
            context.AddRange(items);
            var saveChangesAsync = await context.SaveChangesAsync();
            return saveChangesAsync == items.Count;
        }

        public async Task<bool>UpdateRange(List<T> entityList)
        {
            context.UpdateRange(entityList);
            var saveChangesAsync = await context.SaveChangesAsync();
            return saveChangesAsync > 0;
        }
    }


}
