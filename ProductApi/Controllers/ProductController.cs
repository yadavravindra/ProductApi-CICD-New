using Microsoft.AspNetCore.Mvc;
using ProductApi.Services;
using ProductApi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }
     
    [HttpGet]

    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }
          
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.AddAsync(dto);
        return Ok("Product Added Successfully");
    }

    // PUT: api/product/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok("Product Updated Successfully");
    }

    // DELETE: api/product/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok("Product Deleted Successfully");
    }
}