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
    /// Lógica de negocio para la gestión de mecánicos del taller.
    /// </summary>
    public class MecanicosApi
    {
        private readonly MecanicosRepo repo = new MecanicosRepo();

        /// <summary>
        /// Obtiene todos los mecánicos registrados.
        /// </summary>
        public List<Mecanicos> ObtenerMecanicos()
        {
            try
            {
                return repo.GetAllMecanicos();
            }
            catch (Exception ex)
            {
                // Temporalmente, incluye el InnerException completo
                throw new Exception("Error al obtener mantenimientos: " + ex.ToString());
            }
        }

        /// <summary>
        /// Agrega un nuevo mecánico después de validar sus datos.
        /// </summary>
        /// <param name="m">Mecánico a agregar.</param>
        public void AgregarMecanico(Mecanicos m)
        {
            try
            {
                validarCampos(m);
                repo.AgregarMecanico(m);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza los datos de un mecánico existente.
        /// </summary>
        /// <param name="m">Mecánico con los datos actualizados.</param>
        public void ActualizarMecanico(Mecanicos m)
        {
            try
            {
                validarCampos(m);
                repo.EditarMecanico(m);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un mecánico por su identificador.
        /// </summary>
        /// <param name="id">Identificador del mecánico.</param>
        public void EliminarMecanico(int id)
        {
            try
            {
                repo.EliminarMecanico(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Valida que el nombre, apellidos y especialidad del mecánico son válidos.
        /// </summary>
        /// <param name="m">Mecánico a validar.</param>
        /// <exception cref="Exception">Se lanza si falta algún campo obligatorio.</exception>
        private void validarCampos(Mecanicos m)
        {
            if (string.IsNullOrWhiteSpace(m.nombre) ||
                string.IsNullOrWhiteSpace(m.apellidos) ||
                string.IsNullOrWhiteSpace(m.especialidad))
            {
                throw new Exception("El nombre, los apellidos y la especialidad del mecánico son obligatorios.");
            }
        }
    }
}
