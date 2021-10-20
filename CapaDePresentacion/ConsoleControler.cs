using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;


namespace UI.Consola
{
    using System;
    using Negocio;
    using Negocio.Modelos;

    // MascotaDTO para la vista
    using UI;
    public class Controlador
    {
        private GestorDeMascotas sistema;
        private Vista vista;
        private Dictionary<string, Action> casosDeUso;

        public Controlador(GestorDeMascotas sistema, Vista vista)
        {
            this.sistema = sistema;
            this.vista = vista;
            // c# Action, Func, Predicate: programación funcional
            // c# Dictionary Colección genérica
            this.casosDeUso = new Dictionary<string, Action>(){
                // Action = Func sin valor de retorno
                { "Obtener los propietarios", ObtenerPropietarios },
                { "Obtener los mascotas RAW", ObtenerMascotas },
                { "Obtener los mascotas", ObtenerMascotasDTO },
                { "Comprar una mascota", ComprarMascota },
                { "Mascotas perro con propietario Chico", MascotasPerroConDueño },
                { "Gatas con dueñas (predicado)",MascotasSelectasYLambdaComoParametro },
                { "Pruebas genéricas de input", PruebasDeObtenerEntradaDeTipo },
                // Lambda
                { "Obtener la luna",() => vista.MuestraError($"Caso de uso no implementado") },
            };
        }

        public void Run()
        {
            vista.LimpiarPantalla();
            // Acceso a las Claves del diccionario
            var menu = casosDeUso.Keys.ToList<String>();
            while (true)
                try
                {
                    // Menu
                    var key = vista.TrySeleccionarOpcionDeListaEnumerada("Menu de Usuario", menu, "Seleciona una opción");
                    // Ejecución de la opción escogida
                    vista.LimpiarPantalla();
                    vista.MuestraTitulo(key);
                    casosDeUso[key].Invoke();

                    vista.MuestraLineYEsperaReturn("Pulsa <Return> para continuar");
                    vista.LimpiarPantalla();
                }
                catch { return; }
        }

        // CASOS DE USO
        private void ObtenerPropietarios() =>
            vista.MostrarListaEnumerada("Todos los Propietarios", sistema.ObtenerPropietarios());
        void ObtenerMascotas() =>
            vista.MostrarListaEnumerada("Todos las Mascotas", sistema.ObtenerMascotas());
        void ObtenerMascotasDTO()
        {
            var propietarios = sistema.ObtenerPropietarios();
            var mascotas = sistema.ObtenerMascotas();
            var query = from dueño in propietarios
                        join mascota in mascotas on dueño.IdPropietario equals mascota.IdPropietario
                        select new MascotaMV { Propietario = dueño.Nombre, Nombre = mascota.Nombre, Especie = mascota.Especie };

            vista.MostrarListaEnumerada("Todos las Mascotas", query.ToList());
        }

        void ComprarMascota()
        {
            try
            {
                var propietario = vista.TrySeleccionarOpcionDeListaEnumerada("Comprador de mascota", sistema.ObtenerPropietarios(), "Escoje un comprador");
                var nombre = vista.TryObtenerEntradaDeTipo<string>("¿Cómo se llama la mascota?");
                var especie = vista.TryObtenerEntradaDeTipo<string>("Indicame la especie");
                var hecho = sistema.ComprarMascota(propietario, new Mascota { Nombre = nombre, Especie = especie });
                vista.MuestraMensaje(hecho ? "Mascota felizmente adquirida" : "Operación no permitida");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        void MascotasPerroConDueño()
        {
            // Linq con selección WHERE expecífica
            var propietarios = sistema.ObtenerPropietarios();
            var mascotas = sistema.ObtenerMascotas();
            var query = from dueño in propietarios
                        join mascota in mascotas on dueño.IdPropietario equals mascota.IdPropietario
                        where dueño.Sexo == 'H' & mascota.Especie == "perro"
                        select new MascotaMV { Propietario = dueño.Nombre, Nombre = mascota.Nombre, Especie = mascota.Especie };
            vista.MostrarListaEnumerada("Mascotas con dueño chico", query.ToList());
        }

        // Idem con Funcion de seleccion lambda 
        void MascotasSelectasYLambdaComoParametro()
        {
            MascotasEsCogidas("Super gatas con Dueña", (Propietario p, Mascota m) => p.Sexo == 'M' && m.Especie == "gato");
        }
        void MascotasEsCogidas(String title, Func<Propietario, Mascota, bool> esLaEscogida)
        {
            var propietarios = sistema.ObtenerPropietarios();
            var mascotas = sistema.ObtenerMascotas();
            var query = from dueño in propietarios
                        join mascota in mascotas on dueño.IdPropietario equals mascota.IdPropietario
                        where esLaEscogida(dueño, mascota)
                        select new MascotaMV { Propietario = dueño.Nombre, Nombre = mascota.Nombre, Especie = mascota.Especie };
            vista.MostrarListaEnumerada(title, query.ToList());
        }

        void PruebasDeObtenerEntradaDeTipo()
        {
            try
            {
                var s = vista.TryObtenerEntradaDeTipo<string>("un string");
                Console.WriteLine($"Recibido: {s}");
                var d = vista.TryObtenerEntradaDeTipo<decimal>("un decimal");
                Console.WriteLine($"Recibido: {d}");
                var f = vista.TryObtenerEntradaDeTipo<float>("un float");
                Console.WriteLine($"Recibido: {f}");
                var i = vista.TryObtenerEntradaDeTipo<int>("un int");
                Console.WriteLine($"Recibido: {i}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }
}