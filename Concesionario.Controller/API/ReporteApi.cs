using System.Linq;
using Concesionario.Model;
using Concesionario.Model.Informes;
using System.Data.Entity;

namespace Concesionario.Controller.API
{
    /// <summary>
    /// Proporciona métodos para obtener distintos <see cref="ConcesionarioDS"/> 
    /// utilizados en los informes del concesionario.
    /// </summary>
    public class ReporteApi
    {
        /// <summary>
        /// Obtiene un <see cref="ConcesionarioDS"/> con los datos de todos los clientes.
        /// </summary>
        /// <returns>
        /// Un objeto <see cref="ConcesionarioDS"/> cuya tabla <see cref="ConcesionarioDS.TablaCliente"/>
        /// contiene una fila por cada cliente de la base de datos.
        /// </returns>
        public ConcesionarioDS GetClientesDataSet()
        {
            using (var db = new ConcesionarioBDEntities1())
            {
                var ds = new ConcesionarioDS();
                var tabla = ds.TablaCliente;

                foreach (var c in db.Clientes.ToList())
                {
                    // Utilizo el AddTablaClienteRow q me lo genera el dataset,
                    // para añadir filas a la tabla del dataset
                    tabla.AddTablaClienteRow(
                        c.dni,
                        c.nombre,
                        c.apellidos,
                        c.telefono,
                        c.email
                    );
                }

                return ds;
            }
        }

        /// <summary>
        /// Obtiene un <see cref="ConcesionarioDS"/> con los datos de los mantenimientos de taller.
        /// </summary>
        /// <returns>
        /// Un objeto <see cref="ConcesionarioDS"/> cuya tabla <see cref="ConcesionarioDS.TablaTaller"/>
        /// contiene una fila por cada mantenimiento, incluyendo el mecánico, la moto,
        /// el trabajo realizado y el coste.
        /// </returns>
        public ConcesionarioDS GetTallerDataSet()
        {
            using (var db = new ConcesionarioBDEntities1())
            {
                var ds = new ConcesionarioDS();
                var tabla = ds.TablaTaller;

                var lista = db.Mantenimientos
                              .Include(m => m.Mecanicos)
                              .Include(m => m.Motos)
                              .ToList();

                foreach (var m in lista)
                {
                    tabla.AddTablaTallerRow(
                        m.Mecanicos?.nombre ?? "",
                        m.Motos?.matricula ?? "",
                        m.trabajo_realizado ?? "",
                        m.costo ?? 0m
                    );
                }

                return ds;
            }
        }

        /// <summary>
        /// Obtiene un <see cref="ConcesionarioDS"/> con los datos de las reservas realizadas.
        /// </summary>
        /// <returns>
        /// Un objeto <see cref="ConcesionarioDS"/> cuya tabla <see cref="ConcesionarioDS.TablaReservas"/>
        /// contiene una fila por cada reserva, incluyendo cliente, modelo de moto,
        /// fecha de reserva e importe de la señal.
        /// </returns>
        public ConcesionarioDS GetReservasDataSet()
        {
            using (var db = new ConcesionarioBDEntities1())
            {
                var ds = new ConcesionarioDS();
                var tabla = ds.TablaReservas;
                var lista = db.Reservas
                              .Include(r => r.Clientes)
                              .Include(r => r.Motos)
                              .ToList();

                foreach (var r in lista)
                {
                    tabla.AddTablaReservasRow(
                        r.Clientes.nombre ?? "",
                        r.Motos?.modelo ?? "",
                        (System.DateTime)r.fecha_reserva,
                        r.importe_senal
                    );
                }

                return ds;
            }
        }
    }
}
