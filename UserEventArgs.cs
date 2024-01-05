using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMApplication
{
    public class UserEventArgs : EventArgs
    {
        public string PublicKeyFileName;
        public string PrivateKeyFileName;
        public string UserName;
    }
}
