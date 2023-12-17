//using Mysqlx.Datatypes;
//using MySqlX.XDevAPI;
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
    /// Lógica de interacción para VS_jugadores.xaml
    /// </summary>
    public partial class VS_jugadores : UserControl
    {
        public List<Jugador>jugadores1=new List<Jugador>();
        public List<Jugador>jugadores2=new List<Jugador>();
        public Jugador jugadore1;
        public Jugador jugadore2;
        public Boolean arriba=false;
        public Boolean abajo=false;
        public Fotos fotos = new Fotos();
        public VS_jugadores()
        {
            InitializeComponent();
            arriba = false;
            abajo = false;
        }

        private void boton1_Click(object sender, RoutedEventArgs e)
        {
            string nombre1 = text_jugador1.Text;
            using (var client = new HttpClient())
            {
                jugador1.Items.Clear();
                string url = "https://www.balldontlie.io/api/v1/players?search=" + nombre1+"&per_page=100";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;

                var res = response.Content.ReadAsStringAsync().Result;
                dynamic r = JObject.Parse(res);

                foreach (var datos in r.data)
                {
                    jugador1.Items.Add(datos.first_name+" "+datos.last_name);
                    string jid = datos.id;
                    string jfi = datos.first_name;
                    string jla = datos.last_name;
                    string jpo = datos.posicion;
                    Int32 jhef = 0;
                    Int32 jhei = 0;
                    Int32 jwe = 0;
                    try
                    {
                        jhef = datos.heigth_feet;
                        jhei = datos.heigth_inches;
                        jhef = datos.weigth_pounds;
                    }
                    catch
                    {

                    }

                    Jugador jugadorr = new Jugador(jid, jfi, jla, jpo, jhef, jhei, jwe,null);
                    jugadores1.Add(jugadorr);
                }
            }
        }

        private void jugador1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (jugador1.SelectedItem != null)
            {
                var nombreJugador = jugador1.SelectedItem.ToString();
                foreach (Jugador jugadorr in jugadores1)
                {
                    string nombre = jugadorr.first_name + " " + jugadorr.last_name;
                    if (nombreJugador == nombre)
                    {
                        jugadore1 = jugadorr;
                    }
                }
                arriba = true;
                datos();
            }
        }
        private void comparar()
        {

            }
        private void datos()
        {
            int ultAnio1 = 2022;
            int ultAnio2 = 2022;
            if (arriba && abajo)
            {
                try
                {

                    string id1 = jugadore1.id;
                    string id2 = jugadore2.id;
                    string partidos = "]";
                    string puntos = "]";
                    string asistencias = "]";
                    string tapones= "]";
                    string canastas="]";
                    string triples= "]";
                    string tirosLibres= "]";
                    string rebotes= "]";

                    string partidos2 = "]";
                    string puntos2 = "]";
                    string asistencias2 = "]";
                    string tapones2 = "]";
                    string canastas2 = "]";
                    string triples2 = "]";
                    string tirosLibres2 = "]";
                    string rebotes2 = "]";

                    Boolean salir1 = false;
                    Boolean salir2 = false;

                    string[] anios = new string[5];





                    using (var client = new HttpClient())
                    {
                        for (int i = 2022; salir1 == false && salir2==false; i--)
                        {
                            string url = "https://www.balldontlie.io/api/v1/season_averages?season=" + i + "&player_ids[]=" + id1 + "&player_ids[]=" + id2;

                            client.DefaultRequestHeaders.Clear();

                            var response = client.GetAsync(url).Result;

                            var res = response.Content.ReadAsStringAsync().Result;
                            dynamic r = JObject.Parse(res);
                            foreach (var datos in r.data)
                            {
                                if(datos.player_id == id1 && salir1==false)
                                {
                                    salir1 = true;
                                    ultAnio1 = i;
                                }
                                if(datos.player_id == id2 && salir2 == false)
                                {
                                    salir2= true;
                                    ultAnio1 = i;
                                }

                            }

                        }


                        for (int j = ultAnio1; j > (ultAnio1 - 5); j--)
                        {
                            Boolean entrado = false;
                            string url = "https://www.balldontlie.io/api/v1/season_averages?season=" + j + "&player_ids[]=" + id1;

                            client.DefaultRequestHeaders.Clear();

                            var response = client.GetAsync(url).Result;

                            var res = response.Content.ReadAsStringAsync().Result;
                            dynamic r = JObject.Parse(res);
                            foreach (var datos in r.data)
                            {
                                string ppartidos = datos.games_played.ToString().Replace(',', '.');
                                string ppuntos = datos.pts.ToString().Replace(',', '.');
                                string aasistenicas= datos.ast.ToString().Replace(',', '.');
                                string ttapones=datos.blk.ToString().Replace(",", ".");
                                string ccanastas= datos.fgm.ToString().Replace(",", ".");
                                string ttriples=datos.fg3m.ToString().Replace(",", ".");
                                string ttirosLibres = datos.ftm.ToString().Replace(",", ".");
                                string rrebotes= datos.reb.ToString().Replace(",", ".");

                                partidos = ppartidos + "," + partidos;
                                puntos = ppuntos + "," + puntos;
                                asistencias=aasistenicas + ","+asistencias;
                                tapones=ttapones+","+tapones;
                                canastas=ccanastas+","+canastas;
                                triples=ttriples+","+triples;
                                tirosLibres=ttirosLibres+","+tirosLibres;
                                rebotes=rrebotes+","+rebotes;
                                entrado = true;
                            }
                            if (entrado == false)
                            {
                                partidos = "0," + partidos;
                                puntos = "0," + puntos;
                                asistencias = "0," + asistencias;
                                tapones = "0," + tapones;
                                canastas = "0," + canastas;
                                triples = "0," + triples;
                                tirosLibres = "0," + tirosLibres;
                                rebotes = "0," + rebotes;
                            }
                        }
                        partidos = "[" + partidos;
                        puntos = "[" + puntos;
                        asistencias = "[" + asistencias;
                        tapones = "[" + tapones;
                        canastas = "[" + canastas;
                        triples = "[" + triples;
                        tirosLibres = "[" + tirosLibres;
                        rebotes="[" + rebotes;


                        for (int j = ultAnio2; j > (ultAnio2 - 5); j--)
                        {
                            Boolean entrado = false;
                            string url = "https://www.balldontlie.io/api/v1/season_averages?season=" + j + "&player_ids[]=" + id2;

                            client.DefaultRequestHeaders.Clear();

                            var response = client.GetAsync(url).Result;

                            var res = response.Content.ReadAsStringAsync().Result;
                            dynamic r = JObject.Parse(res);
                            foreach (var datos in r.data)
                            {
                                string ppartidos = datos.games_played.ToString().Replace(',', '.');
                                string ppuntos = datos.pts.ToString().Replace(',', '.');
                                string aasistenicas = datos.ast.ToString().Replace(',', '.');
                                string ttapones = datos.blk.ToString().Replace(",", ".");
                                string ccanastas = datos.fgm.ToString().Replace(",", ".");
                                string ttriples = datos.fg3m.ToString().Replace(",", ".");
                                string ttirosLibres = datos.ftm.ToString().Replace(",", ".");
                                string rrebotes = datos.reb.ToString().Replace(",", ".");

                                partidos2 = ppartidos + "," + partidos2;
                                puntos2 = ppuntos + "," + puntos2;
                                asistencias2 = aasistenicas + "," + asistencias2;
                                tapones2 = ttapones + "," + tapones2;
                                canastas2 = ccanastas + "," + canastas2;
                                triples2 = ttriples + "," + triples2;
                                tirosLibres2 = ttirosLibres + "," + tirosLibres2;
                                rebotes2 = rrebotes + "," + rebotes2;
                                entrado = true;
                            }
                            if (entrado == false)
                            {
                                partidos2 = "0," + partidos2;
                                puntos2 = "0," + puntos2;
                                asistencias2 = "0," + asistencias2;
                                tapones2 = "0," + tapones2;
                                canastas2 = "0," + canastas2;
                                triples2 = "0," + triples2;
                                tirosLibres2 = "0," + tirosLibres2;
                                rebotes2 = "0," + rebotes2;
                            }
                        }

                        partidos2 = "[" + partidos2;
                        puntos2 = "[" + puntos2;
                        asistencias2 = "[" + asistencias2;
                        tapones2 = "[" + tapones2;
                        canastas2 = "[" + canastas2;
                        triples2 = "[" + triples2;
                        tirosLibres2 = "[" + tirosLibres2;
                        rebotes2 = "[" + rebotes2;

                    }

                    using (var cliente = new HttpClient())
                    {
                        string url2 = "https://image-charts.com/chart.js/2.8.0?bkg=white&c=";
                        cliente.DefaultRequestHeaders.Clear();
                        string prametros = "{  \"type\": \"line\", \"data\": {   \"labels\": [\"5años \", \"4años\",   \"3años \",\"2años \", \"1año\"],    \"datasets\" : [      {     \"backgroundColor\": \"rgba(54, 162, 235)\",            \"borderColor\": \"rgb(255,150,150)\",       \"data\": " + partidos + ",        \"label\": \"Partidos jugados"+jugadore1.first_name+" \" ,          },{  \"backgroundColor\": \"rgba(54, 162, 235)\",       \"borderColor\": \"rgb(4, 162, 235, 0.5)\", \"data\": " + partidos2 + ", \"label\":\"Partidos Jugados"+jugadore2.first_name+"\"}    ]  }}";
                        dynamic jsonString = JObject.Parse(prametros);
                        var httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                        fotos.url = url2 + "" + jsonString;

                        cliente.DefaultRequestHeaders.Clear();
                         prametros = "{  \"type\": \"line\", \"data\": {   \"labels\": [\"5años \", \"4años\",   \"3años \",\"2años \", \"1año\"],    \"datasets\" : [      {     \"backgroundColor\": \"rgba(54, 162, 235)\",            \"borderColor\": \"rgb(255,150,150)\",       \"data\": " + puntos + ",        \"label\": \"Puntos por partido" + jugadore1.first_name + " \" ,          },{  \"backgroundColor\": \"rgba(54, 162, 235)\",       \"borderColor\": \"rgb(4, 162, 235, 0.5)\", \"data\": " + puntos2 + ", \"label\":\"Puntos por Partido" + jugadore2.first_name + "\"}    ]  }}";
                         jsonString = JObject.Parse(prametros);
                         httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                        fotos.url2 = url2 + "" + jsonString;

                        cliente.DefaultRequestHeaders.Clear();
                        prametros = "{  \"type\": \"line\", \"data\": {   \"labels\": [\"5años \", \"4años\",   \"3años \",\"2años \", \"1año\"],    \"datasets\" : [      {     \"backgroundColor\": \"rgba(54, 162, 235)\",            \"borderColor\": \"rgb(255,150,150)\",       \"data\": " + asistencias + ",        \"label\": \"Asistencias por partido" + jugadore1.first_name + " \" ,          },{  \"backgroundColor\": \"rgba(54, 162, 235)\",       \"borderColor\": \"rgb(4, 162, 235, 0.5)\", \"data\": " + asistencias2 + ", \"label\":\"Asistencias por Partido" + jugadore2.first_name + "\"}    ]  }}";
                        jsonString = JObject.Parse(prametros);
                        httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                        fotos.url3 = url2 + "" + jsonString;

                        cliente.DefaultRequestHeaders.Clear();
                        prametros = "{  \"type\": \"line\", \"data\": {   \"labels\": [\"5años \", \"4años\",   \"3años \",\"2años \", \"1año\"],    \"datasets\" : [      {     \"backgroundColor\": \"rgba(54, 162, 235)\",            \"borderColor\": \"rgb(255,150,150)\",       \"data\": " + tapones + ",        \"label\": \"Tapones por partido" + jugadore1.first_name + " \" ,          },{  \"backgroundColor\": \"rgba(54, 162, 235)\",       \"borderColor\": \"rgb(4, 162, 235, 0.5)\", \"data\": " + tapones2 + ", \"label\":\"Tapones por Partido" + jugadore2.first_name + "\"}    ]  }}";
                        jsonString = JObject.Parse(prametros);
                        httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                        fotos.url4 = url2 + "" + jsonString;

                        cliente.DefaultRequestHeaders.Clear();
                        prametros = "{  \"type\": \"line\", \"data\": {   \"labels\": [\"5años \", \"4años\",   \"3años \",\"2años \", \"1año\"],    \"datasets\" : [      {     \"backgroundColor\": \"rgba(54, 162, 235)\",            \"borderColor\": \"rgb(255,150,150)\",       \"data\": " + canastas + ",        \"label\": \"Canastas por partido" + jugadore1.first_name + " \" ,          },{  \"backgroundColor\": \"rgba(54, 162, 235)\",       \"borderColor\": \"rgb(4, 162, 235, 0.5)\", \"data\": " + canastas2 + ", \"label\":\"Canastas por Partido" + jugadore2.first_name + "\"}    ]  }}";
                        jsonString = JObject.Parse(prametros);
                        httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                        fotos.url5 = url2 + "" + jsonString;

                        cliente.DefaultRequestHeaders.Clear();
                        prametros = "{  \"type\": \"line\", \"data\": {   \"labels\": [\"5años \", \"4años\",   \"3años \",\"2años \", \"1año\"],    \"datasets\" : [      {     \"backgroundColor\": \"rgba(54, 162, 235)\",            \"borderColor\": \"rgb(255,150,150)\",       \"data\": " + triples + ",        \"label\": \"Triples por partido" + jugadore1.first_name + " \" ,          },{  \"backgroundColor\": \"rgba(54, 162, 235)\",       \"borderColor\": \"rgb(4, 162, 235, 0.5)\", \"data\": " + triples2 + ", \"label\":\"Triples por Partido" + jugadore2.first_name + "\"}    ]  }}";
                        jsonString = JObject.Parse(prametros);
                        httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                        fotos.url6 = url2 + "" + jsonString;

                        cliente.DefaultRequestHeaders.Clear();
                        prametros = "{  \"type\": \"line\", \"data\": {   \"labels\": [\"5años \", \"4años\",   \"3años \",\"2años \", \"1año\"],    \"datasets\" : [      {     \"backgroundColor\": \"rgba(54, 162, 235)\",            \"borderColor\": \"rgb(255,150,150)\",       \"data\": " + tirosLibres + ",        \"label\": \"Tiros libres por partido" + jugadore1.first_name + " \" ,          },{  \"backgroundColor\": \"rgba(54, 162, 235)\",       \"borderColor\": \"rgb(4, 162, 235, 0.5)\", \"data\": " + tirosLibres2 + ", \"label\":\"Tiros libres por Partido" + jugadore2.first_name + "\"}    ]  }}";
                        jsonString = JObject.Parse(prametros);
                        httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                        fotos.url7 = url2 + "" + jsonString;

                        cliente.DefaultRequestHeaders.Clear();
                        prametros = "{  \"type\": \"line\", \"data\": {   \"labels\": [\"5años \", \"4años\",   \"3años \",\"2años \", \"1año\"],    \"datasets\" : [      {     \"backgroundColor\": \"rgba(54, 162, 235)\",            \"borderColor\": \"rgb(255,150,150)\",       \"data\": " + rebotes + ",        \"label\": \"Rebotes por partido" + jugadore1.first_name + " \" ,          },{  \"backgroundColor\": \"rgba(54, 162, 235)\",       \"borderColor\": \"rgb(4, 162, 235, 0.5)\", \"data\": " + rebotes2 + ", \"label\":\"Rebotes por Partido" + jugadore2.first_name + "\"}    ]  }}";
                        jsonString = JObject.Parse(prametros);
                        httpContent = new StringContent(jsonString.ToString(), Encoding.UTF8);
                        fotos.url8 = url2 + "" + jsonString;
                    }
                    OpenControl(new stat_jugadores(fotos,jugadore1,jugadore2));
                }
  
                catch
                {
                    MessageBox.Show("Numero de llamadas a la API superado, vuelve a intentarlo en unos segundos");
                }
            }
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
            private void boton2_Click(object sender, RoutedEventArgs e)
        {
            string nombre2 = text_jugador2.Text;
            using (var client = new HttpClient())
            {
                jugador2.Items.Clear();
                string url = "https://www.balldontlie.io/api/v1/players?search=" + nombre2 + "&per_page=100";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;

                var res = response.Content.ReadAsStringAsync().Result;
                dynamic r = JObject.Parse(res);

                foreach (var datos in r.data)
                {
                    jugador2.Items.Add(datos.first_name + " " + datos.last_name);
                    string jid = datos.id;
                    string jfi = datos.first_name;
                    string jla = datos.last_name;
                    string jpo = datos.posicion;
                    Int32 jhef = 0;
                    Int32 jhei = 0;
                    Int32 jwe = 0;
                    try
                    {
                        jhef = datos.heigth_feet;
                        jhei = datos.heigth_inches;
                        jhef = datos.weigth_pounds;
                    }
                    catch
                    {

                    }

                    Jugador jugadorr = new Jugador(jid, jfi, jla, jpo, jhef, jhei, jwe, null);
                    jugadores2.Add(jugadorr);
                }
            }


        }

        private void jugador2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        
            if (jugador2.SelectedItem != null)
            {
                var nombreJugador = jugador2.SelectedItem.ToString();
                foreach (Jugador jugadorr in jugadores2)
                {
                    string nombre = jugadorr.first_name + " " + jugadorr.last_name;
                    if (nombreJugador == nombre)
                    {
                        jugadore2 = jugadorr;
                    }
                }
                abajo = true;
                datos();
            }
        }
    }
}
