using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMApplication
{
    public class PasswordItem
    {
        public string Source { get; set; }
        public string Password { get; set; }

        public PasswordItem(string source, string password)
        {
            Source = source;
            Password = password;
        }
    }
}
