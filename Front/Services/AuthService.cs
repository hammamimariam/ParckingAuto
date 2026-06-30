using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using ParckingAuto.DTO;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public string Token { get; private set; } = "";
    public string Role { get; private set; } = "";
    public string UserName { get; private set; } = "";

    public event Action? AuthStateChanged;

    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

    public AuthService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<bool> TryRestoreSessionAsync()
    {
        try
        {
            var session = await _js.InvokeAsync<AuthSession>("authStorage.load");
            if (string.IsNullOrEmpty(session.Token)) return false;

            Token = session.Token;
            Role = session.Role;
            UserName = session.UserName;
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            NotifyAuthChanged();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<(bool Success, string Role)> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("Auth/login", new LoginRequest { Email = email, Password = password });
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null)
                {
                    Token = loginResponse.Token;
                    Role = loginResponse.Role;
                    UserName = loginResponse.Nom;
                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                    await _js.InvokeVoidAsync("authStorage.save", Token, Role, UserName);
                    NotifyAuthChanged();
                    return (true, Role);
                }
            }
        }
        catch
        {
        }

        return (false, "");
    }

    public async Task LogoutAsync()
    {
        Token = "";
        Role = "";
        UserName = "";
        _http.DefaultRequestHeaders.Authorization = null;
        await _js.InvokeVoidAsync("authStorage.clear");
        NotifyAuthChanged();
    }

    private void NotifyAuthChanged() => AuthStateChanged?.Invoke();

    private class AuthSession
    {
        public string Token { get; set; } = "";
        public string Role { get; set; } = "";
        public string UserName { get; set; } = "";
    }
}
