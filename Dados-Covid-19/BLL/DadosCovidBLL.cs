using Dados_Covid_19.DAO;
using Dados_Covid_19.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados_Covid_19.BLL
{
    public class DadosCovidBLL
    {
        DadosCovidDAO dao = new DadosCovidDAO();
       public void Cadastrar(Dados dados, Usuario usuario)
        {
            dao.Inserir(dados, usuario);
        }
    }
}
