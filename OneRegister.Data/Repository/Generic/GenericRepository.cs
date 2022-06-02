using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Context;
using OneRegister.Data.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Repository.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IGenericEntity
    {
        public GenericRepository(OneRegisterContext context)
        {
            Context = context;
            Entities = context.Set<T>();
        }
        public OneRegisterContext Context { get; }

        public IQueryable<T> Entities { get; }
        public virtual Guid? CurrentUserId => BasicUser.AdminId;

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

        public void Add(List<T> entities)
        {
            entities.ForEach(x => {
                x.CreatedBy = x.ModifiedBy = CurrentUserId;
                if (x.State == StateOfEntity.Init)
                {
                    x.State = StateOfEntity.InProgress;
                }
            });
            Context.AddRange(entities);
            Context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = Entities.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                entity.State = StateOfEntity.Deleted;
                Context.SaveChanges();
            }
        }

        public T GetById(Guid id, bool asNoTrack = false)
        {
            var query = asNoTrack ? Entities.AsNoTracking() : Entities;
            query = query.Where(x => x.Id == id);
            return query.FirstOrDefault();
        }

        public List<T> GetByState(StateOfEntity state, bool asNoTrack = false)
        {
            var query = asNoTrack ? Entities.AsNoTracking() : Entities;
            return query.Where(x => x.State == state).ToList();
        }

        public List<T> GetList(bool asNoTrack = false)
        {
            var query = asNoTrack ? Entities.AsNoTracking() : Entities;
            return query.ToList();
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

        public void Update(T entity)
        {
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserId;

            Context.Update(entity);
            Context.SaveChanges();
        }
    }
}
