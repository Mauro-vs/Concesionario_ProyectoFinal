using Concesionario.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Controller.API
{
    /// <summary>
    /// Servicio de lógica de negocio para gestionar los mantenimientos del taller.
    /// Encapsula las validaciones de negocio y usa <see cref="TallerRepo"/> para el acceso a datos.
    /// </summary>
    public class TallerApi
    {
        private readonly TallerRepo tallerRepo = new TallerRepo();

        /// <summary>
        /// Obtiene todos los mantenimientos registrados en el taller.
        /// </summary>
        /// <returns>Lista de entidades <see cref="Mantenimientos"/> con sus motos y mecánicos relacionados.</returns>
        /// <exception cref="Exception">Se produce si ocurre un error al acceder a la base de datos.</exception>
        public List<Mantenimientos> ObtenerMantenimientos()
        {
            try
            {
                return tallerRepo.GetAllMantenimientos();
            }
            catch (Exception ex)
            {
                // Temporalmente, incluye el InnerException completo
                throw new Exception("Error al obtener mantenimientos: " + ex.ToString());
            }
        }

        /// <summary>
        /// Agrega un nuevo mantenimiento al taller.
        /// </summary>
        /// <param name="m">Mantenimiento a agregar.</param>
        /// <exception cref="Exception">Se produce si los datos no son válidos o al guardar en la base de datos.</exception>
        public void AgregarMantenimiento(Mantenimientos m)
        {
            try
            {
                validarCampos(m);
                tallerRepo.AgregarMantenimiento(m);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un mantenimiento existente.
        /// </summary>
        /// <param name="m">Mantenimiento con los datos actualizados.</param>
        public void ActualizarMantenimiento(Mantenimientos m)
        {
            try
            {
                validarCampos(m);
                tallerRepo.EditarMantenimiento(m);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un mantenimiento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del mantenimiento a eliminar.</param>
        public void EliminarMantenimiento(int id)
        {
            try
            {
                tallerRepo.EliminarMantenimiento(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Valida las reglas de negocio de un mantenimiento.
        /// </summary>
        /// <param name="m">Mantenimiento a validar.</param>
        /// <exception cref="Exception">
        /// Se produce si falta la moto o el mecánico, el motivo está vacío,
        /// el coste es negativo o las fechas son inconsistentes.
        /// </exception>
        public void validarCampos(Mantenimientos m)
        {
            if (m.id_moto <= 0)
            {
                throw new Exception("Debe seleccionar una moto válida.");
            }

            if (m.id_mecanico <= 0)
            {
                throw new Exception("Debe seleccionar un mecánico válido.");
            }
            if (string.IsNullOrWhiteSpace(m.motivo_cliente))
            {
                throw new Exception("El motivo del cliente no puede estar vacío.");
            }
            if (m.costo < 0)
            {
                throw new Exception("El costo no puede ser negativo.");
            }

            if (m.fecha_recepcion.HasValue && m.fecha_entrega.HasValue)
            {
                if (m.fecha_entrega < m.fecha_recepcion)
                {
                    throw new Exception("La fecha de entrega no puede ser anterior a la fecha de recepción.");
                }
            }
        }
    }
}
