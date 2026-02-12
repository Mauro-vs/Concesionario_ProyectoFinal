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
    /// Lógica de interacción para MecanicosWindow.xaml
    /// </summary>
    public partial class MecanicosWindow : Window
    {
        /// <summary>
        /// Controlador de lógica de negocio para mecánicos.
        /// </summary>
        Controller.API.MecanicosApi ctlr = new Controller.API.MecanicosApi();

        /// <summary>
        /// Inicializa una nueva instancia de la ventana <see cref="MecanicosWindow"/>.
        /// </summary>
        public MecanicosWindow()
        {
            InitializeComponent();
            limpiarCampos();
            RefreshGrid();
        }

        /// <summary>
        /// Cierra la ventana actual.
        /// </summary>
        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   
        }

        /// <summary>
        /// Limpia los campos del formulario.
        /// </summary>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
        }

        /// <summary>
        /// Guarda o actualiza un mecánico según el contexto de selección.
        /// </summary>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mecanicos m = new Mecanicos
                {
                    id_mecanico = string.IsNullOrEmpty(txtIdMecanico.Text) ? 0 : int.Parse(txtIdMecanico.Text),
                    nombre = txtNombre.Text,
                    apellidos = txtApellidos.Text,
                    especialidad = txtEspecialidad.Text
                };

                if (dgMecanicos.SelectedItem is Mecanicos mecSelec)
                {
                    m.id_mecanico = mecSelec.id_mecanico;
                    ctlr.ActualizarMecanico(m);
                    MessageBox.Show("Mecánico actualizado correctamente.");
                }
                else
                {
                    ctlr.AgregarMecanico(m);
                    MessageBox.Show("Mecánico agregado correctamente.");
                }

                RefreshGrid();
                limpiarCampos();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Elimina el mecánico seleccionado en la cuadrícula.
        /// </summary>
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgMecanicos.SelectedItem is Mecanicos m)
                {
                    ctlr.EliminarMecanico(m.id_mecanico);
                    MessageBox.Show("Mecánico eliminado correctamente.");
                    RefreshGrid();
                    limpiarCampos();
                }
                else
                {
                    MessageBox.Show("Seleccione un mecánico para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Maneja el cambio de selección en la cuadrícula de mecánicos.
        /// </summary>
        private void dgMecanicos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMecanicos.SelectedItem is Mecanicos m) 
            {
                txtIdMecanico.Text = m.id_mecanico.ToString();
                txtNombre.Text = m.nombre;
                txtApellidos.Text = m.apellidos;
                txtEspecialidad.Text = m.especialidad;
            }
        }

        /// <summary>
        /// Limpia los campos de entrada del formulario.
        /// </summary>
        public void limpiarCampos()
        {
            dgMecanicos.SelectedItem = null;
            txtIdMecanico.Text = "";
            txtNombre.Text = "";
            txtApellidos.Text = "";
            txtEspecialidad.Text = "";
        }

        /// <summary>
        /// Recarga la lista de mecánicos en la cuadrícula.
        /// </summary>
        public void RefreshGrid()
        {
            dgMecanicos.ItemsSource = null;
            dgMecanicos.ItemsSource = ctlr.ObtenerMecanicos();
        }
    }
}
