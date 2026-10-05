using KoosMatlilWebsite.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.ViewComponents
{
    public class SchoolContactViewComponent : ViewComponent
    {
        private readonly WebsiteApiService _apiService;

        public SchoolContactViewComponent(WebsiteApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var schoolInformation =
                await _apiService.GetSchoolInformationAsync();

            return View(schoolInformation);
        }
    }
}
