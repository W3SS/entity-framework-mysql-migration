using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace EFMigration
{
	public class Customer
	{
		[Key]
		public int Id { set; get; }

		public string FirstName { set; get; }
		public string LastName { set; get; }
	}

	public class EFContext : DbContext
	{
		public EFContext (string connName):base (connName) {
			
		}

		public DbSet<Customer> Customers { set; get; }
	}

	class MainClass
	{
		/*
		public static void Main (string [] args)
		{
			//Database.SetInitializer<EFContext> (new MigrateDatabaseToLatestVersion<EFContext, Configuration> ());
			Database.SetInitializer<EFContext> (new CreateDatabaseIfNotExists<EFContext> ());
			using (var context = new EFContext ("name=test")) {
				context.Customers.Add (new Customer { });
				context.SaveChanges ();
			}
		}
*/
	}
}
