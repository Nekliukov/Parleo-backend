using Parleo.BLL.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IUtilityService
    {
        Task<IReadOnlyCollection<LanguageModel>> GetLanguagesAsync();
    }
}
