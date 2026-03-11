using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<int> AddAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
}