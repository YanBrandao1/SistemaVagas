using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace SistemaVagas.Settings
{
    public class AppSettings
    {
        public FirebaseSettings Firebase { get; set; }
    }

    public class FirebaseSettings
    {
        public string ApiKey { get; set; }
        public string DatabaseUrl { get; set; }
    }
    public class FirebaseErrorResponse
    {
        public FirebaseError error { get; set; }
    }

    public class FirebaseError
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<FirebaseErrorDetail> errors { get; set; }
    }

    public class FirebaseErrorDetail
    {
        public string message { get; set; }
        public string domain { get; set; }
        public string reason { get; set; }
    }
}
