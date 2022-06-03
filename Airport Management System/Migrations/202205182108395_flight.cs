namespace Airport_Management_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class flight : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Flights",
                c => new
                    {
                        FlightId = c.Int(nullable: false, identity: true),
                        Airline = c.String(),
                        FlightNum = c.String(),
                        Terminal = c.Int(nullable: false),
                        Gate = c.String(),
                        Destination = c.String(),
                        DepartureTime = c.DateTime(nullable: false),
                        ArrivalTime = c.DateTime(nullable: false),
                        FlightStatus = c.String(),
                    })
                .PrimaryKey(t => t.FlightId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Flights");
        }
    }
}
