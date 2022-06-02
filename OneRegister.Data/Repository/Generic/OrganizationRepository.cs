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
    public class OrganizationRepository<T> : IOrganizationRepository<T> where T : class, IOrganizationEntity
    {
        private readonly IHttpContextPermissionHandler _permissionHandler;

        public OrganizationRepository(OneRegisterContext context, IHttpContextPermissionHandler permissionHandler)
        {
            Context = context;
            _permissionHandler = permissionHandler;
            Entities = Context.Set<T>();
            FilteredEntities = Context.Set<T>().Where(x => x.Path.StartsWith(UserOrganizationPath));

        }
        public OneRegisterContext Context { get; }

        public IQueryable<T> FilteredEntities { get; }

        public IQueryable<T> Entities { get; }

        public string UserOrganizationPath => _permissionHandler.UserOrganizationPath();

        public Guid? CurrentUserId => _permissionHandler.UserId();

        public void Add(T entity)
        {
            var parent = Context.Organizations.FirstOrDefault(x=>x.Id == entity.ParentId);
            if(parent == null) throw new ApplicationException("Can't add an extra root");

            var sequencer = Context.Organizations.Where(x => x.ParentId == entity.ParentId).Max(x => x.Sequencer) + 1;
            entity.Sequencer = sequencer;
            entity.Path = parent.Path + "." + sequencer.ToString();

            entity.CreatedBy = entity.ModifiedBy = CurrentUserId;
            if (entity.State == StateOfEntity.Init)
            {
                entity.State = StateOfEntity.InProgress;
            }
            Context.Add(entity);
            Context.SaveChanges();
        }

        public bool AnyByIdAsAdmin(Guid id)
        {
            return Entities.Any(x => x.Id == id);
        }

        public bool AnyByName(string name)
        {
            return FilteredEntities.Any(x => x.Name == name);
        }

        public bool AnyByNameAsAdmin(string name)
        {
            return Entities.Any(x => x.Name == name);
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

        public T GetByIdAsAdmin(Guid id, bool asNoTrack, params Expression<Func<T, object>>[] includes)
        {
            var query = asNoTrack ? Entities.AsNoTracking() : Entities;
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            query = query.Where(x => x.Id == id);
            return query.FirstOrDefault();
        }

        public T GetByName(string name, bool asNoTrack = false)
        {
            var query = asNoTrack ? FilteredEntities.AsNoTracking() : FilteredEntities;
            query = query.Where(x => x.Name == name);
            return query.FirstOrDefault();
        }

        public List<T> GetList(bool asNoTrack = false)
        {
            var query = asNoTrack ? FilteredEntities.AsNoTracking() : FilteredEntities;
            return query.ToList();
        }

        public List<T> GetListAsAdmin(bool asNoTrack = false)
        {
            var query = asNoTrack ? Entities.AsNoTracking() : Entities;
            return query.ToList();
        }

        public void Update(T entity)
        {
            var modifiedProperties = Context.Entry(entity).Properties.Where(p => p.IsModified);
            if(modifiedProperties.Any(p=>
            p.Metadata.Name == nameof(IOrganizationEntity.Path) 
            || p.Metadata.Name == nameof(IOrganizationEntity.Sequencer)
            || p.Metadata.Name == nameof(IOrganizationEntity.ParentId)))
            {
                throw new ApplicationException("can't change organization hierarchy in this manner");
            }

            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserId;

            Context.Update(entity);
            Context.SaveChanges();
        }

        public void Update(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsAdmin(T entity)
        {
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserId;

            Context.Update(entity);
            Context.SaveChanges();
        }

        public void UpdateAsAdmin(List<T> entities)
        {
            entities.ForEach(entity => { 
                entity.ModifiedAt = DateTime.Now;
                entity.ModifiedBy = CurrentUserId;
            });

            Context.Update(entities);
            Context.SaveChanges();
        }
    }
}
