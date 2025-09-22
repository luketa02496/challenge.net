namespace ApiMottu.Models
{
    public class Resource<T>
    {
        public T Data { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();

        public Resource(T data)
        {
            Data = data;
        }
    }
}
