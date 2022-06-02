using OneRegister.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OneRegister.Data.Contract
{
    public interface IOrganizationRepository<T>
    {
        OneRegisterContext Context { get; }
        IQueryable<T> FilteredEntities { get; }
        IQueryable<T> Entities { get; }
        List<T> GetList(bool asNoTrack = false);
        List<T> GetListAsAdmin(bool asNoTrack = false);

        T GetById(Guid id, bool asNoTrack = false);
        T GetById(Guid id, bool asNoTrack, params Expression<Func<T, object>>[] includes);
        T GetByName(string name, bool asNoTrack = false);
        T GetByIdAsAdmin(Guid id, bool asNoTrack = false);
        T GetByIdAsAdmin(Guid id, bool asNoTrack, params Expression<Func<T, object>>[] includes);
        bool AnyByIdAsAdmin(Guid id);
        bool AnyByNameAsAdmin(string name);
        bool AnyByName(string name);
        void Update(T entity);
        void Update(List<T> entities);
        void UpdateAsAdmin(T entity);
        void UpdateAsAdmin(List<T> entities);
        void Add(T entity);
        string UserOrganizationPath { get; }
        Guid? CurrentUserId { get; }
    }
}
