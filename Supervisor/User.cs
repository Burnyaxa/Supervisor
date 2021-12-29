using System;

#nullable disable

namespace Supervisor
{
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Iv { get; set; }
        public string PasswordVersion { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
