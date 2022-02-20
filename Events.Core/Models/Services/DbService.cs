

using Events.Api.Models.General;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Events.Service.Service.DataServices;

namespace Events.Core.Models.Services
{
    public interface DbService<T,V> where T : Model
    {

        Task<long> GetLastId();
        Task<T> GetFirst(Expression<Func<T, bool>> predicate);
        Task<T> Find(Expression<Func<T, bool>> predicate);
        Task<T> Find(long id);
        Task Delete(T entity);
        
        Task<List<V>> GetItems(Expression<Func<V, bool>> predicate, PaggingModel page = null,Func<V,bool> func = null);
        Task<Int64> GetCount(Expression<Func<V, bool>> predicate);
        Task<DbResponse<T>> AddItem(T entity);
        void UpdateEntity(Expression<Func<T, bool>> predicate, T entity);
        Task<T> UpdateEntity(T entity);
        Task<bool> UpdateRange(List<T> entityList);
        Task<bool> addRange(List<T> items);
    }
}
