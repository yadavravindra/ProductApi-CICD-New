using ProductApi.Models;
using ProductApi.DTOs;
using ProductApi.Repositories;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly ProductRepository _repo;

    public ProductService(ProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<Product>> GetAllAsync()
        => await _repo.GetAllAsync();

    public async Task<Product?> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);

    public async Task AddAsync(ProductDto dto)
        => await _repo.AddAsync(dto);

    public async Task UpdateAsync(int id, ProductDto dto)
        => await _repo.UpdateAsync(id, dto);

    public async Task DeleteAsync(int id)
        => await _repo.DeleteAsync(id);
}