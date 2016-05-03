using System;
using NUnit.Framework;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Reflection;

namespace EFMigration.Tests
{
	[TestFixture]
	public class DbSpec
	{
		[Test]
		public void ShouldCreateSchema ()
		{
			Database.SetInitializer<EFContext> (new CreateDatabaseIfNotExists<EFContext> ());
			using (var context = new EFContext ("name=test")) {
				context.Customers.Add (new Customer { });
				context.SaveChanges ();
			}
		}

		[Test]
		public void ShouldMigrateDb ()
		{

		}

		[Test]
		public void ShouldGetConfigFile ()
		{
			var config = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			Assert.True (config.Contains ("bin/Debug"));
		}

		[Test]
		public void ShouldGetConfigFile2 ()
		{
			var config = ConfigurationManager.OpenExeConfiguration (ConfigurationUserLevel.None).FilePath;
			Assert.True (config.Contains ("bin/Debug"));
		}
	}
}

