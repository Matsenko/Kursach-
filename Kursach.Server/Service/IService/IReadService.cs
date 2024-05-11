using Kursach.Models;

namespace Kursach.Service.IService;

public interface IReadService
{
    public Task<IEnumerable<MbModel>> GetMbAsync();
}