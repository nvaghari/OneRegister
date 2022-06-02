using OneRegister.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OneRegister.Data.Contract;

public interface IOrganizedRepository<T>
{
    OneRegisterContext Context { get; }
    IQueryable<T> FilteredEntities { get; }
    IQueryable<T> Entities { get; }
    List<T> GetList(bool asNoTrack = false);
    T GetById(Guid id, bool asNoTrack = false);
    T GetById(Guid id, bool asNoTrack, params Expression<Func<T, object>>[] includes);
    T GetByIdAsAdmin(Guid id, bool asNoTrack = false);
    bool AnyByIdAsAdmin(Guid id);
    void Update(T entity);
    void Add(T entity);
    void Delete(Guid id);
    void Remove(Guid id);
    string UserOrganizationPath { get;}
    Guid? CurrentUserId { get; }
}
