namespace ProductApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string EmpCode { get; set; }
        public string FirstName { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
    }
}
