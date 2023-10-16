using Microsoft.EntityFrameworkCore;
using ms_forum.Domains;
using System.Data.Common;

namespace ms_forum
{
    public class ForumDbContext : DbContext, IDbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options) { }
        public DbSet<Forum> Forum { get; set; }
        public DbSet<ForumTopicoReplica> ForumTopicoReplica { get; set; }
        public DbSet<ForumTopicoResposta> ForumTopicoResposta { get; set; }
        public DbSet<ForumTag> ForumTag { get; set; }
        public DbSet<ForumTopicoTag> ForumTopicoTag { get; set; }
        public DbSet<ForumTopico> ForumTopico { get; set; }

        public DbConnection Connection => base.Database.GetDbConnection();
    }
}
