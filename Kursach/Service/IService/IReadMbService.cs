using Kursach.Models;

namespace Kursach.Service.IService;

public interface IReadMbService
{
    public Task<IEnumerable<MbModel>> GetMbAsync();
}