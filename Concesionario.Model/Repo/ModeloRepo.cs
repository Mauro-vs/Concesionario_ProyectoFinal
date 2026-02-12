using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Model
{
    /// <summary>
    /// Repositorio para gestionar operaciones CRUD sobre la entidad <see cref="Motos"/>.
    /// </summary>
    public class ModeloRepo
    {
        /// <summary>
        /// Contexto de base de datos de Entity Framework para el concesionario.
        /// </summary>
        private ConcesionarioBDEntities1 db = new ConcesionarioBDEntities1();

        /// <summary>
        /// Obtiene la lista completa de motos registradas en la base de datos.
        /// </summary>
        /// <returns>
        /// Una lista de objetos <see cref="Motos"/> con todos los modelos existentes.
        /// </returns>
        public List<Motos> GetModelos()
        {
            return db.Motos.ToList();
        }

        /// <summary>
        /// Obtiene una moto a partir de su identificador único.
        /// </summary>
        /// <param name="id">
        /// Identificador de la moto que se desea recuperar.
        /// </param>
        /// <returns>
        /// La instancia de <see cref="Motos"/> cuyo identificador coincide con el proporcionado,
        /// o <c>null</c> si no se encuentra ningún registro.
        /// </returns>
        public Motos GetModeloById(int id)
        {
            return db.Motos.Find(id);
        }

        /// <summary>
        /// Agrega una nueva moto al contexto y guarda los cambios en la base de datos.
        /// </summary>
        /// <param name="modelo">
        /// Objeto <see cref="Motos"/> que contiene la información de la moto a insertar.
        /// </param>
        public void AgregarModelo(Motos modelo)
        {
            db.Motos.Add(modelo);
            db.SaveChanges();
        }

        /// <summary>
        /// Actualiza la información de una moto existente en la base de datos.
        /// </summary>
        /// <param name="modelo">
        /// Objeto <see cref="Motos"/> con los valores actualizados.
        /// Debe contener un valor válido en la propiedad <see cref="Motos.id_moto"/>.
        /// </param>
        public void ActualizarModelo(Motos modelo)
        {
            var modeloExistente = db.Motos.Find(modelo.id_moto);
            if (modeloExistente != null)
            {
                db.Entry(modeloExistente).CurrentValues.SetValues(modelo);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina una moto de la base de datos a partir de su identificador.
        /// </summary>
        /// <param name="id">
        /// Identificador de la moto que se desea eliminar.
        /// </param>
        public void EliminarModelo(int id)
        {
            try
            {
                var modelo = db.Motos.Find(id);
                if (modelo != null)
                {
                    db.Motos.Remove(modelo);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo eliminar la moto. Verifique que no tenga mantenimientos pendientes.", ex);
            }
        }
    }
}
