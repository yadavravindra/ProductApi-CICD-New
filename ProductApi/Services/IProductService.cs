using ProductApi.Models;
using ProductApi.DTOs;

namespace ProductApi.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(ProductDto dto);
    Task UpdateAsync(int id, ProductDto dto);
    Task DeleteAsync(int id);
}   