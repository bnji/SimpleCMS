using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS.Web
{
    public static class Extensions
    {
        public static T FindWithAutoDectectChangesDisabled<T>(this DbSet<T> dbSet, DbContext context, int? id) where T : class
        {
            T result = null;
            context.Configuration.AutoDetectChangesEnabled = false;
            result = dbSet.Find(id);
            context.Configuration.AutoDetectChangesEnabled = true;
            return result;
        }
    }
}
