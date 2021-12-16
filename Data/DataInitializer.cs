using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Data.Models;

namespace Data
{

    public class DataInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<Datacontext>
    {
        protected override void Seed(Datacontext context)
        {
            var SchoolTypes = new List<School_Type> {
                new School_Type{Type = "Gymnasie" },
                new School_Type{Type = "Högskola" },
                new School_Type{Type = "Universitet" }
            };
            SchoolTypes.ForEach(s => context.School_Types.Add(s));
            context.SaveChanges();
        }
    }
}