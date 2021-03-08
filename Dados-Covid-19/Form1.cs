using Dados_Covid_19.BLL;
using Dados_Covid_19.DTO;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Device.Location;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dados_Covid_19
{
    public partial class Form1 : Form
    {
        private List<Dados> DadosList = new List<Dados>();
        DadosCovidBLL bll = new DadosCovidBLL();
        //Dados Dados = new Dados();
        Usuario usuario = new Usuario();
        public Form1()
        {
            InitializeComponent();

            //Casos de Covid-19 nos ultimos 6 mêses           
            lblCasosC19.Text = Convert.ToString(SomaCasosCovid(String.Format("2020-08-01{0}", "T00:00:01Z")));

            //Média Móvel de casos
            MediaMovelCasos();

            //Dados do Brasi no último mês
            DadosBrasil();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Buscar(String data)
        {
            try
            {
                String url = "https://api.covid19api.com/live/country/brazil/status/confirmed/date/" + data + "";
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                DadosList = JsonConvert.DeserializeObject<List<Dados>>(response.Content);
            }
            catch (Exception)
            {
                MessageBox.Show("Serviço fora do ar!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
        }
        public int SomaCasosCovid(String data)
        {
            Buscar(data);
            int casos = 0;
            foreach (var item in DadosList)
            {
                casos += item.Confirmed;
            }
            return casos;
        }
        public void MediaMovelCasos()
        {
            DateTime diaInicial = DateTime.Now;
            int localQtd1 = 0;
            int localQtd2 = 0;

            diaInicial = diaInicial.AddDays(-14);
            Buscar(String.Format(diaInicial.ToString("yyyy-MM-dd") + "{0}", "T00:00:01Z"));
            DateTime d1 = DateTime.Today.AddDays(-7);
            foreach (var item in DadosList)
            {
                if (item.Date <= d1)
                {
                    localQtd1 += item.Confirmed;
                }
                else
                {
                    localQtd2 += item.Confirmed;
                }
            }

            //Atribui a média móvel = Quantidade de casos em uma semana dividido por 7
            double mediamovel1 = localQtd1 / 7;
            double mediamovel2 = localQtd2 / 7;
            String sinal;

            //Calcula o percentual de variação da média
            double valor = ((mediamovel2 - mediamovel1) / mediamovel1) * 100;
            if (mediamovel1 < mediamovel2)
            {
                lblDadosPercentuais.Text = "A média móvel representa um aumento de " + valor.ToString("N2") + "%";
                sinal = " > ";
            }
            else
            {
                lblDadosPercentuais.Text = "A média móvel representa uma redução de " + valor.ToString("N2") + "%";
                sinal = " < ";
            }
            lblMediaMovel.Text = mediamovel1.ToString() + sinal + mediamovel2.ToString();
        }
        public void DadosBrasil()
        {
            //Atribui o número de dias do mês anterior
            int diasDoMes = DateTime.DaysInMonth(Convert.ToInt32(DateTime.Today.ToString("yyyy")), Convert.ToInt32(DateTime.Today.AddMonths(-2).ToString("MM")));
            Dados dados = new Dados();

            String data = String.Format(DateTime.Today.AddMonths(-2).ToString("yyyy-MM-" + diasDoMes) + "{0}", "T00:00:01Z");

            Buscar(data);

            int maiorConfirmado = 0;
            int maiorMortes = 0;
            int dia = Convert.ToInt32(DateTime.Today.ToString("dd"));
            DateTime h = DateTime.Today.AddDays(-dia);

            foreach (var item in this.DadosList)
            {
                if (item.Date <= h)
                {
                    if (item.Confirmed > maiorConfirmado)
                    {
                        maiorConfirmado = item.Confirmed;
                        dados.Confirmed = item.Confirmed;
                    }
                    if (item.Deaths > maiorMortes)
                    {
                        maiorMortes = item.Deaths;
                        dados.Deaths = item.Deaths;
                    }
                }
            }

            DadosUsuario();
            lblTotalCasosBr.Text = maiorConfirmado.ToString();
            lblTotalMortesBr.Text = maiorMortes.ToString();
            bll.Cadastrar(dados, usuario);
        }

        //Função busca a localização do uusuário no momento de cada consulta.
        public void DadosUsuario()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            usuario = new Usuario();
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
            GeoCoordinate coord = watcher.Position.Location;
            
            if (coord.IsUnknown != true)
            {
                usuario.localLat = coord.Latitude.ToString();
                usuario.localLong = coord.Longitude.ToString();
                usuario.Data = DateTime.Today;
            }
            else
            {
                MessageBox.Show("Não foi possível obter a localização", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void lblCasosC19_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
