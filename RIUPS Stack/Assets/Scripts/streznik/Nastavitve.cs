using System;
using System.Data.SqlClient;

namespace StackClone
{
    public class Nastavitve
    {
        public Nastavitve() { }

        public static string GetConnectionString()
        {
            SqlConnectionStringBuilder conBuild = new SqlConnectionStringBuilder();
            conBuild.UserID = "sa";
            conBuild.Password = "sM3!999";
            conBuild.MultipleActiveResultSets = true;
            conBuild.PersistSecurityInfo = false;
            conBuild.PersistSecurityInfo = false;
            conBuild.InitialCatalog = "StackDB";
            conBuild.DataSource = @"LUX-PC\SMETANOVASRV01";
            conBuild.Pooling = true;
            conBuild.AsynchronousProcessing = true;
            conBuild.ConnectTimeout = 10;

            return conBuild.ToString();
        }

    }
}