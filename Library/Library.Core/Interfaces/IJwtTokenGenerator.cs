using Library.Core.Entities;

namespace Library.Core.Interfaces;

public interface IJwtTokenGenerator
{
    Task<string> GenerateTokenAsync(ApplicationUser applicationUser);
}
