using Dados_Covid_19.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados_Covid_19.DAO
{
    public class DadosCovidDAO
    {   
        //Informações de conexão
        Conexao conexao = new Conexao();
        Dados dados = new Dados();
        SqlCommand cmd = new SqlCommand();

        public String mensagem ="";

        public void Inserir(Dados dados, Usuario usuario)
        {
            cmd.CommandText = ("INSERT INTO Dados (confirmados, mortes, LocalLat, localLong, data) VALUES (@confirmados, @mortes, @localLat, @localLong, @data)");

            cmd.Parameters.AddWithValue("@confirmados", dados.Confirmed);//Alterar informação
            cmd.Parameters.AddWithValue("@mortes", dados.Deaths);//Alterar informação
            cmd.Parameters.AddWithValue("@localLat", usuario.localLat);//Alterar informação
            cmd.Parameters.AddWithValue("@localLong", usuario.localLong);//Alterar informação
            cmd.Parameters.AddWithValue("@data", usuario.Data);//Alterar informação

            try
            {
                cmd.Connection = conexao.Conectar();
                cmd.ExecuteNonQuery();
                conexao.Desconectar();

                this.mensagem = "Cadastrado com sucesso!!";
            }
            catch (SqlException e)
            {
                this.mensagem = "Erro ao se conectar!" + e;
            }

        }
    } 
}
