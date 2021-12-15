﻿using Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Datacontext : DbContext
    {
        public Datacontext() : base("Datacontext")
        {

        }
        public DbSet<CV> CVs { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<School_Type> School_Types { get; set; }

        public DbSet<Skill> Skills { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<User_Message> User_Messages { get; set; }
        public DbSet<Work_Experience> Work_Experiences { get; set; }

    }
}
