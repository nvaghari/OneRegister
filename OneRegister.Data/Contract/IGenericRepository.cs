using OneRegister.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Data.Contract
{
    public interface IGenericRepository<T>
    {
        OneRegisterContext Context { get; }
        IQueryable<T> Entities { get; }
        List<T> GetList(bool asNoTrack = false);
        T GetById(Guid id, bool asNoTrack = false);
        List<T> GetByState(StateOfEntity state, bool asNoTrack = false);
        void Update(T entity);
        void Add(T entity);
        void Add(List<T> entities);
        void Delete(Guid id);
        void Remove(Guid id);
        Guid? CurrentUserId { get; }
    }
}
