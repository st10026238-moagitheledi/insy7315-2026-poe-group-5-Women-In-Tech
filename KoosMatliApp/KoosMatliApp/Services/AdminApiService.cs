using KoosMatliApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace KoosMatliApp.Services
{
    public class AdminApiService : IAdminApiService
    {
        private readonly HttpClient _http;

        private readonly JsonSerializerOptions _jsonOptions =
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        public AdminApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<SchoolDate>> GetSchoolDatesAsync()
        {
            var response = await _http.GetAsync("api/admin/school/dates");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<SchoolDate>>(
                _jsonOptions) ?? new List<SchoolDate>();
        }

        public async Task<SchoolDate?> AddSchoolDateAsync(SchoolDate schoolDate)
        {
            var response = await _http.PostAsJsonAsync(
                "api/admin/school/dates",
                schoolDate,
                _jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    $"Adding the school date failed: {(int)response.StatusCode} " +
                    $"{response.ReasonPhrase}. {error}");
            }

            return await response.Content.ReadFromJsonAsync<SchoolDate>(
                _jsonOptions);
        }
    }
}