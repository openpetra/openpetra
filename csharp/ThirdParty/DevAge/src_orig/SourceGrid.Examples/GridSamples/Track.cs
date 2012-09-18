
using System;
using FluentNHibernate.Mapping;

namespace WindowsFormsSample.GridSamples.PingGrids
{
	public class TrackMap : ClassMap<Track>
	{
		public TrackMap()
		{
			this.Table("Track");
			Id(x => x.TrackId).GeneratedBy.Native("TRACK_GENERATOR");
			Map(x => x.Name);
			Map(x => x.Composer);
			Map(x => x.DateCreated);
			Map(x => x.IsDeleted);
			Map(x => x.Price);
		}
	}
	
	public class Track
	{
		public virtual int TrackId {get;set;}
		public virtual string Name {get;set;}
		public virtual string Composer {get;set;}
		public virtual DateTime DateCreated {get;set;}
		public virtual bool IsDeleted {get;set;}
		public virtual decimal Price {get;set;}
	}
}
