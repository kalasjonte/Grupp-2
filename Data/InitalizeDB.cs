using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Data.Models;

namespace Data
{

    public class InitalizeDB : System.Data.Entity.DropCreateDatabaseIfModelChanges<Datacontext>
    {
        protected override void Seed(Datacontext context)
        {
            var SchoolTypes = new List<School_Type> {
                new School_Type{School_TypeID = 1, Type = "Gymnasie" },
                new School_Type{School_TypeID = 2, Type = "Högskola" }
            };
            SchoolTypes.ForEach(s => context.School_Types.Add(s));
            context.SaveChanges();
        }
    }
}
