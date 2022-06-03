namespace Airport_Management_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class passenger : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Passengers",
                c => new
                    {
                        PassengerId = c.Int(nullable: false, identity: true),
                        PassengerFName = c.String(),
                        PassengerLName = c.String(),
                        PassengerPassport = c.String(),
                        CheckInStatus = c.DateTime(),
                        PassengerSeatingClasses = c.String(),
                        PassengerSeatingNum = c.String(),
                    })
                .PrimaryKey(t => t.PassengerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Passengers");
        }
    }
}
