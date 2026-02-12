using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Model.Repo
{
    /// <summary>
    /// Repositorio para operaciones CRUD sobre la entidad <see cref="Clientes"/>.
    /// </summary>
    public class ClientesRepo
    {
        /// <summary>
        /// Contexto de base de datos de la aplicación.
        /// </summary>
        private readonly ConcesionarioBDEntities1 db = new ConcesionarioBDEntities1();

        /// <summary>
        /// Obtiene la lista completa de clientes.
        /// </summary>
        /// <returns>
        /// Lista de todas las entidades <see cref="Clientes"/> almacenadas en la base de datos.
        /// </returns>
        public List<Clientes> GetAllClientes()
        {
            return db.Clientes.ToList();
        }

        /// <summary>
        /// Obtiene un cliente por su identificador.
        /// </summary>
        /// <param name="id">Identificador del cliente.</param>
        /// <returns>
        /// La entidad <see cref="Clientes"/> que coincide con el identificador especificado,
        /// o <c>null</c> si no se encuentra.
        /// </returns>
        public Clientes GetClienteById(int id)
        {
            return db.Clientes.Find(id);
        }

        /// <summary>
        /// Agrega un nuevo cliente a la base de datos.
        /// </summary>
        /// <param name="c">Entidad <see cref="Clientes"/> que se va a agregar.</param>
        public void AgregarCliente(Clientes c)
        {
            db.Clientes.Add(c);
            db.SaveChanges();
        }

        /// <summary>
        /// Edita los datos de un cliente existente.
        /// </summary>
        /// <param name="c">
        /// Entidad <see cref="Clientes"/> con los datos actualizados. 
        /// Se utiliza la propiedad <see cref="Clientes.id_cliente"/> para localizar el registro.
        /// </param>
        public void EditarCliente(Clientes c)
        {
            var existing = db.Clientes.Find(c.id_cliente);
            if (existing != null)
            {
                // Actualiza las propiedades
                existing.dni = c.dni;
                existing.nombre = c.nombre;
                existing.apellidos = c.apellidos;
                existing.telefono = c.telefono;
                existing.email = c.email;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina un cliente de la base de datos.
        /// </summary>
        /// <param name="id">Identificador del cliente que se desea eliminar.</param>
        public void EliminarCliente(int id)
        {
            var c = db.Clientes.Find(id);
            if (c != null)
            {
                db.Clientes.Remove(c);
                db.SaveChanges();
            }
        }
    }
}
