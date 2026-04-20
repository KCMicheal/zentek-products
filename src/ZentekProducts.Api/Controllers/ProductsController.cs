using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZentekProducts.Api.Models;
using ZentekProducts.Api.Models.DTOs;
using ZentekProducts.Api.Services;

namespace ZentekProducts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? colour)
    {
        IEnumerable<Product> products;

        if (!string.IsNullOrWhiteSpace(colour))
        {
            products = await _productService.GetProductsByColourAsync(colour);
        }
        else
        {
            products = await _productService.GetAllProductsAsync();
        }

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var product = await _productService.CreateProductAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
    }
}