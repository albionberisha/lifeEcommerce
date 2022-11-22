using lifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Mvc;
using lifeEcommerce.Models.Dtos;
using lifeEcommerce.Models.Entities;
using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using lifeEcommerce.Helpers;

namespace lifeEcommerce.Controllers
{
    //[ApiController]
    //public class OrderController : Controller
    //{
    //    private readonly IOrderService _productService;
    //    private readonly IConfiguration _configuration;

    //    public OrderController(IOrderService productService, IConfiguration configuration)
    //    {
    //        _productService = productService;
    //        _configuration = configuration;
    //    }


    //    [HttpGet("GetOrder")]
    //    public async Task<IActionResult> Get(int id)
    //    {
    //        var product = await _productService.GetOrder(id);

    //        if (product == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(product);
    //    }

    //    [HttpGet("GetOrders")]
    //    public async Task<IActionResult> GetOrders()
    //    {
    //        var categories = await _productService.GetAllOrders();

    //        return Ok(categories);
    //    }

    //    [HttpGet("OrdersListView")]
    //    public async Task<IActionResult> OrdersListView(string? search, int categoryId = 0, int page = 1, int pageSize = 10)
    //    {
    //        var products = await _productService.OrdersListView(search ,page, pageSize, categoryId);

    //        return Ok(products);
    //    }

    //    [HttpPost("PostOrder")]
    //    public async Task<IActionResult> Post([FromForm] OrderDataCreateDto OrderToCreate)
    //    {
    //        await _productService.CreateOrder(OrderToCreate);

    //        return Ok("Order created successfully!");
    //    }

    //    [HttpPut("UpdateOrder")]
    //    public async Task<IActionResult> Update(OrderDataDto OrderToUpdate)
    //    {
    //        await _productService.UpdateOrder(OrderToUpdate);

    //        return Ok("Order updated successfully!");
    //    }

    //    [HttpDelete("DeleteOrder")]
    //    public async Task<IActionResult> Delete(int id)
    //    {
    //        await _productService.DeleteOrder(id);

    //        return Ok("Order deleted successfully!");
    //    }

    //    [HttpPost("UploadImage")]
    //    public async Task<IActionResult> UploadImage(IFormFile file)
    //    {
    //        var uploadPicture = await UploadToBlob(file);

    //        var imageUrl = $"{_configuration.GetValue<string>("BlobConfig:CDNLife")}{file.FileName + Path.GetExtension(file.FileName)}";
            
    //        return Ok(imageUrl);
    //    }
    //}
}
