using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Model
{
    /// <summary>
    /// Repositorio de acceso a datos para los mantenimientos del taller.
    /// Usa Entity Framework sobre <see cref="ConcesionarioBDEntities1"/>.
    /// </summary>
    public class TallerRepo
    {
        private readonly ConcesionarioBDEntities1 db = new ConcesionarioBDEntities1();

        /// <summary>
        /// Devuelve todos los mantenimientos, incluyendo datos de motos y mecánicos.
        /// </summary>
        public List<Mantenimientos> GetAllMantenimientos()
        {
            return db.Mantenimientos
                     .Include(m => m.Motos)
                     .Include(m => m.Mecanicos)
                     .ToList();
        }

        /// <summary>
        /// Busca un mantenimiento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del mantenimiento.</param>
        public Mantenimientos GetMantenimientoById(int id)
        {
            return db.Mantenimientos.Find(id);
        }

        /// <summary>
        /// Inserta un nuevo mantenimiento y marca la moto como "Taller".
        /// </summary>
        /// <param name="m">Mantenimiento a insertar.</param>
        public void AgregarMantenimiento(Mantenimientos m)
        {
            db.Mantenimientos.Add(m);

            // Marcar moto como EnTaller
            var moto = db.Motos.Find(m.id_moto);
            if (moto != null)
                moto.estado = "Taller";

            db.SaveChanges();
        }

        /// <summary>
        /// Actualiza un mantenimiento existente; si cambia de moto, ajusta el estado
        /// de la moto anterior y de la nueva.
        /// </summary>
        /// <param name="m">Mantenimiento con los datos actualizados.</param>
        public void EditarMantenimiento(Mantenimientos m)
        {
            var existing = db.Mantenimientos.Find(m.id_mantenimiento);
            if (existing != null)
            {
                // Si cambia la moto, liberar la anterior y poner la nueva EnTaller
                if (existing.id_moto != m.id_moto)
                {
                    var motoAnterior = db.Motos.Find(existing.id_moto);
                    if (motoAnterior != null)
                        motoAnterior.estado = "Disponible";

                    var motoNueva = db.Motos.Find(m.id_moto);
                    if (motoNueva != null)
                        motoNueva.estado = "Taller";
                }

                existing.id_moto = m.id_moto;
                existing.id_mecanico = m.id_mecanico;
                existing.fecha_recepcion = m.fecha_recepcion;
                existing.fecha_entrega = m.fecha_entrega;
                existing.motivo_cliente = m.motivo_cliente;
                existing.trabajo_realizado = m.trabajo_realizado;
                existing.costo = m.costo;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina un mantenimiento y devuelve la moto asociada al estado "Disponible".
        /// </summary>
        /// <param name="id">Identificador del mantenimiento a eliminar.</param>
        public void EliminarMantenimiento(int id)
        {
            var m = db.Mantenimientos.Find(id);
            if (m != null)
            {
                var moto = db.Motos.Find(m.id_moto);
                if (moto != null)
                    moto.estado = "Disponible"; // sale del taller

                db.Mantenimientos.Remove(m);
                db.SaveChanges();
            }
        }
    }
}
