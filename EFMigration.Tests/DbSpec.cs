using System;
using NUnit.Framework;
using System.Configuration;
using System.Data.Entity;
using System.IO;

namespace EFMigration.Tests
{
	public class DbSpec
	{
		static DbSpec ()
		{
			AppDomain.CurrentDomain.SetData ("APP_CONFIG_FILE", "/Users/wk/Source/ef/EFMigration/EFMigration.Tests/bin/Debug/EFMigration.Tests.dll.config");
		}

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
		public void ShouldGetConfigFile ()
		{
			var config = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			Assert.Fail (config);
		}

		[Test]
		public void ShouldGetConfigFile2 ()
		{
			var config = ConfigurationManager.OpenExeConfiguration (ConfigurationUserLevel.None);
			Assert.Fail (config.FilePath);
		}
	}
}

