using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;


namespace Servicios
{
    using Negocio.Modelos;

    public class RepositorioCSV : IRepositorio
    {
        const string dataPath  = "Data/";
        string propietariosFile  = dataPath +"propietarios.csv";
        string mascotasFile = dataPath +"mascotas.csv";
        void IRepositorio.Inicializar(){}
        List<Propietario> IRepositorio.CargarPropietarios()
        {
            return File.ReadAllLines(propietariosFile)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(p=>Propietario.ParseRow(p)).ToList();
        }
        List<Mascota> IRepositorio.CargarMascotas() =>
            File.ReadAllLines(mascotasFile)
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(Mascota.ParseRow).ToList();
    }
}