using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMApplication
{
    public class SubmitPasswordEventArgs : EventArgs
    {
        public string Source;
        public string Password;
    }
}
