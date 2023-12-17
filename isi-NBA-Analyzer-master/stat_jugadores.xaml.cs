using NBA_Analyzer.API;
using NBA_Analyzer.API2;
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

namespace NBA_Analyzer
{
    /// <summary>
    /// Lógica de interacción para stat_jugadores.xaml
    /// </summary>
    public partial class stat_jugadores : UserControl
    {
        public stat_jugadores(Fotos foto,Jugador jugador1,Jugador jugador2)
        {
            InitializeComponent();
            label_enfrentamiento.Content = jugador1.first_name + " " + jugador1.last_name + " VS " + jugador2.first_name + " " + jugador2.last_name;
            DataContext = foto;
        }
    }
}
