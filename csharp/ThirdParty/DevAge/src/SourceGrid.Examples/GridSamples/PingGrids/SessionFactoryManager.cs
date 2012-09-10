
using System;
using System.IO;
using System.Windows.Forms;

using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace WindowsFormsSample.GridSamples.PingGrids
{
	public class SessionFactoryManager
	{
		public event EventHandler DatabaseCreated;
		public event EventHandler DatabaseOpened;
		
		public FirebirdEmbeddedPreparer FirebirdEmbeddedPreparer{get;set;}
		public EmptyFirebirdDatabasePreparer EmptyFirebirdDatabasePreparer{get;set;}
		
		protected virtual void OnDatabaseOpened(EventArgs e)
		{
			if (DatabaseOpened != null) {
				DatabaseOpened(this, e);
			}
		}
		
		protected virtual void OnDatabaseCreated(EventArgs e)
		{
			if (DatabaseCreated != null) {
				DatabaseCreated(this, e);
			}
		}
		
		
		/// <summary>
		/// Set to true to automatically create DB schema
		/// upon connecting
		/// </summary>
		public bool Export {get;set;}
		private ISessionFactory m_factory = null;
		
		public ISessionFactory SessionFactory {
			get { return m_factory; }
		}
		
		
		/// <summary>
		/// Will return null, if user will not want to create a new database
		/// </summary>
		/// <returns></returns>
		public ISessionFactory CreateSessionFactory()
		{
			FirebirdEmbeddedPreparer.EnsureFirebirdReady();
			
			
			// Ask if user wants to create a database
			// If this class will be used from non-gui environment, this code will have 
			// to be extracted somewhere else
			if (EmptyFirebirdDatabasePreparer.ExistsFile() == false)
			{
				var res = MessageBox.Show("FireBird databse not yet created. Do you want to create it now?",
				                          "Db not exists",
				                          MessageBoxButtons.YesNo,
				                          MessageBoxIcon.Question);
				if (res == DialogResult.No)
				{
					return null;
				}
				EmptyFirebirdDatabasePreparer.Copy();
				this.Export = true;
			}
			
			
			var pathToDB = Path.Combine(Directory.GetCurrentDirectory(), "frmSample60.fdb");
			
			var conf = new FluentNHibernate.Cfg.Db.FirebirdConfiguration()
				.ShowSql()
				.ConnectionString(string.Format(@"
	User=SYSDBA;Password=masterkey;Database={0};
	DataSource=localhost; Port=3050;Dialect=3; Charset=UNICODE_FSS;Role=;Connection lifetime=0;Pooling=true;
	MinPoolSize=1;MaxPoolSize=50;Packet Size=8192;ServerType=1;", pathToDB));
			
			m_factory = Fluently.Configure()
				.Database(conf)
				.Mappings(m =>
				          m.FluentMappings.AddFromAssemblyOf<frmSample60>())
				.ExposeConfiguration(BuildSchema)
				.BuildSessionFactory();
			return m_factory;
		}
		
		private void BuildSchema(Configuration config)
		{
			// delete the existing db on each run
			//if (File.Exists(DbFile))
			//    File.Delete(DbFile);
			
			// this NHibernate tool takes a configuration (with mapping info in)
			// and exports a database schema from it
			new SchemaExport(config)
				.Create(false, Export);
			
			if (Export)
			{
				OnDatabaseCreated(EventArgs.Empty);
			}
			
			OnDatabaseOpened(EventArgs.Empty);
		}
	}
}
