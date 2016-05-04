namespace EFMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgeAndTelephone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "Telephone", c => c.Int(nullable: false));
            AlterColumn("dbo.Customers", "FirstName", c => c.String(unicode: false));
            AlterColumn("dbo.Customers", "LastName", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "LastName", c => c.String());
            AlterColumn("dbo.Customers", "FirstName", c => c.String());
            DropColumn("dbo.Customers", "Telephone");
            DropColumn("dbo.Customers", "Age");
        }
    }
}
