using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class FunduContext : DbContext
    {
        public FunduContext(DbContextOptions options) : base (options)
        {}
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<NotesEntity> UserNotes { get; set; }
        public DbSet<LableEntity> Lable { get; set; } 
        public DbSet<CollabEntity> Collaborator { get; set; } 
    }
}
                       