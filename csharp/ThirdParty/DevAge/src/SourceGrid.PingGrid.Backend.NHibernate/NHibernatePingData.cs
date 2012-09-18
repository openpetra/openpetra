
using System;
using System.Collections;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using SourceGrid.Extensions.PingGrids;

namespace SourceGrid.PingGrid.Backend.NHibernate
{
	public class NHibernatePingData<T> : IPingData where T : class
	{
		ISessionFactory sessionFactory = null;
		private Order order = null;
		
		private Dictionary<int, T> cache = new Dictionary<int, T>();
		
		/// <summary>
		/// Use this to inject some custom performance optimized
		/// property value reader.
		/// </summary>
		public IPropertyResolver PropertyResolver{get;set;}
		
		private int? count = null;
		
		public void Invalidate()
		{
			count = null;
			cache.Clear();
		}
		
		public NHibernatePingData(ISessionFactory factory)
		{
			if (factory == null)
				throw new ArgumentNullException();
			this.sessionFactory = factory;
			PropertyResolver = ReflectionPropertyResolver.SharedInstance;
		}
		
		
		public int Count {
			get {
				if (count == null)
				{
					using (var session = sessionFactory.OpenSession())
					{
						// populate the database
						using (var transaction = session.BeginTransaction())
						{
							var criteria = session.CreateCriteria(typeof(T))
								.SetProjection(Projections.RowCount());
							var res = criteria.UniqueResult<int>();
							count = res;
						}
					}
				}
				return count.Value;
			}
		}
		
		public bool AllowSort {
			get {
				return true;
			}
			set {
			}
		}
		
		public void ApplySort(string propertyName, bool @ascending)
		{
			var sort = Projections.Property(propertyName);
			if (ascending == true)
				order = Order.Asc(sort);
			else
				order = Order.Desc(sort);
			
			Invalidate();
		}
		
		
		/// <summary>
		/// Gets value from NHibernate.
		/// 
		/// In fact we do not need whole object here, w
		/// </summary>
		/// <param name="index"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public object GetItemValue(int index, string propertyName)
		{
			if (cache.ContainsKey(index) == false)
				InitCache(index);
			if (cache.ContainsKey(index) == false)
				return null;
			
			var obj = cache[index];
			
			return PropertyResolver.ReadValue(obj, propertyName);
		}
		
		private void InitCache(int index)
		{
			var from = index - 50;
			var to = index + 50;
			
			if (from < 0)
				from = 0;
			if (to > Count)
				to = Count;
			
			using (var session = sessionFactory.OpenStatelessSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					var criteria =
						session.CreateCriteria(typeof(T));
					//var idProp = Projections.Id();
					criteria
						.SetFirstResult(from)
						.SetMaxResults(to - from);
					//.Add(Restrictions.Ge(idProp, from))
					//.Add(Restrictions.Le(idProp, to))
					//.AddOrder(Order.Asc(idProp))
					if (order != null)
						criteria.AddOrder(order);
					
					var obj = criteria.List<T>();
					transaction.Commit();
					if (obj == null)
						return;
					for (int i = from; i < to; i ++)
					{
						if (cache.ContainsKey(i))
							cache.Remove(i);
						cache.Add(i, obj[i - from]);
					}
				}
			}
		}
		
	}
}