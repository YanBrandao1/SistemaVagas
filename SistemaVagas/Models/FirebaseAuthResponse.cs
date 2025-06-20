using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVagas.Models
{
    class FirebaseAuthResponse
    {
        public string IdToken { get; set; }          // JWT Token
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiresIn { get; set; }
        public string LocalId { get; set; }          // User ID
        public string Registered { get; set; }       // Para login
    }
}
