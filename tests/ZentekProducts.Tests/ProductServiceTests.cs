using Microsoft.EntityFrameworkCore;
using ZentekProducts.Api.Data;
using ZentekProducts.Api.Models;
using ZentekProducts.Api.Models.DTOs;
using ZentekProducts.Api.Services;

namespace ZentekProducts.Tests;

public class ProductServiceTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        using var context = CreateContext();
        context.Products.AddRange(
            new Product { Id = 1, Name = "Product 1", Colour = "Red", Price = 10 },
            new Product { Id = 2, Name = "Product 2", Colour = "Blue", Price = 20 }
        );
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var result = await service.GetAllProductsAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetProductsByColourAsync_FiltersByColour()
    {
        using var context = CreateContext();
        context.Products.AddRange(
            new Product { Id = 1, Name = "Product 1", Colour = "Red", Price = 10 },
            new Product { Id = 2, Name = "Product 2", Colour = "Blue", Price = 20 },
            new Product { Id = 3, Name = "Product 3", Colour = "RED", Price = 30 }
        );
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var result = await service.GetProductsByColourAsync("red");

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateProductAsync_CreatesNewProduct()
    {
        using var context = CreateContext();
        var service = new ProductService(context);

        var dto = new CreateProductDto
        {
            Name = "New Product",
            Colour = "Green",
            Price = 50,
            Description = "Test product",
            Category = "Electronics",
            StockQty = 100
        };

        var result = await service.CreateProductAsync(dto);

        Assert.NotEqual(0, result.Id);
        Assert.Equal("New Product", result.Name);
        Assert.Equal("Green", result.Colour);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsProduct()
    {
        using var context = CreateContext();
        context.Products.Add(new Product { Id = 1, Name = "Product 1", Colour = "Red", Price = 10 });
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var result = await service.GetProductByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Product 1", result.Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsNullForInvalidId()
    {
        using var context = CreateContext();
        var service = new ProductService(context);

        var result = await service.GetProductByIdAsync(999);

        Assert.Null(result);
    }
}