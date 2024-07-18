using ThreadBasket.Domain.Entities;

namespace ThreadBasket.Domain.Contracts;

public interface IDmcThreadRepository
{
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsAsync(string floss);
    Task<int> CountAsync();
    Task<DmcThread?> GetThreadAsync(int id);
    Task<IEnumerable<DmcThread>> GetThreadListAsync(int page, int size);
    Task<int?> AddThreadAsync(DmcThread thread);
    Task<bool> UpdateThreadAsync(DmcThread thread);
    Task<bool> DeleteThreadAsync(int id);
}