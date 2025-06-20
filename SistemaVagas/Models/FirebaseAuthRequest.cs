using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVagas.Models
{
    public class FirebaseAuthRequest
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool returnSecureToken { get; set; }

    }
}
