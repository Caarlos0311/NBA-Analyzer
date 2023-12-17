using System;
using System.Collections.Generic;
using System.Linq;
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
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http;
using System.Net.Http.Headers;
using NBA_Analyzer.API;
using System.IO;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;
using NBA_Analyzer.API2;
//using MySql.Data.MySqlClient;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;
using Label = System.Windows.Controls.Label;
using NBA_Analyzer.clases;
using Button = System.Windows.Controls.Button;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace NBA_Analyzer
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<Team> lista_equipos=new List<Team>();
        public List<Jugador> lista_jugadores=new List<Jugador>();
        static HttpClient client = new HttpClient();

        public static Button b_ayuda_L;
        public static Ayuda wAyuda;

        //labels
        public static Label nombreUsuario;
        public static Label apellidosUsuario;
        public static Label ultimoInicio;

        public MainWindow(InicioSesion inicioSesion)
        {
            InitializeComponent();
            pedir_equipos();

            //label
            nombreUsuario = lbl_nombre_is;
            apellidosUsuario = lbl_apellidos_is;
            ultimoInicio = lbl_ultimoInicio_is;

            b_ayuda_L = btn_ayuda;
            wAyuda = new Ayuda(this);
            OpenControl(new equipos());
        }

        private void btn_ayuda_Click(object sender, RoutedEventArgs e)
        {
            Ayuda winAyu = new Ayuda(this);
            //win_Preferencias.aplicarCambios();
            winAyu.Show();
            btn_ayuda.IsEnabled = false;
            //Win_Control.b_ayuda_C.IsEnabled = false;
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

        private void pedir_equipos()
        {
            using (var client = new HttpClient())
            {
                string url = "https://www.balldontlie.io/api/v1/teams";

                client.DefaultRequestHeaders.Clear();

                var response = client.GetAsync(url).Result;

                var res = response.Content.ReadAsStringAsync().Result;
                dynamic r = JObject.Parse(res);
                foreach (var team in r.data)
                {
                    string eid = team.id;
                    string eabb = team.abbreviation;
                    string eci = team.city;
                    string eco = team.conference;
                    string edi = team.division;
                    string efu = team.full_name;
                    string ena = team.name;

                    Team equipo = new Team(eid, eabb, eci, eco, edi, efu, ena);
                    lista_equipos.Add(equipo);
                }


            }
        }


       

        private void Jugadores_Click(object sender, RoutedEventArgs e)
        {
            OpenControl(new equipos());
        }

        private void vs_Equipos_Click(object sender, RoutedEventArgs e)
        {
            OpenControl(new VS_equipos(lista_equipos));
        }

        private void vs_jugadores_Click(object sender, RoutedEventArgs e)
        {
            OpenControl(new VS_jugadores());
        }

        public static void asignarUSuario(Administrador a)
        {
            nombreUsuario.Content = a.Nombre;
            apellidosUsuario.Content = a.Apellidos;
            ultimoInicio.Content = a.UltimoAcceso.ToString();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
            MessageBox.Show("Hasta pronto.");
        }
    }
}
