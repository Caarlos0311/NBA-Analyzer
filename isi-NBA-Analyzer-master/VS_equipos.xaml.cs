//using Mysqlx.Datatypes;
using NBA_Analyzer.API;
using NBA_Analyzer.API2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NBA_Analyzer
{
    /// <summary>
    /// Lógica de interacción para VS_equipos.xaml
    /// </summary>
    public partial class VS_equipos : UserControl
    {
        public List<Team> lista_team = new List<Team>();
        public Boolean arriba = false;
        public Boolean abajo = false;
        public Team equipo1;
        public Team equipo2;
        public Fotos fotos = new Fotos();
        public VS_equipos(List<Team> lista_equipos)
        {
            InitializeComponent();
            arriba = false;
            abajo = false;
            lista_team = lista_equipos;
            for (int i = 2022; i > 1978; i--)
            {
                anios.Items.Add(i.ToString());
            }
            foreach (Team equi in lista_equipos)
            {
                listaEquipos.Items.Add(equi.full_name);
                listaEquipos2.Items.Add(equi.full_name);
            }
        }

        private void listaEquipos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var equipo = listaEquipos.SelectedItem;
            foreach (Team team in lista_team)
            {
                if (team.full_name == equipo)
                {
                    equipo1 = team;
                }
            }
            arriba = true;
            comparar();

        }

        public void comparar()
        {
            int casa1 = 0;
            int casa2 = 0;
            int fuera1 = 0;
            int fuera2 = 0;

            int vicasa1 = 0;
            int vicfuera1 = 0;
            int dercasa1 = 0;
            int derfuera1 = 0;

            int vicasa2 = 0;
            int vicfuera2 = 0;
            int dercasa2 = 0;
            int derfuera2 = 0;
            if (arriba && abajo)
            {
                using (var client = new HttpClient())
                {
                    string anio = "2021";
                    var ano = anios.SelectedItem;
                    if (ano != null)
                    {
                        anio = ano.ToString();
                    }

                    Boolean postseason = (bool)label_Postseason.IsChecked;
                    int max = 4;
                    for(int i=0; i < max; i++) { 
                    string url = "https://www.balldontlie.io/api/v1/games?seasons[]=" + anio + "&team_ids[]=" + equipo1.id + "&team_ids[]=" + equipo2.id + "&postseason=" + postseason.ToString()+"&per_page=100&page="+i;

                    client.DefaultRequestHeaders.Clear();

                    var response = client.GetAsync(url).Result;

                    var res = response.Content.ReadAsStringAsync().Result;
                    dynamic r = JObject.Parse(res);
                        max = r.meta.total_pages;
                        foreach (var datos in r.data)
                        {
                            
                            if (datos.home_team.id == equipo1.id)
                            {
                                casa1 += 1;
                                if (datos.home_team_score > datos.visitor_team_score)
                                {
                                    vicasa1 += 1;
                                }
                                else
                                {
                                    dercasa1 += 1;
                                }
                            }
                            else if (datos.visitor_team.id == equipo1.id)
                            {
                                fuera1 += 1;
                                if (datos.home_team_score > datos.visitor_team_score)
                                {
                                    derfuera1 += 1;
                                }
                                else
                                {
                                    vicfuera1 += 1;
                                }
                            }

                            else if (datos.home_team.id == equipo2.id)
                            {
                                casa2 += 1;
                                if (datos.home_team_score > datos.visitor_team_score)
                                {
                                    vicasa2 += 1;
                                }
                                else
                                {
                                    dercasa2 += 1;
                                }
                            }
                            else if (datos.visitor_team.id == equipo2.id)
                            {
                                fuera2 += 1;
                                if (datos.home_team_score > datos.visitor_team_score)
                                {
                                    derfuera2 += 1;
                                }
                                else
                                {
                                    vicfuera2 += 1;
                                }
                            }

                        }


                    }

                }
                int partidos1=casa1 + fuera1;
                int partidos2=casa2+fuera2;
                using (var cliente = new HttpClient())
                {
                    string url2 = "https://image-charts.com/chart.js/2.8.0?bkg=white&c=";
                    cliente.DefaultRequestHeaders.Clear();
                    string parametros = "{\"type\": \"pie\", \"data\": { \"labels\": [ 'victorias en casa' ,'derrotas en casa','victorias fuera','derrotas fuera'], \"datasets\":[ { \"backgroundColor\":[ \"rgba(255,99,132,0.5)\",\"rgba(54,162,235,0.5)\", \"rgba(255,205,86,0.5)\",\"rgba(75,192,192,0.5)\"] ,    \"data\": [" + vicasa1 + "," + dercasa1 + "," + vicfuera1 + "," + derfuera1 + "],\"label\": \"" + equipo1.full_name + "\"}]}}";
                    dynamic jsonString = JObject.Parse(parametros);
                    var httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                    fotos.url = url2 + "" + jsonString;
                    
                }
                using (var cliente = new HttpClient())
                {
                    string url2 = "https://image-charts.com/chart.js/2.8.0?bkg=white&c=";
                    cliente.DefaultRequestHeaders.Clear();
                    string parametros = "{\"type\": \"pie\", \"data\": { \"labels\": [ 'victorias en casa' ,'derrotas en casa','victorias fuera','derrotas fuera'], \"datasets\":[ { \"backgroundColor\":[ \"rgba(255,99,132,0.5)\",\"rgba(54,162,235,0.5)\", \"rgba(255,205,86,0.5)\",\"rgba(75,192,192,0.5)\"] ,    \"data\": [" + vicasa2 + "," + dercasa2 + "," + vicfuera2 + "," + derfuera2 + "],\"label\": \"" + equipo2.full_name + "\"}]}}";
                    dynamic jsonString = JObject.Parse(parametros);
                    var httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                    fotos.url3 = url2 + "" + jsonString;

                }
                using (var cliente = new HttpClient())
                {
                    string url2 = "https://image-charts.com/chart.js/2.8.0?bkg=white&c=";
                    cliente.DefaultRequestHeaders.Clear();
                    string parametros = "{\"type\": \"bar\", \"data\": { \"labels\": [ 'partidos','partidos en casa','victorias en casa' ,'derrotas en casa','partidos fuera','victorias fuera','derrotas fuera'], \"datasets\":[ {     \"data\": ["+partidos1+","+casa1+"," + vicasa1 + "," + dercasa1 + ","+fuera1+"," + vicfuera1 + "," + derfuera1 + "],\"label\": \"" + equipo1.full_name + "\"},{ \"data\": ["+partidos2+","+casa2+"," + vicasa2 + "," + dercasa2 + ","+fuera2+"," + vicfuera2 + "," + derfuera2 + "],\"label\": \"" + equipo2.full_name + "\"}]}}";
                    dynamic jsonString = JObject.Parse(parametros);
                    var httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                    fotos.url2 = url2 + "" + jsonString;

                }
                OpenControl(new statEquipos(fotos,equipo1,equipo2));


            }

        }

        private void listaEquipos2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var equipo = listaEquipos2.SelectedItem;
            foreach (Team team in lista_team)
            {
                if (team.full_name == equipo)
                {
                    equipo2 = team;
                }
            }
            abajo = true;
            comparar();
        }

        private System.Windows.Controls.UserControl activeWindow = null;

        public void OpenControl(UserControl cont)
        {
            if (activeWindow != null)
            {
                ventana.Children.Clear();
            }
            activeWindow = cont;
            ventana.Children.Add(cont);
        }
    }
}
