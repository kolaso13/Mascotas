using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Negocio.Modelos
{
    public class Mascota
    {
        public string Nombre;
        public string Especie;
        public int IdPropietario;

        public override string ToString() => $"{Nombre}/{Especie}";

        internal static Mascota ParseRow(string row)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            var columns = row.Split(',');
            // PropId, Nombre, Especie
            // 10, Tobby, perro
            return new Mascota
            {
                Nombre = columns[1].Trim(),
                Especie = columns[2].Trim(),
                IdPropietario = Int32.Parse(columns[0].Trim(), nfi)
            };
        }
    }
}