using NBA_Analyzer.API;
using NBA_Analyzer.API2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Lógica de interacción para equipos.xaml
    /// </summary>
    public partial class equipos : UserControl
    {
        public List<Jugador> lista_Jugadores=new List<Jugador>();
        
        static HttpClient client = new HttpClient();
        public Fotos fotos = new Fotos();
        public Jugador jugadorr;
        public equipos()
        {

            this.InitializeComponent();

        }




        private UserControl activeWindow = null;

        public void OpenControl(UserControl cont)
        {
            if (activeWindow != null)
            {
                ventana.Children.Clear();
            }
            activeWindow = cont;
            ventana.Children.Add(cont);
        }

        private void listaJugadores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int ultAnio = 2022;
            try { 


                var jugador = listaJugadores.SelectedItem;
                if (jugador != null)
                {


                    string id = "";
                    string datitos = "]";
                    string ppartidos = "]";
                    Boolean salir = false;
                    
                    string[] anios = new string[5];

                    string puntos="0";
                    string rebotes="0";
                    string robos = "0";
                    string asistencias = "0";
                    string tapones = "0";
                    string perdidas = "0";

                    for (int i = 0; i < anios.Length; i++)
                    {
                        anios[i] = "nada";
                    }
                    int x = 4;
                    foreach (Jugador ju in lista_Jugadores)
                    {
                        string nombre = ju.first_name + " " + ju.last_name;
                        if (nombre == jugador.ToString())
                        {
                            jugadorr = ju;
                            id = ju.id;
                            break;
                        }

                    }

                    using (var client = new HttpClient())
                    {
                        for (int i = 2022; salir == false; i--)
                        {
                            string url = "https://www.balldontlie.io/api/v1/season_averages?season=" + i + "&player_ids[]=" + id;

                            client.DefaultRequestHeaders.Clear();

                            var response = client.GetAsync(url).Result;

                            var res = response.Content.ReadAsStringAsync().Result;
                            dynamic r = JObject.Parse(res);
                            foreach (var datos in r.data)
                            {
                                salir = true;
                                ultAnio = i;
                                 puntos=datos.pts.ToString().Replace(',', '.');
                                 rebotes=datos.reb.ToString().Replace(',', '.');
                                robos=datos.stl.ToString().Replace(',', '.');
                                 asistencias = datos.ast.ToString().Replace(',', '.');
                                tapones = datos.blk.ToString().Replace(',', '.');
                                perdidas = datos.turnover.ToString().Replace(',', '.');

                            }

                        }
                        using (var cliente = new HttpClient())
                        {
                            string url2 = "https://image-charts.com/chart.js/2.8.0?bkg=white&c=";
                            cliente.DefaultRequestHeaders.Clear();
                            string parametros = "{\"type\": \"radar\", \"data\": { \"labels\": [ 'Puntos' ,'Rebotes', 'Robos', 'Asistencias','Tapones','perdidas'], \"datasets\":[ { \"backgroundColor\": \"rgba(255,150,150,0.5)\",       \"borderColor\": \"rgb(255,150,150)\", \"data\": [" + puntos + "," + rebotes + "," + robos + "," + asistencias + "," + tapones + "," + perdidas + "],\"label\": "+ultAnio+"}]}}";
                            dynamic jsonString = JObject.Parse(parametros);
                            var httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                            fotos.url2 = url2 + "" + jsonString;

                        }
                    }


                    for (int j = ultAnio; j > (ultAnio - 5); j--)
                        {
                            Boolean entrado = false;
                            string url = "https://www.balldontlie.io/api/v1/season_averages?season=" + j + "&player_ids[]=" + id;

                            client.DefaultRequestHeaders.Clear();

                            var response = client.GetAsync(url).Result;

                            var res = response.Content.ReadAsStringAsync().Result;
                            dynamic r = JObject.Parse(res);
                            foreach (var datos in r.data)
                            {
                                string partidos = datos.games_played.ToString().Replace(',', '.');
                            string ppuntos = datos.pts.ToString().Replace(',', '.');
                                anios[x] = j.ToString();
                                datitos =partidos + ","+datitos;
                                ppartidos = ppuntos +","+ppartidos;
                                x -= 1;
                                entrado = true;
                            }
                            if (entrado == false)
                            {
                                anios[x] = j.ToString();
                                x -= 1;
                                datitos= "0,"+datitos;
                                ppartidos= "0,"+ppartidos;
                            }



                        
                    }

                    datitos= "["+datitos;
                    ppartidos= "["+ppartidos;

                    using (var client = new HttpClient())
                    {
                        string url2 = "https://image-charts.com/chart.js/2.8.0?bkg=white&c=";
                        client.DefaultRequestHeaders.Clear();
                        string prametros = "{  \"type\": \"line\", \"data\": {   \"labels\": [" + anios[0] + ", " + anios[1] + ", " + anios[2] + "," + anios[3] + "," + anios[4] + "],    \"datasets\" : [      {        \"backgroundColor\": \"rgba(255,150,150,0.5)\",       \"borderColor\": \"rgb(255,150,150)\",       \"data\": " + datitos + ",        \"label\": \"Partidos jugados\",       \"fill\": \"origin\"      },{  \"backgroundColor\": \"rgba(54, 162, 235)\",       \"borderColor\": \"rgb(4, 162, 235, 0.5)\", \"data\": " + ppartidos+", \"label\":\"Puntos por Partido\"}    ]  }}";
                        
                        dynamic jsonString = JObject.Parse(prametros);
                        var httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                        fotos.url = url2 + "" + jsonString;

                    }
                }

                OpenControl(new Jugadores(fotos,jugadorr,ultAnio));
            }
            catch
            {
                MessageBox.Show("Numero de llamadas a la API superado, vuelve a intentarlo en unos segundos");
            }
        }

        private void botno_Click(object sender, RoutedEventArgs e)
        {
            string nombre1 = text_jugador.Text;
            using (var client = new HttpClient())
            {
                listaJugadores.Items.Clear();
                string url = "https://www.balldontlie.io/api/v1/players?search=" + nombre1 + "&per_page=100";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;

                var res = response.Content.ReadAsStringAsync().Result;
                dynamic r = JObject.Parse(res);

                foreach (var datos in r.data)
                {
                    listaJugadores.Items.Add(datos.first_name + " " + datos.last_name);
                    string jid = datos.id;
                    string jfi = datos.first_name;
                    string jla = datos.last_name;
                    string jpo = datos.position;
                    Int32 jhef = 0;
                    Int32 jhei = 0;
                    Int32 jwe = 0;
                    try
                    {
                        jhef = datos.height_feet;
                        jhei = datos.height_inches;
                        jwe = datos.weight_pounds;
                    }
                    catch
                    {

                    }
                    string eid = datos.team.id;
                    string abrreviacion = datos.team.abbreviation;
                    string city= datos.team.city;
                    string conference= datos.team.conference;
                    string division= datos.team.division;
                    string full_name= datos.team.full_name;
                    string name=datos.team.name;

                    Team equipito = new Team(eid,abrreviacion,city,conference,division,full_name,name);

                    jugadorr = new Jugador(jid, jfi, jla, jpo, jhef, jhei, jwe, equipito);
                    lista_Jugadores.Add(jugadorr);
                }
            }
        
    }
    }
}
