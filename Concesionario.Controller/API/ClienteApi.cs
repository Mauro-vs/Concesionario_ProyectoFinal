using Concesionario.Model;
using Concesionario.Model.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Controller.API
{
    /// <summary>
    /// Proporciona operaciones de gestión y validación para entidades de tipo <see cref="Clientes"/>.
    /// </summary>
    public class ClienteApi
    {
        private readonly ClientesRepo repo = new ClientesRepo();

        /// <summary>
        /// Obtiene la lista completa de clientes desde el repositorio.
        /// </summary>
        /// <returns>Lista de objetos <see cref="Clientes"/>.</returns>
        public List<Clientes> ObtenerClientes()
        {
            try
            {
                return repo.GetAllClientes();
            }
            catch (Exception ex)
            {
                // Temporalmente, incluye el InnerException completo
                throw new Exception("Error al obtener mantenimientos: " + ex.ToString());
            }
        }

        /// <summary>
        /// Agrega un nuevo cliente después de validar sus datos.
        /// </summary>
        /// <param name="c">Instancia de <see cref="Clientes"/> con los datos del nuevo cliente.</param>
        public void AgregarCliente(Clientes c)
        {
            try
            {
                validarCampos(c);
                repo.AgregarCliente(c);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente después de validar sus datos.
        /// </summary>
        /// <param name="c">Instancia de <see cref="Clientes"/> con los datos actualizados.</param>
        public void ActualizarCliente(Clientes c)
        {
            try
            {
                validarCampos(c);
                repo.EditarCliente(c);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un cliente por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del cliente a eliminar.</param>
        public void EliminarCliente(int id)
        {
            try
            {
                repo.EliminarCliente(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Valida los campos obligatorios y el formato de los datos de un cliente.
        /// </summary>
        /// <param name="c">Instancia de <see cref="Clientes"/> a validar.</param>
        /// <exception cref="Exception">
        /// Se produce si algún campo obligatorio está vacío,
        /// si el teléfono no tiene 9 dígitos,
        /// si el email no es válido
        /// o si el DNI es inválido o está duplicado.
        /// </exception>
        public void validarCampos(Clientes c)
        {
            if (string.IsNullOrWhiteSpace(c.dni)
                || string.IsNullOrWhiteSpace(c.nombre)
                || string.IsNullOrWhiteSpace(c.apellidos)
                || string.IsNullOrWhiteSpace(c.telefono)
                || string.IsNullOrWhiteSpace(c.email))
            {
                throw new Exception("Los campos son obligatorios.");
            }

            var patronTelefono = @"^\d{9}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(c.telefono, patronTelefono))
            {
                throw new Exception("El teléfono debe tener 9 dígitos.");
            }

            ValidarEmail(c.email);
            ValidarDNI(c.dni, c.id_cliente);
        }

        /// <summary>
        /// Valida que una dirección de correo tenga un formato correcto.
        /// </summary>
        /// <param name="email">Dirección de correo electrónico a validar.</param>
        /// <exception cref="Exception">
        /// Se produce si el email no cumple el formato esperado.
        /// </exception>
        public void ValidarEmail(string email)
        {
            var patronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, patronEmail))
            {
                throw new Exception("El email no tiene un formato válido.");
            }
        }

        /// <summary>
        /// Valida el formato del DNI y comprueba que sea único en la base de datos.
        /// </summary>
        /// <param name="dni">DNI a validar.</param>
        /// <param name="id_cliente">
        /// Identificador del cliente actual (opcional). Se usa para excluirlo
        /// al comprobar duplicados en actualizaciones.
        /// </param>
        /// <exception cref="Exception">
        /// Se produce si el DNI no tiene un formato válido
        /// o si ya existe asociado a otro cliente.
        /// </exception>
        public void ValidarDNI(string dni, int? id_cliente = null)
        {
            var patronDNI = @"^\d{8}[A-Za-z]$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(dni, patronDNI))
            {
                throw new Exception("El DNI no tiene un formato válido.");
            }
            var clientes = repo.GetAllClientes();
            if (clientes.Any(c => c.dni == dni && c.id_cliente != id_cliente))
            {
                throw new Exception("El DNI ya existe para otro cliente.");
            }
        }
    }
}
