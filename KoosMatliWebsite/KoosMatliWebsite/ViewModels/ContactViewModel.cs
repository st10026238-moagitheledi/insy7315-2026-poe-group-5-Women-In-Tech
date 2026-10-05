using KoosMatliWebsite.DTOs;
using KoosMatliWebsite.Models;

namespace KoosMatliWebsite.ViewModels
{
    public class ContactViewModel
    {
        public SchoolInformationDto? SchoolInformation { get; set; }

        public ContactMessage ContactMessage { get; set; } = new ContactMessage();
    }
}
