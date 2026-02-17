using Concesionario.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Controller.API
{
    /// <summary>
    /// Logica de negocio para la gestión de modelos de motos.
    /// </summary>
    public class ModeloApi
    {
        private readonly ModeloRepo modeloRepo = new ModeloRepo();

        /// <summary>
        /// Obtiene todos los modelos de motos registrados en el concesionario, incluyendo sus detalles.
        /// </summary>
        /// <returns></returns>
        public List<Motos> ObtenerModelos()
        {
            try
            {
                return modeloRepo.GetModelos();
            }
            catch (Exception ex)
            {
                // Temporalmente, incluye el InnerException completo
                throw new Exception("Error al obtener modelo: " + ex.ToString());
            }
        }

        // METODO DE AGREGAR MODELO
        /// <summary>
        /// Agrega un nuevo modelo de moto al concesionario después de validar sus datos.
        /// </summary>
        /// <param name="modelo"> Modelo de moto a agregar.</param>
        public void AgregarModelo(Motos modelo)
        {
            try
            {
                validarCampos(modelo);
                modeloRepo.AgregarModelo(modelo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // METODO DE ACTUALIZAR MODELO 
        /// <summary>
        /// Actualiza los datos de un modelo de moto existente en el concesionario después de validar sus datos.
        /// </summary>
        /// <param name="modelo"> Modelo de moto con los datos actualizados.</param>
        public void ActualizarModelo(Motos modelo)
        {
            try
            {
                validarCampos(modelo);
                modeloRepo.ActualizarModelo(modelo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // METODO DE ELIMINAR MODELO
        /// <summary>
        /// Elimina un modelo de moto del concesionario. Antes de eliminar, se debe verificar que el modelo no tenga mantenimientos pendientes en el taller para evitar inconsistencias en la base de datos.
        /// </summary>
        /// <param name="id"> Identificador del modelo de moto a eliminar.</param>
        public void EliminarModelo(int id)
        {
            try
            {
                modeloRepo.EliminarModelo(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // VALIDAR CAMPOS DE MODELO
        /// <summary>
        /// Validamos los campos de un modelo de moto para asegurarnos de que cumplen con las reglas de negocio antes de agregarlo o actualizarlo en la base de datos. 
        /// Esto incluye validar que los campos obligatorios estén presentes, 
        /// que los valores numéricos sean razonables, y que la matrícula tenga un formato correcto y sea única si se proporciona.
        /// </summary>
        /// <param name="modelo"> Modelo de moto a validar.</param>
        /// <exception cref="ArgumentNullException"> Se produce si el modelo es nulo.</exception>
        /// <exception cref="Exception"> Se produce si algún campo no cumple con las reglas de validación.</exception>
        public void validarCampos(Motos modelo)
        {
            if (modelo == null)
            {
                throw new ArgumentNullException(nameof(modelo), "El modelo no puede ser nulo.");
            }

            // Marca y modelo obligatorios
            if (string.IsNullOrWhiteSpace(modelo.marca) ||
                string.IsNullOrWhiteSpace(modelo.modelo))
            {
                throw new Exception("Los campos principales son obligatorios");
            }

            // Solo validar matrícula si viene informada
            if (!string.IsNullOrWhiteSpace(modelo.matricula))
            {
                validarMatricula(modelo.matricula);
                validarMatriculaUnica(modelo.matricula, modelo.id_moto);
            }

            // Año obligatorio y rango razonable
            var anioActual = DateTime.Now.Year;
            if (modelo.anio <= 0)
            {
                throw new Exception("El año es obligatorio.");
            }

            if (modelo.anio < 1950 || modelo.anio > anioActual + 1)
            {
                throw new Exception($"El año debe estar entre 1950 y {anioActual + 1}.");
            }

            // Precio debe ser positivo
            if (modelo.precio.HasValue && modelo.precio.Value < 0)
            {
                throw new Exception("El precio no puede ser negativo.");
            }

            // Kilómetros debe ser positivo
            if (modelo.kilometros.HasValue && modelo.kilometros.Value < 0)
            {
                throw new Exception("Los kilómetros no pueden ser negativos.");
            }

            // Si no se proporciona un estado, asignar "Disponible" por defecto
            if (string.IsNullOrEmpty(modelo.estado)) modelo.estado = "Disponible";
        }

        // VALIDAR FORMATO DE MATRICULA
        /// <summary>
        /// Valida que la matrícula de la moto tenga un formato correcto. 
        /// En este ejemplo, se espera un formato de 4 dígitos seguidos de 3 letras mayúsculas (ejemplo: 1234ABC).
        /// </summary>
        /// <param name="matricula"> Matrícula a validar.</param>
        /// <exception cref="Exception"> Se produce si la matrícula no cumple con el formato esperado.</exception>
        public void validarMatricula(string matricula)
        {
            // Validar formato de matrícula (ejemplo: 1234ABC)
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{4}[A-Z]{3}$");
            if (!regex.IsMatch(matricula))
            {
                throw new Exception("La matrícula debe tener el formato 1234ABC.");
            }
        }

        // VALIDAR QUE LA MATRICULA SEA UNICA
        /// <summary>
        /// Valida que la matrícula de la moto sea única en el sistema. 
        /// Si se está agregando un nuevo modelo, se verifica que no exista ningún otro modelo con la misma matrícula. 
        /// Si se está actualizando un modelo existente, se permite que tenga la misma matrícula siempre y cuando no haya otro modelo diferente con esa matrícula.
        /// </summary>
        /// <param name="matricula"> Matrícula a validar.</param>
        /// <param name="idMoto"> Identificador del modelo de moto que se está actualizando (opcional).</param>
        /// <exception cref="Exception"> Se produce si ya existe otro modelo con la misma matrícula.</exception>
        public void validarMatriculaUnica(string matricula, int? idMoto = null)
        {
            var modelosExistentes = modeloRepo.GetModelos();
            if (modelosExistentes.Any(m => m.matricula == matricula && m.id_moto != idMoto))
            {
                throw new Exception("La matrícula ya existe. Debe ser única.");
            }
        }
    }
}
