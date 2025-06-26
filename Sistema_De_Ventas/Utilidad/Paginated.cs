namespace Sistema_De_Ventas.Utilidad
{
    public class Paginated<T>
    {
        public int TotalRegistros { get; set; }
        public List<T> Resultados { get; set; }
    }
}
