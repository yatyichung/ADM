namespace Airport_Management_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testing : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Passengers", "CheckInStatus", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Passengers", "CheckInStatus", c => c.Time(nullable: false, precision: 7));
        }
    }
}
