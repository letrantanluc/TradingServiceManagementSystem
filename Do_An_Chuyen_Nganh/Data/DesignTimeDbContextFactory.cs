//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.EntityFrameworkCore;
//using Oracle.ManagedDataAccess.Client;

//namespace Do_An_Chuyen_Nganh.Data
//{
//    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OracleDbContext>
//    {
//        public OracleDbContext CreateDbContext(string[] args)
//        {
//            IConfigurationRoot configuration = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("appsettings.json")
//                .Build();

//            var builder = new DbContextOptionsBuilder<OracleDbContext>();
//            var connectionString = configuration.GetConnectionString("OracleConnection");

//            // Create a connection with SYSDBA privilege
//            var sysdbaConnectionString = new OracleConnectionStringBuilder(connectionString)
//            {
//                UserID = "sys",
//                Password = "123",
//                DBAPrivilege = "SYSDBA"
//            }.ToString();

//            builder.UseOracle(sysdbaConnectionString);

//            return new OracleDbContext(builder.Options);
//        }
//    }
//}
