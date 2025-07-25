using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SistemaVagas.Models;
using SistemaVagas.Settings;

namespace SistemaVagas.Services
{
    public class FirebaseAuthService
    {
        private readonly string apiKey;
        private static string _idToken;
        public static string IdToken => _idToken;
        public static bool IsUserLoggedIn => !string.IsNullOrWhiteSpace(_idToken);
        public static void Logout()
        {
            _idToken = null;
            SecureStorage.Remove("auth_token");
        }

        HttpClient httpClient = new HttpClient();

        public FirebaseAuthService(string apiKey)
        {
            this.apiKey = apiKey;
        }

        // Método que realiza o login do usuário
        public async Task<FirebaseAuthResponse> LoginAsync(string email, string password)
        {
            // Monta o payload
            var payload = new FirebaseAuthRequest
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);

            // Chama o endpoint do Firebase
            var response = await httpClient.PostAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}",
                new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            );

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // 1) desserializa a resposta
                var authResponse =
                    JsonConvert.DeserializeObject<FirebaseAuthResponse>(jsonResponse)!;

                // 2) armazena o token em memória
                _idToken = authResponse.IdToken;

                // 3) devolve o objeto de resposta
                return authResponse;
            }
            else
            {
                // mantém seu tratamento de erro atual
                var firebaseError =
                    JsonConvert.DeserializeObject<FirebaseErrorResponse>(jsonResponse);

                string errorMessage = firebaseError?.error?.message switch
                {
                    "INVALID_EMAIL" => "E-mail inválido.",
                    "INVALID_LOGIN_CREDENTIALS" => "E-mail ou senha inválido(s).",
                    "USER_DISABLED" => "Esta conta foi desativada.",
                    _ => "Erro desconhecido. Verifique os dados e tente novamente."
                };

                throw new Exception(errorMessage);
            }
        }

        // Método que realiza o Cadastro de usuário
        public async Task<FirebaseAuthResponse> RegisterAsync(string email, string password)
        {
            FirebaseAuthRequest payload = new FirebaseAuthRequest();
            payload.email = email;
            payload.password = password;
            payload.returnSecureToken = true;

            var jsonPayload = JsonConvert.SerializeObject(payload);

            var response = await httpClient.PostAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}",
                new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            );

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var authResponse = JsonConvert.DeserializeObject<FirebaseAuthResponse>(jsonResponse);
                return authResponse;
            }
            else
            {
                var firebaseError = JsonConvert.DeserializeObject<FirebaseErrorResponse>(jsonResponse);

                string errorMessage = firebaseError?.error?.message switch
                {
                    "EMAIL_EXISTS" => "Já existe uma conta associada a este e-mail.",
                    "WEAK_PASSWORD : Password should be at least 6 characters" => "A senha deve ter pelo menos 6 caracteres.",
                    _ => "Erro desconhecido. Verifique os dados e tente novamente."
                };

                throw new Exception($"Erro no cadastro: {errorMessage}");
            }
        }
    }
}
