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

            // Agora envia para o nó "vagas_pendentes"
            string endpoint = $"{firebaseDatabaseUrl.TrimEnd('/')}/vagas_pendentes.json";

            var json = JsonSerializer.Serialize(vaga);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(endpoint, content);

            return response.IsSuccessStatusCode;
        }


        public static async Task<List<Vaga>> ObterVagasAsync(string databaseUrl)
        {
            using var httpClient = new HttpClient();

            string endpoint = $"{databaseUrl.TrimEnd('/')}/vagas.json";

            var response = await httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
                return new List<Vaga>();

            var json = await response.Content.ReadAsStringAsync();

            // Firebase retorna como dicionário (chave aleatória -> vaga)
            var vagasDict = JsonSerializer.Deserialize<Dictionary<string, Vaga>>(json);

            return vagasDict?.Values.ToList() ?? new List<Vaga>();
        }

        public static async Task<Dictionary<string, Vaga>> ObterVagasPendentesAsync(string databaseUrl)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{databaseUrl.TrimEnd('/')}/vagas_pendentes.json");

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, Vaga>>(json) ?? new();
        }

        public static async Task AprovarVagaAsync(string id, string databaseUrl)
        {
            using var httpClient = new HttpClient();

            // Pega a vaga pendente
            var vagaResp = await httpClient.GetAsync($"{databaseUrl}/vagas_pendentes/{id}.json");
            var vagaJson = await vagaResp.Content.ReadAsStringAsync();

            // Salva em "vagas"
            await httpClient.PostAsync($"{databaseUrl}/vagas.json", new StringContent(vagaJson, Encoding.UTF8, "application/json"));

            // Apaga de "vagas_pendentes"
            await httpClient.DeleteAsync($"{databaseUrl}/vagas_pendentes/{id}.json");
        }

        public static async Task RejeitarVagaAsync(string id, string databaseUrl)
        {
            using var httpClient = new HttpClient();
            await httpClient.DeleteAsync($"{databaseUrl}/vagas_pendentes/{id}.json");
        }


    }
}
