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
    /// Lógica de interacción para statEquipos.xaml
    /// </summary>
    public partial class statEquipos : UserControl
    {
        public statEquipos(Fotos fotos,Team equipo1, Team equipo2)
        {
            InitializeComponent();
            label_equipo1.Content= equipo1.full_name;
            label_equipo2.Content = equipo2.full_name;
            DataContext = fotos;
        }
    }
}
