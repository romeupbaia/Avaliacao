using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados_Covid_19.DAO
{
    class Conexao
    {
        SqlConnection con = new SqlConnection();
        //Consutrutor
        public Conexao()
        {
            con.ConnectionString = @"Data Source=DESKTOP-PBF9B3B\SQLEXPRESS;Initial Catalog=dbCovid;Integrated Security=True";
        }

        //Conectar
        public SqlConnection Conectar()
        {
            if(con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }

        //Desconctar
        public void Desconectar()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}
