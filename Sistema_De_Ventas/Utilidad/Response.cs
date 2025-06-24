namespace Sistema_De_Ventas.Utilidad
{
    public class Response<T>
    {
        public bool status { get; set; }
        public T value { get; set; }
        public string message { get; set; }
    }
}
