using Parleo.BLL.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IUtilityService
    {
        Task<IReadOnlyCollection<LanguageModel>> GetLanguagesAsync();

        Task<IReadOnlyCollection<HobbyModel>> GetHobbiesAsync();

        Task<bool> AllLanguagesExistAsync(ICollection<LanguageModel> languages);

        Task<bool> AllHobbiesExistAsync(ICollection<HobbyModel> hobbies);
    }
}
