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
    /// API de gestión de reservas.
    /// Encapsula la lógica de negocio relacionada con las reservas y
    /// delega el acceso a datos en <see cref="ReservasRepo"/>.
    /// </summary>
    public class ReservasApi
    {
        /// <summary>
        /// Repositorio de acceso a datos de reservas.
        /// </summary>
        ReservasRepo repo = new ReservasRepo();

        /// <summary>
        /// Obtiene todas las reservas registradas.
        /// </summary>
        /// <returns>Lista de objetos <see cref="Reservas"/>.</returns>
        public List<Reservas> ObtenerReservas()
        {
            try
            {
                return repo.GetAllReservas();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener reservas: " + ex.ToString());
            }
        }

        /// <summary>
        /// Obtiene la lista de motos disponibles (no reservadas).
        /// </summary>
        /// <returns>Lista de objetos <see cref="Motos"/> disponibles para reservar.</returns>
        public List<Motos> ObtenerMotosDisponibles()
        {
            try
            {
                return repo.GetMotosDisponibles();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener motos disponibles: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Agrega una nueva reserva.
        /// </summary>
        /// <param name="r">Objeto <see cref="Reservas"/> con los datos de la reserva a agregar.</param>
        public void AgregarReserva(Reservas r)
        {
            try
            {
                validarCampos(r);
                repo.AgregarReserva(r);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza una reserva existente.
        /// </summary>
        /// <param name="r">Objeto <see cref="Reservas"/> con los datos actualizados.</param>
        public void ActualizarReserva(Reservas r)
        {
            try
            {
                validarCampos(r);
                repo.EditarReserva(r);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina una reserva por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la reserva a eliminar.</param>
        public void EliminarReserva(int id)
        {
            try
            {
                repo.EliminarReserva(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Valida los campos obligatorios de una reserva.
        /// </summary>
        /// <param name="r">Objeto <see cref="Reservas"/> a validar.</param>
        /// <exception cref="Exception">
        /// Se lanza cuando falta algún campo obligatorio o no es válido.
        /// </exception>
        private void validarCampos(Reservas r)
        {
            if (r.fecha_reserva == null
                || r.fecha_limite == null
                || r.importe_senal <= 0
                || r.id_cliente <= 0
                || r.id_moto <= 0)
            {
                throw new Exception("Todos los campos son obligatorios y deben ser válidos.");
            }

            validarFechas(r);
        }

        /// <summary>
        /// Valida las fechas de una reserva.
        /// </summary>
        /// <param name="r">Objeto <see cref="Reservas"/> cuyas fechas se van a validar.</param>
        /// <exception cref="Exception">
        /// Se lanza cuando la fecha de reserva es anterior a la fecha actual.
        /// </exception>
        private void validarFechas(Reservas r)
        {
            if (r.fecha_reserva < DateTime.Now.Date)
            {
                throw new Exception("La fecha de reserva no puede ser anterior a la fecha actual.");
            }
        }
    }
}
