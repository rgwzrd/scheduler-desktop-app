using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Data
{
    internal class InMemoryUserRepository : IUserRepository
    {
        private readonly List<DemoUser> _users = new List<DemoUser>
        {
            new DemoUser { UserId = 1, Username = "test", Password = "test" }
        };

        public int? GetUserIdByCredentials(string username, string password)
        {
            username = (username ?? "").Trim();
            password = (password ?? "").Trim();

            var user = _users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            return user?.UserId;
        }

        private class DemoUser
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
