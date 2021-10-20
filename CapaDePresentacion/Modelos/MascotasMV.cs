namespace UI
{
public class MascotaMV
    {
        public string Nombre;
        public string Especie;
        public string Propietario;

        // Comillas escapadas
        public override string ToString() => $"\"{Nombre}\" es un {Especie} de {Propietario}";
    }
}