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
    /// Ventana de gestión de clientes del concesionario.
    /// Permite realizar operaciones CRUD sobre la entidad <see cref="Clientes"/>.
    /// </summary>
    public partial class ClientesWindow : Window
    {
        /// <summary>
        /// API de negocio para la gestión de clientes.
        /// </summary>
        Controller.API.ClienteApi ctlr = new Controller.API.ClienteApi();

        /// <summary>
        /// Inicializa la ventana y carga el listado inicial de clientes.
        /// </summary>
        public ClientesWindow()
        {
            InitializeComponent();
            limpiarCampos();
            RefreshGrid();
        }

        /// <summary>
        /// Cierra la ventana actual y vuelve a la pantalla anterior.
        /// </summary>
        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Limpia los campos de edición para crear o editar un cliente.
        /// </summary>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiarCampos();
        }

        /// <summary>
        /// Guarda los datos del cliente actual.
        /// Si hay un cliente seleccionado en el grid, actualiza;
        /// en caso contrario, crea un nuevo cliente.
        /// </summary>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clientes c = new Clientes
                {
                    // Si el campo ID está vacío, se asume que es un nuevo cliente (id_cliente = 0)
                    id_cliente = string.IsNullOrEmpty(txtIdCliente.Text) ? 0 : int.Parse(txtIdCliente.Text),
                    dni = txtDni.Text,
                    nombre = txtNombre.Text,
                    apellidos = txtApellidos.Text,
                    telefono = txtTelefono.Text,
                    email = txtEmail.Text
                };

                if (dgClientes.SelectedItem is Clientes clieSelec)
                {
                    c.id_cliente = clieSelec.id_cliente;
                    ctlr.ActualizarCliente(c);
                    MessageBox.Show("Cliente actualizado correctamente.");
                }
                else
                {
                    ctlr.AgregarCliente(c);
                    MessageBox.Show("Cliente agregado correctamente.");
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
        /// Elimina el cliente seleccionado previa confirmación.
        /// </summary>
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgClientes.SelectedItem is Clientes c)
                {
                    var result = MessageBox.Show($"¿Confirma que desea eliminar al cliente {c.nombre} {c.apellidos}?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        ctlr.EliminarCliente(c.id_cliente);
                        MessageBox.Show("Cliente eliminado correctamente.");
                        RefreshGrid();
                        limpiarCampos();
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un cliente para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            InformeClientesWindow informeWindow = new InformeClientesWindow();
            informeWindow.ShowDialog();
        }

        private void dgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Cargar datos al seleccionar fila
            if (dgClientes.SelectedItem is Clientes c)
            {
                txtIdCliente.Text = c.id_cliente.ToString();
                txtDni.Text = c.dni;
                txtNombre.Text = c.nombre;
                txtApellidos.Text = c.apellidos;
                txtTelefono.Text = c.telefono;
                txtEmail.Text = c.email;
            }
        }

        /// <summary>
        /// Limpia la selección del grid y los campos de texto.
        /// </summary>
        private void limpiarCampos()
        {
            // Limpiar para crear nuevo
            dgClientes.SelectedItem = null;
            txtIdCliente.Text = "";
            txtDni.Text = "";
            txtNombre.Text = "";
            txtApellidos.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
        }

        /// <summary>
        /// Recarga el grid de clientes desde la capa de negocio.
        /// </summary>
        private void RefreshGrid()
        {
            // Refrescar datagrid
            dgClientes.ItemsSource = null;
            dgClientes.ItemsSource = ctlr.ObtenerClientes();
        }
    }
}
