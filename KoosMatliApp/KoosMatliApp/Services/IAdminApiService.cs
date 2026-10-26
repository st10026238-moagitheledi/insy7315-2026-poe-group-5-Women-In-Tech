using KoosMatliApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoosMatliApp.Services
{
    public interface IAdminApiService
    {
        Task<List<SchoolDate>> GetSchoolDatesAsync();

        Task<SchoolDate?> AddSchoolDateAsync(SchoolDate schoolDate);
    }
}