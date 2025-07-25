using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SistemaVagas.Models;

namespace SistemaVagas.Services
{
    public static class FirebaseVagasService
    {
        public static async Task<bool> AdicionarVagaAsync(Vaga vaga, string firebaseDatabaseUrl)
        {
            using var httpClient = new HttpClient();

            string endpoint = $"{firebaseDatabaseUrl.TrimEnd('/')}/vagas.json";

            var json = JsonSerializer.Serialize(vaga);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(endpoint, content);

            return response.IsSuccessStatusCode;
        }
    }
}
