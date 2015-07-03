namespace Domain.Data.EntityFramework
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly DbContext context;
		private readonly DbSet<T> databaseSet;
		private readonly string[] includes = new string[0];

		public Repository(DbContext context)
		{
			this.context = context;
			this.context.Database.Log = s => Console.Write(s);
			this.databaseSet = context.Set<T>();
		}

		public Repository(DbContext context, string[] includes)
			: this(context)
		{
			this.includes = includes;
		}

		public virtual T Get(int id)
		{
			return this.databaseSet.Find(id);
		}

        public virtual IQueryable<T> Get()
        {
            if (this.includes.Length == 0)
            {
                return this.databaseSet;
            }
            else
            {
                var query = this.databaseSet.Include(this.includes[0]);
                if (this.includes.Length > 1)
                {
                    for (var i = 1; i < this.includes.Length; i++)
                    {
                        query = query.Include(this.includes[i]);
                    }
                }

                return query;
            }
        }
        
        public virtual async Task<T> GetAsync(int id)
		{
			return await this.databaseSet.FindAsync(id);
		}

 		public virtual T Insert(T entity)
		{
			this.databaseSet.Add(entity);
			this.context.Entry(entity).State = EntityState.Added;
			return entity;
		}

		public virtual T Update(T entityToUpdate)
		{
			this.databaseSet.Attach(entityToUpdate);
			this.context.Entry(entityToUpdate).State = EntityState.Modified;
			return entityToUpdate;
		}

		public virtual void Delete(int id)
		{
			T entityToDelete = this.databaseSet.Find(id);
			this.Delete(entityToDelete);
		}

		public virtual void Delete(T entityToDelete)
		{
			if (this.context.Entry(entityToDelete).State == EntityState.Detached)
			{
				this.databaseSet.Attach(entityToDelete);
			}

			this.databaseSet.Remove(entityToDelete);
		}
	}
}
