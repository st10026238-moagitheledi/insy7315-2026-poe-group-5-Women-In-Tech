using KoosMatliWebsite.DTOs;

namespace KoosMatlilWebsite.Services
{
    public class WebsiteApiService
    {
        private readonly HttpClient _httpClient;

        public WebsiteApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NewsDto>> GetNewsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<NewsDto>>(
                "api/news"
            ) ?? new List<NewsDto>();
        }

        public async Task<NewsDto?> GetNewsByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<NewsDto>(
                $"api/news/{id}"
            );
        }

        public async Task<List<StaffMemberDto>> GetStaffAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<StaffMemberDto>>(
                "api/staffmembers"
            ) ?? new List<StaffMemberDto>();
        }

        public async Task<List<SchoolEventDto>> GetEventsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<SchoolEventDto>>(
                "api/events"
            ) ?? new List<SchoolEventDto>();
        }

        public async Task<List<GalleryItemDto>> GetGalleryAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<GalleryItemDto>>(
                "api/gallery"
            ) ?? new List<GalleryItemDto>();
        }

        public async Task<List<AdmissionDto>> GetAdmissionsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AdmissionDto>>(
                "api/admissions"
            ) ?? new List<AdmissionDto>();
        }

        public async Task<SchoolInformationDto?> GetSchoolInformationAsync()
        {
            return await _httpClient.GetFromJsonAsync<SchoolInformationDto>(
                "api/about"
            );
        }

        public async Task<bool> SubmitContactMessageAsync(ContactMessageDto message)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/contact",
                message);

            return response.IsSuccessStatusCode;
        }
    }
}