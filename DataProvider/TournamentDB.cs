namespace DataProvider
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class TournamentDB : DbContext
    {
        // Your context has been configured to use a 'TournamentDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DataProvider.TournamentDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'TournamentDB' 
        // connection string in the application configuration file.
        public TournamentDB()
            : base("name=TournamentDB")
        {
        //    Database.SetInitializer(new DropCreateDatabaseAlways<TournamentDB>());            
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Session> Sessions { get; set; } 
        public virtual DbSet<Stripe> Stripes { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Player> Players { get; set; }
    }

   
}