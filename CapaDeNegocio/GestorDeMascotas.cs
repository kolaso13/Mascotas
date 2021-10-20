using System;
using System.Collections.Generic;
using System.Linq;


using Servicios;

namespace Negocio
{
    using Negocio.Modelos;

    public class GestorDeMascotas
    {
        IRepositorio Repositorio;
        List<Propietario> cachePropietarios = new List<Propietario>{};
        List<Mascota> cacheMascotas = new List<Mascota>{};

        public GestorDeMascotas(IRepositorio repositorio)
        {
            Repositorio = repositorio;
            Repositorio.Inicializar();
            cachePropietarios = Repositorio.CargarPropietarios();
            cacheMascotas = Repositorio.CargarMascotas();
        }
        public List<Propietario> ObtenerPropietarios() => cachePropietarios;
        public List<Mascota> ObtenerMascotas() => cacheMascotas;
        public bool ComprarMascota(Propietario propi, Mascota masco)
        {
            // Podríamos aplicar aquí reglas de Negocio
            // Por ejemplo NO PERMITIR COMRAR A MENORES DE 18 => return false;
            masco.IdPropietario = propi.IdPropietario;
            cacheMascotas.Add(masco);
            return true;
        }
    }
}