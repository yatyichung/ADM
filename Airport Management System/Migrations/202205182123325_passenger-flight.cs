namespace Airport_Management_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class passengerflight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Passengers", "FlightId", c => c.Int(nullable: false));
            CreateIndex("dbo.Passengers", "FlightId");
            AddForeignKey("dbo.Passengers", "FlightId", "dbo.Flights", "FlightId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Passengers", "FlightId", "dbo.Flights");
            DropIndex("dbo.Passengers", new[] { "FlightId" });
            DropColumn("dbo.Passengers", "FlightId");
        }
    }
}
