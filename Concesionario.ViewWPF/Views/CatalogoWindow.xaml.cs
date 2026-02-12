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

namespace Concesionario.ViewWPF
{
    /// <summary>
    /// Ventana WPF para la gestión del catálogo de motos del concesionario.
    /// Permite crear, editar, eliminar y visualizar modelos de motos.
    /// </summary>
    public partial class CatalogoWindow : Window
    {
        /// <summary>
        /// Controlador de la lógica de negocio para operaciones sobre modelos de motos.
        /// </summary>
        private readonly ModeloApi ctlr = new Controller.API.ModeloApi();

        /// <summary>
        /// Inicializa una nueva instancia de la ventana <see cref="CatalogoWindow"/>.
        /// Carga el catálogo de motos y limpia los campos del formulario.
        /// </summary>
        public CatalogoWindow()
        {
            InitializeComponent();

            try
            {
                RefreshGrid();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el catálogo: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Maneja el evento de clic del botón Volver.
        /// Cierra la ventana actual y regresa a la ventana anterior.
        /// </summary>
        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            // Cerrar esta ventana de stock
            this.Close();
        }

        /// <summary>
        /// Maneja el evento de clic del botón Guardar.
        /// Crea o actualiza una moto en base a los datos introducidos en el formulario.
        /// Si hay una moto seleccionada en el grid, se actualiza; en caso contrario, se crea una nueva.
        /// </summary>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Motos moto = new Motos
                {
                    marca = txtMarca.Text,
                    modelo = txtModelo.Text,
                    matricula = txtMatricula.Text,
                    anio = int.Parse(txtAnio.Text),
                    kilometros = int.Parse(txtKms.Text),
                    precio = decimal.Parse(txtPrecio.Text)
                };

                if (dgStock.SelectedItem is Motos motoSeleccionada)
                {
                    // Si hay una moto seleccionada, actualizamos
                    moto.id_moto = motoSeleccionada.id_moto;
                    ctlr.ActualizarModelo(moto);
                    MessageBox.Show("Moto actualizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Si no hay una moto seleccionada, agregamos una nueva
                    ctlr.AgregarModelo(moto);
                    MessageBox.Show("Moto agregada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                RefreshGrid();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la moto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Maneja el evento de clic del botón Eliminar.
        /// Elimina la moto seleccionada actualmente en el grid de stock.
        /// </summary>
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgStock.SelectedItem is Motos motoSeleccionada)
            {
                try
                {
                    ctlr.EliminarModelo(motoSeleccionada.id_moto);
                    MessageBox.Show("Moto eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshGrid();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar la moto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione una moto para eliminar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Maneja el evento de clic del botón Limpiar.
        /// Limpia todos los campos del formulario y deselecciona cualquier fila del grid.
        /// </summary>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        /// <summary>
        /// Recarga los datos del catálogo de motos desde la capa de negocio
        /// y los asigna como origen de datos del grid de stock.
        /// </summary>
        private void RefreshGrid()
        {
            var list = ctlr.ObtenerModelos();

            dgStock.ItemsSource = null;
            dgStock.ItemsSource = list;
            dgStock.SelectedIndex = 0;
        }

        /// <summary>
        /// Limpia todos los campos de entrada del formulario y deselecciona
        /// cualquier moto seleccionada en el grid.
        /// </summary>
        private void LimpiarCampos()
        {
            dgStock.SelectedItem = null;
            txtIdMoto.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtMatricula.Text = "";
            txtAnio.Text = "";
            txtKms.Text = "";
            txtPrecio.Text = "";
        }

        /// <summary>
        /// Maneja el evento de cambio de selección del grid de stock.
        /// Cuando se selecciona una moto, sus datos se cargan en los campos del formulario.
        /// </summary>
        private void dgStock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStock.SelectedItem is Motos motoSeleccionada)
            {
                txtIdMoto.Text = motoSeleccionada.id_moto.ToString();
                txtMarca.Text = motoSeleccionada.marca;
                txtModelo.Text = motoSeleccionada.modelo;
                txtMatricula.Text = motoSeleccionada.matricula;
                txtAnio.Text = motoSeleccionada.anio.ToString();
                txtKms.Text = motoSeleccionada.kilometros.ToString();
                txtPrecio.Text = motoSeleccionada.precio.ToString();
            }
        }
    }
}
