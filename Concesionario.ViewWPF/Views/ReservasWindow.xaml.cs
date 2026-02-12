using Concesionario.Controller.API;
using Concesionario.Model;
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
using System.Windows.Shapes;

namespace Concesionario.ViewWPF.Views
{
    /// <summary>
    /// Ventana de gestión de reservas de motos.
    /// Permite crear, modificar y eliminar reservas, así como asociarlas
    /// a clientes y motos disponibles.
    /// </summary>
    public partial class ReservasWindow : Window
    {
        /// <summary>
        /// API de negocio para la gestión de reservas.
        /// </summary>
        ReservasApi ctrl = new ReservasApi();

        /// <summary>
        /// Inicializa la ventana de reservas y carga los datos iniciales
        /// (clientes, motos disponibles y listado de reservas).
        /// </summary>
        public ReservasWindow()
        {
            InitializeComponent();
            limpiarCampos();
            cargarClientes();
            cargarMotos();
            RefreshGrid();
        }

        /// <summary>
        /// Cierra la ventana de reservas.
        /// </summary>
        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Limpia los campos del formulario de reservas.
        /// </summary>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
        }

        /// <summary>
        /// Guarda la reserva actual.
        /// Si hay una reserva seleccionada en el grid, actualiza;
        /// en caso contrario, crea una nueva reserva.
        /// </summary>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reservas r = new Reservas
                {
                    id_reserva = string.IsNullOrEmpty(txtIdReserva.Text) ? 0 : int.Parse(txtIdReserva.Text),
                    fecha_reserva = dpFechaReserva.SelectedDate ?? DateTime.Now,
                    fecha_limite = dpFechaLimite.SelectedDate ?? DateTime.Now.AddDays(15),
                    importe_senal = decimal.TryParse(txtSenal.Text, out decimal senal) ? senal : 0,
                    id_cliente = (int)(cmbCliente.SelectedValue ?? 0),
                    id_moto = (int)(cmbMoto.SelectedValue ?? 0)
                };

                if (dgReservas.SelectedItem is Reservas resSelec)
                {
                    r.id_reserva = resSelec.id_reserva;
                    ctrl.ActualizarReserva(r);
                    MessageBox.Show("Reserva actualizada correctamente.");
                }
                else
                {
                    ctrl.AgregarReserva(r);
                    MessageBox.Show("Reserva agregada correctamente.");
                }

                limpiarCampos();
                RefreshGrid();
                cargarMotos(); // actualizar motos disponibles
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Elimina la reserva seleccionada en el grid.
        /// </summary>
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgReservas.SelectedItem is Reservas resSelec)
            {
                try
                {
                    ctrl.EliminarReserva(resSelec.id_reserva);
                    MessageBox.Show("Reserva eliminada correctamente.");
                    limpiarCampos();
                    RefreshGrid();
                    cargarMotos(); // liberar moto
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Seleccione una reserva para eliminar.");
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            InformeReservas informe = new InformeReservas(); 
            informe.ShowDialog();
        }

        private void dgReservas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgReservas.SelectedItem is Reservas r)
            {
                txtIdReserva.Text = r.id_reserva.ToString();

                dpFechaReserva.SelectedDate = r.fecha_reserva;
                dpFechaLimite.SelectedDate = r.fecha_limite;

                txtSenal.Text = r.importe_senal.ToString();

                cmbCliente.SelectedValue = r.id_cliente;
                cmbMoto.SelectedValue = r.id_moto;
            }
        }

        private void limpiarCampos()
        {
            dgReservas.SelectedItem = null;
            txtIdReserva.Text = "";
            txtSenal.Text = "";
            dpFechaReserva.SelectedDate = DateTime.Now;
            dpFechaLimite.SelectedDate = DateTime.Now.AddDays(15); // Por defecto 15 días
            cmbCliente.SelectedIndex = -1;
            cmbMoto.SelectedIndex = -1;
        }

        /// <summary>
        /// Vuelve a cargar el grid de reservas desde la API.
        /// </summary>
        private void RefreshGrid()
        {
            try
            {
                dgReservas.ItemsSource = null;
                dgReservas.ItemsSource = ctrl.ObtenerReservas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar reservas: " + ex.Message);
            }
        }

        /// <summary>
        /// Carga en el combo las motos disponibles obtenidas desde la API de reservas.
        /// </summary>
        private void cargarMotos()
        {
            try
            {
                var lista = ctrl.ObtenerMotosDisponibles();
                cmbMoto.ItemsSource = null;
                cmbMoto.ItemsSource = lista;
                cmbMoto.DisplayMemberPath = "modelo";
                cmbMoto.SelectedValuePath = "id_moto";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar motos: " + ex.Message);
            }
        }

        /// <summary>
        /// Carga en el combo la lista de clientes desde la base de datos.
        /// </summary>
        private void cargarClientes()
        {
            using (var db = new ConcesionarioBDEntities1())
            {
                var lista = db.Clientes.ToList();
                cmbCliente.ItemsSource = null;
                cmbCliente.ItemsSource = lista;
                cmbCliente.DisplayMemberPath = "nombre";
                cmbCliente.SelectedValuePath = "id_cliente";
            }
        }
    }
}
