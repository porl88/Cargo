namespace Domain.Testing
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Domain.Data.EntityFramework;
	using Domain.Entities;

	public class MockRepository<T> : IRepository<T> where T : BaseEntity
	{
		private List<T> entities = new List<T>();

		protected List<T> Entities
		{
			get
			{
				return this.entities;
			}
		}

		public virtual T Get(int id)
		{
			return this.entities.FirstOrDefault(x => x.Id == id);
		}

		public virtual async Task<T> GetAsync(int id)
		{
			throw new NotImplementedException();
		}

		public virtual IQueryable<T> Get()
		{
			return this.entities.AsQueryable();
		}

		public virtual IQueryable<T> Find(Expression<Func<T, bool>> expression)
		{
			var predicate = expression.Compile();
			return this.entities.Where(predicate).AsQueryable();
		}

		public virtual T Insert(T entity)
		{
			if (entity.Id > 0)
			{
				var entityInList = this.entities.FirstOrDefault(x => x.Id == entity.Id);
				if (entityInList != null)
				{
					entityInList = entity;
				}
				else
				{
					this.entities.Add(entity);
				}
			}
			else
			{
				if (this.entities.Count > 0)
				{
					entity.Id = this.entities.Max(x => x.Id) + 1;
				}
				else
				{
					entity.Id = 1;
				}

				this.entities.Add(entity);
			}

			return entity;
		}

		public virtual T Update(T entity)
		{
			if (entity.Id > 0)
			{
				var entityInList = this.entities.FirstOrDefault(x => x.Id == entity.Id);
				if (entityInList != null)
				{
					entityInList = entity;
				}
				else
				{
					this.entities.Add(entity);
				}
			}
			else
			{
				if (this.entities.Count > 0)
				{
					entity.Id = this.entities.Max(x => x.Id) + 1;
				}
				else
				{
					entity.Id = 1;
				}

				this.entities.Add(entity);
			}

			return entity;
		}

		public virtual void Delete(T entity)
		{
			this.entities.Remove(entity);
		}

		public virtual void Delete(int id)
		{
			var entityToRemove = this.Find(x => x.Id == id).FirstOrDefault();
			if (entityToRemove != null)
			{
				this.Delete(entityToRemove);
			}
		}


        public Task<List<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> FindAsync(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}
