using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.DataEntity;

namespace URFX.Data
{
    public class URFXContextInitializer: DropCreateDatabaseAlways<URFXDbContext>
    {
        protected override void Seed(URFXDbContext context)
        {
            var sqlFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "URFX.Data", "SqlFiles", "*.sql")).OrderBy(x => x);
            foreach (string file in sqlFiles)
            {
                context.Database.ExecuteSqlCommand(File.ReadAllText(file));
            }
            base.Seed(context);

        }
    }
}
