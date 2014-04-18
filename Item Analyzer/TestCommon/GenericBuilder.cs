using System;
using System.Collections.Generic;
using System.Linq;

namespace TestCommon
{
	public class GenericBuilder<TEntity> where TEntity : new()
	{
		private readonly List<Func<TEntity, object>> setters;

		public GenericBuilder()
		{
			setters = new List<Func<TEntity, object>>();
		}

		public GenericBuilder<TEntity> With(params Func<TEntity, object>[] props)
		{
			setters.AddRange(props);
			return this;
		}

		public TEntity Build()
		{
			var model = new TEntity();
			setters.ForEach(s => s.Invoke(model));
			return model;
		}
	}
}