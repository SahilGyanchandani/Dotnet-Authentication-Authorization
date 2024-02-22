namespace User.Management.Service.Models
{
    public class ServiceResponse<T>
    {
        public bool?  isSuccess { get; set; }
        public int? status {  get; set; }
        public string? message { get; set; }
        public T? response { get; set; }
    }
}
