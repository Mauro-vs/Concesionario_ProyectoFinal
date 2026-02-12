using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity; // Agregado para usar Include

namespace Concesionario.Model.Repo
{
    /// <summary>
    /// Repositorio para la gestión de reservas y el estado asociado de las motos.
    /// </summary>
    public class ReservasRepo
    {
        private readonly ConcesionarioBDEntities1 db = new ConcesionarioBDEntities1();

        /// <summary>
        /// Obtiene todas las reservas incluyendo la información relacionada
        /// de clientes y motos mediante carga ansiosa (<c>Include</c>).
        /// </summary>
        /// <returns>Lista con todas las reservas.</returns>
        public List<Reservas> GetAllReservas()
        {
            return db.Reservas
                     .Include(r => r.Clientes)
                     .Include(r => r.Motos)
                     .ToList();
        }

        /// <summary>
        /// Obtiene la lista de motos disponibles, es decir,
        /// aquellas cuyo campo <c>estado</c> es &quot;Disponible&quot;.
        /// </summary>
        /// <returns>Lista de motos disponibles para reservar o vender.</returns>
        public List<Motos> GetMotosDisponibles()
        {
            return db.Motos
                     .Where(m => m.estado == "Disponible")
                     .ToList();
        }

        /// <summary>
        /// Agrega una nueva reserva y marca la moto asociada como &quot;Reservada&quot;.
        /// </summary>
        /// <param name="r">Entidad <see cref="Reservas"/> a agregar.</param>
        public void AgregarReserva(Reservas r)
        {
            db.Reservas.Add(r);

            // Marca la moto como reservada
            var moto = db.Motos.Find(r.id_moto);
            if (moto != null)
                moto.estado = "Reservada";

            db.SaveChanges();
        }

        /// <summary>
        /// Edita una reserva existente. Si cambia la moto asociada,
        /// libera la moto anterior (estado &quot;Disponible&quot;) y marca la nueva como &quot;Reservada&quot;.
        /// </summary>
        /// <param name="r">Entidad <see cref="Reservas"/> con los nuevos datos.</param>
        public void EditarReserva(Reservas r)
        {
            var existing = db.Reservas.Find(r.id_reserva);
            if (existing != null)
            {
                // Si cambia la moto, libera la anterior y reserva la nueva
                if (existing.id_moto != r.id_moto)
                {
                    var motoAnterior = db.Motos.Find(existing.id_moto);
                    if (motoAnterior != null)
                        motoAnterior.estado = "Disponible";

                    var motoNueva = db.Motos.Find(r.id_moto);
                    if (motoNueva != null)
                        motoNueva.estado = "Reservada";
                }

                existing.fecha_reserva = r.fecha_reserva;
                existing.fecha_limite = r.fecha_limite;
                existing.importe_senal = r.importe_senal;
                existing.id_cliente = r.id_cliente;
                existing.id_moto = r.id_moto;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina una reserva por identificador y vuelve a marcar
        /// la moto asociada como &quot;Disponible&quot;.
        /// </summary>
        /// <param name="id">Identificador de la reserva a eliminar.</param>
        public void EliminarReserva(int id)
        {
            var r = db.Reservas.Find(id);
            if (r != null)
            {
                // Al eliminar, liberar la moto
                var moto = db.Motos.Find(r.id_moto);
                if (moto != null)
                    moto.estado = "Disponible";

                db.Reservas.Remove(r);
                db.SaveChanges();
            }
        }
    }
}
