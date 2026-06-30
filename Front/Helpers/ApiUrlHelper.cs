namespace Front.Helpers
{
    public static class ApiUrlHelper
    {
        public static string FileUrl(HttpClient http, string? path)
        {
            if (string.IsNullOrWhiteSpace(path)) return "#";
            var baseUri = http.BaseAddress ?? new Uri("https://localhost:7275/api/");
            var origin = baseUri.GetLeftPart(UriPartial.Authority);
            return $"{origin}/{path.TrimStart('/')}";
        }
    }
}
