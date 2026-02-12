using Concesionario.Controller.API;
using Concesionario.Model;
using Concesionario.Model.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Concesionario.Testing
{
    [TestClass]
    public sealed class Test1
    {
        // TEST VALIDAR TELEFONO
        [TestMethod]
        public void ValidarTelefono()
        {
            var api = new ClienteApi();
            var c = new Clientes();
            c.dni = "12345678Z";
            c.nombre = "Pepe";
            c.apellidos = "Prueba";
            c.email = "pepe@correo.com";
            c.telefono = "123"; // Telefono no valido

            var error = Assert.ThrowsException<Exception>(() => api.validarCampos(c));

            Assert.AreEqual("El teléfono debe tener 9 dígitos.", error.Message);
        }

        // TEST VALIDAR PRECIO
        [TestMethod]
        public void ValidarPrecioNegativo()
        {
            var api = new ModeloApi();
            var m = new Motos();
            m.marca = "Honda";
            m.modelo = "CBR";
            m.anio = 2020;
            m.estado = "Disponible";
            m.precio = -500; // El precio no puede ser negativo

            var error = Assert.ThrowsException<Exception>(() => api.validarCampos(m));

            Assert.AreEqual("El precio no puede ser negativo.", error.Message);
        }

        // TEST VALIDAR COSTO
        [TestMethod]
        public void ValidarCostoNegativo()
        {
            var api = new TallerApi();
            var m = new Mantenimientos();
            m.id_moto = 1;
            m.id_mecanico = 1;
            m.motivo_cliente = "Revisión";
            m.costo = -10; // El costo no puede ser negativo

            var error = Assert.ThrowsException<Exception>(() => api.validarCampos(m));

            Assert.AreEqual("El costo no puede ser negativo.", error.Message);
        }

        // TEST DE INTEGRACION
        [TestMethod]
        public void GuardarCliente()
        {
            var repo = new ClientesRepo();
            var c = new Clientes();
            c.dni = "99999999X";
            c.nombre = "Test";
            c.apellidos = "Integracion";
            c.telefono = "600000000";
            c.email = "test@bd.com";

            repo.AgregarCliente(c);

            var encontrado = repo.GetAllClientes().FirstOrDefault(c => c.dni == "99999999X");

            // Assert.IsNotNull significa: "Espero que SÍ hayas encontrado algo"
            Assert.IsNotNull(encontrado, "El cliente no se guardó.");

            repo.EliminarCliente(encontrado.id_cliente);
        }
    }
}
