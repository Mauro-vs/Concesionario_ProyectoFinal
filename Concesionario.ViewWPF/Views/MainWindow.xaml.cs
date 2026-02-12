using Concesionario.ViewWPF.Views;
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

namespace Concesionario.ViewWPF
{
    /// <summary>
    /// Ventana principal de la aplicación.
    /// Actúa como menú de navegación hacia las distintas áreas del concesionario.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Inicializa la ventana principal.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Abre la ventana de gestión del catálogo de motos.
        /// </summary>
        private void btnCatalogo_Click(object sender, RoutedEventArgs e)
        {
            var catalogoWindow = new CatalogoWindow();
            catalogoWindow.Show();
        }

        /// <summary>
        /// Abre la ventana de gestión del taller (mantenimientos).
        /// </summary>
        private void btnTaller_Click(object sender, RoutedEventArgs e)
        {
            var tallerWindow = new TallerWindow();
            tallerWindow.Show();
        }

        /// <summary>
        /// Abre la ventana de gestión de clientes.
        /// </summary>
        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            var clientesWindow = new ClientesWindow();
            clientesWindow.Show();
        }

        /// <summary>
        /// Abre la ventana de gestión de mecánicos.
        /// </summary>
        private void btnMecanico_Click(object sender, RoutedEventArgs e)
        {
            var mecanicoWindow = new MecanicosWindow();
            mecanicoWindow.Show();
        }

        /// <summary>
        /// Abre la ventana de gestión de reservas.
        /// </summary>
        private void btnReservas_Click(object sender, RoutedEventArgs e)
        {
            var reservasWindow = new ReservasWindow();
            reservasWindow.Show();
        }
    }
}
