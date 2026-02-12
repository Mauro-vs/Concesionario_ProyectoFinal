using Concesionario.Model;
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
using System.Windows.Shapes;

namespace Concesionario.ViewWPF
{
    /// <summary>
    /// Lógica de interacción para TallerWindow.xaml
    /// </summary>
    /// <summary>
    /// Ventana principal para la gestión de mantenimientos del taller.
    /// Permite crear, editar, eliminar e imprimir los mantenimientos.
    /// </summary>
    public partial class TallerWindow : Window
    {
        /// <summary>
        /// API de lógica de negocio para gestionar los mantenimientos del taller.
        /// </summary>
        Controller.API.TallerApi ctlr = new Controller.API.TallerApi();

        /// <summary>
        /// Inicializa una nueva instancia de la ventana del taller
        /// y carga los datos iniciales (mecánicos, motos y mantenimientos).
        /// </summary>
        public TallerWindow()
        {
            InitializeComponent();
            CargarMecanicos();
            CargarMotos();
            RefreshGrid();
            limpiarCampos();
        }

        /// <summary>
        /// Carga en el combo de mecánicos todos los mecánicos disponibles en la base de datos.
        /// </summary>
        private void CargarMecanicos()
        {
            using (var db = new ConcesionarioBDEntities1())
            {
                var lista = db.Mecanicos.ToList();
                cmbMecanico.ItemsSource = null;
                cmbMecanico.ItemsSource = lista;
                cmbMecanico.DisplayMemberPath = "nombre";
                cmbMecanico.SelectedValuePath = "id_mecanico";
            }
        }

        /// <summary>
        /// Carga en el combo de motos las motos con estado &quot;Disponible&quot; o &quot;Taller&quot;.
        /// </summary>
        private void CargarMotos()
        {
            using (var db = new ConcesionarioBDEntities1())
            {
                var lista = db.Motos
                              .Where(m => m.estado == "Disponible" || m.estado == "Taller")
                              .ToList();

                cmbMoto.ItemsSource = null;
                cmbMoto.ItemsSource = lista;
                cmbMoto.DisplayMemberPath = "matricula";
                cmbMoto.SelectedValuePath = "id_moto";
            }
        }

        /// <summary>
        /// Cierra la ventana actual y vuelve a la pantalla anterior.
        /// </summary>
        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Limpia todos los campos del formulario de mantenimiento.
        /// </summary>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
        }

        /// <summary>
        /// Guarda el mantenimiento actual.
        /// Si hay un mantenimiento seleccionado en la rejilla, lo actualiza;
        /// en caso contrario, crea uno nuevo.
        /// </summary>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idMoto = (int)(cmbMoto.SelectedValue ?? 0);

                Mantenimientos m = new Mantenimientos
                {
                    id_moto = idMoto,
                    id_mecanico = cmbMecanico.SelectedItem != null ? ((Mecanicos)cmbMecanico.SelectedItem).id_mecanico : 0,
                    motivo_cliente = txtMotivoCliente.Text,
                    trabajo_realizado = txtTrabajo.Text,
                    fecha_recepcion = dpFechaRecepcion.SelectedDate,
                    fecha_entrega = dpFechaEntrega.SelectedDate,
                    costo = decimal.TryParse(txtCosto.Text, out decimal costo) ? (decimal?)costo : null
                };

                if (dgTaller.SelectedItem is Mantenimientos mSelec)
                {
                    m.id_mantenimiento = mSelec.id_mantenimiento;
                    ctlr.ActualizarMantenimiento(m);
                    MessageBox.Show("Mantenimiento actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ctlr.AgregarMantenimiento(m);
                    MessageBox.Show("Mantenimiento agregado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                RefreshGrid();
                CargarMotos();
                limpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el mantenimiento: " + ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Elimina el mantenimiento seleccionado en la rejilla
        /// tras confirmar la operación con el usuario.
        /// </summary>
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgTaller.SelectedItem is Mantenimientos m)
            {
                var result = MessageBox.Show("¿Estás seguro de que deseas eliminar este mantenimiento?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        ctlr.EliminarMantenimiento(m.id_mantenimiento);
                        MessageBox.Show("Mantenimiento eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        RefreshGrid();
                        CargarMotos();
                        limpiarCampos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el mantenimiento: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un mantenimiento para eliminar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Abre la ventana de informe de taller para imprimir o visualizar los mantenimientos.
        /// </summary>
        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            InformeTallerWindow informeWindow = new InformeTallerWindow();
            informeWindow.ShowDialog();
        }

        /// <summary>
        /// Rellena los campos del formulario con los datos del mantenimiento
        /// seleccionado en la rejilla.
        /// </summary>
        private void dgTaller_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTaller.SelectedItem is Mantenimientos m)
            {
                txtIdMantenimiento.Text = m.id_mantenimiento.ToString();
                cmbMoto.SelectedValue = m.id_moto;
                cmbMecanico.SelectedValue = m.id_mecanico;

                txtMotivoCliente.Text = m.motivo_cliente;
                txtTrabajo.Text = m.trabajo_realizado;
                txtCosto.Text = m.costo.ToString();

                dpFechaRecepcion.SelectedDate = m.fecha_recepcion;
                dpFechaEntrega.SelectedDate = m.fecha_entrega;
            }
        }

        /// <summary>
        /// Restablece el formulario a su estado inicial y deselecciona la rejilla.
        /// </summary>
        public void limpiarCampos()
        {
            dgTaller.SelectedItem = null;
            txtIdMantenimiento.Text = "";
            cmbMoto.SelectedIndex = -1;
            cmbMecanico.SelectedIndex = -1;
            txtMotivoCliente.Text = "";
            txtTrabajo.Text = "";
            txtCosto.Text = "";
            dpFechaRecepcion.SelectedDate = DateTime.Now;
            dpFechaEntrega.SelectedDate = null;
        }

        /// <summary>
        /// Recarga los datos de mantenimientos en la rejilla desde la API.
        /// </summary>
        public void RefreshGrid()
        {
            dgTaller.ItemsSource = null;
            dgTaller.ItemsSource = ctlr.ObtenerMantenimientos();
        }

        private void cmbMoto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
