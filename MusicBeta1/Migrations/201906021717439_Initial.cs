namespace MusicBeta1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Musics",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Artist = c.String(),
                        Genre = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                        OriginalFileName = c.String(),
                        MusicPath = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Musics");
        }
    }
}
