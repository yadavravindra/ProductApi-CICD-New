using Microsoft.Data.SqlClient;
using ProductApi.Models;
using ProductApi.DTOs;
using System.Data;

namespace ProductApi.Repositories;

public class ProductRepository
{
    private readonly IConfiguration _config;

    public ProductRepository(IConfiguration config)
    {
        _config = config;
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
    }

    // ✅ Get All
    public async Task<List<Product>> GetAllAsync()
    {
        var list = new List<Product>();

        using var con = GetConnection();
        using var cmd = new SqlCommand("sp_GetProducts", con);
        cmd.CommandType = CommandType.StoredProcedure;

        await con.OpenAsync();
        var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new Product
            {
                Id = (int)reader["Id"],
                Name = reader["Name"]?.ToString() ?? "",
                Price = (decimal)reader["Price"]
            });
        }

        return list;
    }

    // ✅ Get By Id
    public async Task<Product?> GetByIdAsync(int id)
    {
        using var con = GetConnection();
        using var cmd = new SqlCommand("sp_GetProductById", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Id", id);

        await con.OpenAsync();
        var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Product
            {
                Id = (int)reader["Id"],
                Name = reader["Name"]?.ToString() ?? "",
                Price = (decimal)reader["Price"]
            };
        }

        return null;
    }

    // ✅ Add
    public async Task AddAsync(ProductDto dto)
    {
        using var con = GetConnection();
        using var cmd = new SqlCommand("sp_AddProduct", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Name", dto.Name);
        cmd.Parameters.AddWithValue("@Price", dto.Price);

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    // ✅ Update
    public async Task UpdateAsync(int id, ProductDto dto)
    {
        using var con = GetConnection();
        using var cmd = new SqlCommand("sp_UpdateProduct", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Name", dto.Name);
        cmd.Parameters.AddWithValue("@Price", dto.Price);

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    // ✅ Delete
    public async Task DeleteAsync(int id)
    {
        using var con = GetConnection();
        using var cmd = new SqlCommand("sp_DeleteProduct", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Id", id);

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}