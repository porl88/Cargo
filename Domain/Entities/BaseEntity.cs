namespace Domain.Entities
{
	using System;

	public abstract class BaseEntity
	{
		public int Id { get; set; }

		public DateTimeOffset Created { get; set; }

		public DateTimeOffset Updated { get; set; }
	}
}
