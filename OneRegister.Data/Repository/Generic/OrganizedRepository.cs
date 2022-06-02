using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Context;
using OneRegister.Data.Contract;
using OneRegister.Security.Services.HttpContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OneRegister.Data.Repository.Generic
{
    public class OrganizedRepository<T> : IOrganizedRepository<T> where T: class,IOrganizedEntity
    {
        private readonly IHttpContextPermissionHandler _permissionHandler;


        public OrganizedRepository(OneRegisterContext context, IHttpContextPermissionHandler permissionHandler)
        {
            Context = context;
            _permissionHandler = permissionHandler;
            FilteredEntities = Context.Set<T>().Include(x=>x.Organization).Where(x => x.Organization.Path.StartsWith(UserOrganizationPath));
            Entities = Context.Set<T>().Include(x=>x.Organization);
        }
        public string UserOrganizationPath => _permissionHandler.UserOrganizationPath();
        public Guid? CurrentUserId => _permissionHandler.UserId();
        public OneRegisterContext Context { get; }
        public IQueryable<T> FilteredEntities { get; }
        public IQueryable<T> Entities { get; }

        public bool AnyByIdAsAdmin(Guid id)
        {
            return Entities.Any(x => x.Id == id);
        }

        public T GetById(Guid id, bool asNoTrack = false)
        {
            var query = asNoTrack ? FilteredEntities.AsNoTracking() : FilteredEntities;
            query = query.Where(x => x.Id == id);
            return query.FirstOrDefault();
        }
        public T GetById(Guid id, bool asNoTrack, params Expression<Func<T, object>>[] includes)
        {
            var query = asNoTrack ? FilteredEntities.AsNoTracking() : FilteredEntities;
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            query = query.Where(x => x.Id == id);
            return query.FirstOrDefault();
        }
        public T GetByIdAsAdmin(Guid id, bool asNoTrack = false)
        {
            var query = asNoTrack ? Entities.AsNoTracking() : Entities;
            query = query.Where(x => x.Id == id);
            return query.FirstOrDefault();
        }

        public List<T> GetList(bool asNoTrack = false)
        {
            var query = asNoTrack ? FilteredEntities.AsNoTracking() : FilteredEntities;
            return query.ToList();
        }

        public void Update(T entity)
        {
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserId;

            Context.Update(entity);
            Context.SaveChanges();
        }
        public void Add(T entity)
        {
            entity.CreatedBy = entity.ModifiedBy = CurrentUserId;
            if (entity.State == StateOfEntity.Init)
            {
                entity.State = StateOfEntity.InProgress;
            }
            Context.Add(entity);
            Context.SaveChanges();
        }
        public void Delete(Guid id)
        {
            var entity = Entities.FirstOrDefault(x => x.Id == id);
            if (entity != null) {
                entity.State = StateOfEntity.Deleted;
                Context.SaveChanges();
            }
            
        }
        public void Remove(Guid id)
        {
            var entity = Entities.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                Context.Remove(entity);
                Context.SaveChanges();
            }

        }

    }
}
