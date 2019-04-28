using Parleo.BLL.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IUtilityService
    {
        Task<IReadOnlyCollection<LanguageModel>> GetLanguagesAsync();

        Task<IReadOnlyCollection<HobbyModel>> GetHobbiesAsync();

        Task<bool> LanguageExistsAsync(ICollection<LanguageModel> languages);

        Task<bool> HobbiesExistsAsync(ICollection<HobbyModel> hobbies);
    }
}
