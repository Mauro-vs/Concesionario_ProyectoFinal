using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Model.Repo
{
    /// <summary>
    /// Repositorio para operaciones CRUD sobre la entidad <see cref="Mecanicos"/>.
    /// </summary>
    public class MecanicosRepo
    {
        private readonly ConcesionarioBDEntities1 db = new ConcesionarioBDEntities1();

        /// <summary>
        /// Obtiene la lista completa de mecánicos.
        /// </summary>
        /// <returns>Lista de <see cref="Mecanicos"/> existentes en la base de datos.</returns>
        public List<Mecanicos> GetAllMecanicos()
        {
            return db.Mecanicos.ToList();
        }

        /// <summary>
        /// Obtiene un mecánico por su identificador.
        /// </summary>
        /// <param name="id">Identificador del mecánico.</param>
        /// <returns>
        /// Instancia de <see cref="Mecanicos"/> que coincide con el identificador especificado
        /// o <c>null</c> si no se encuentra.
        /// </returns>
        public Mecanicos GetMecanicoById(int id)
        {
            return db.Mecanicos.Find(id);
        }

        /// <summary>
        /// Agrega un nuevo mecánico a la base de datos.
        /// </summary>
        /// <param name="m">Instancia de <see cref="Mecanicos"/> a agregar.</param>
        public void AgregarMecanico(Mecanicos m)
        {
            db.Mecanicos.Add(m);
            db.SaveChanges();
        }

        /// <summary>
        /// Edita los datos de un mecánico existente.
        /// </summary>
        /// <param name="m">
        /// Instancia de <see cref="Mecanicos"/> con los datos actualizados. 
        /// Se usa la propiedad <see cref="Mecanicos.id_mecanico"/> para localizar el registro.
        /// </param>
        public void EditarMecanico(Mecanicos m)
        {
            var existing = db.Mecanicos.Find(m.id_mecanico);
            if (existing != null)
            {
                // Actualiza las propiedades
                existing.nombre = m.nombre;
                existing.apellidos = m.apellidos;
                existing.especialidad = m.especialidad;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina un mecánico por su identificador.
        /// </summary>
        /// <param name="id">Identificador del mecánico a eliminar.</param>
        public void EliminarMecanico(int id)
        {
            var m = db.Mecanicos.Find(id);
            if (m != null)
            {
                db.Mecanicos.Remove(m);
                db.SaveChanges();
            }
        }
    }
}
