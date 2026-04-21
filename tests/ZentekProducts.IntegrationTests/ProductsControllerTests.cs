using System.Net;
using System.Net.Http.Json;
using ZentekProducts.Api.Models.DTOs;

namespace ZentekProducts.IntegrationTests;

public class ProductsControllerTests
{
    private readonly HttpClient _client;

    public ProductsControllerTests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
    }

    [Fact(Skip = "Requires API running on localhost:5000")]
    public async Task Health_Get_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact(Skip = "Requires API running on localhost:5000")]
    public async Task Auth_Login_WithValidCredentials_ReturnsToken()
    {
        var loginDto = new LoginDto { Username = "admin", Password = "admin123" };
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var token = await response.Content.ReadFromJsonAsync<TokenDto>();
        Assert.NotNull(token);
        Assert.NotNull(token.Token);
    }

    [Fact(Skip = "Requires API running on localhost:5000")]
    public async Task Auth_Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var loginDto = new LoginDto { Username = "wrong", Password = "wrong" };
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(Skip = "Requires API running on localhost:5000")]
    public async Task Products_GetAll_WithoutAuth_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/api/products");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(Skip = "Requires API running on localhost:5000")]
    public async Task Products_GetAll_WithValidAuth_ReturnsProducts()
    {
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login",
            new LoginDto { Username = "admin", Password = "admin123" });
        var token = await loginResponse.Content.ReadFromJsonAsync<TokenDto>();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token?.Token);

        var response = await _client.GetAsync("/api/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact(Skip = "Requires API running on localhost:5000")]
    public async Task Products_Create_WithValidAuth_ReturnsCreated()
    {
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login",
            new LoginDto { Username = "admin", Password = "admin123" });
        var token = await loginResponse.Content.ReadFromJsonAsync<TokenDto>();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token?.Token);

        var productDto = new CreateProductDto
        {
            Name = "Test Product",
            Colour = "Red",
            Price = 99.99m,
            Description = "Test description",
            StockQty = 10
        };
        var response = await _client.PostAsJsonAsync("/api/products", productDto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}