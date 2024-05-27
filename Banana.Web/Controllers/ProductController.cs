using Banana.Web.Models;
using Banana.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Banana.Web.Controllers
{
    public class ProductController(IProductService productService) : Controller
    {
        private readonly IProductService _productService = productService;
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? list = new();
            ResponseDto? response = await _productService.GetAllProductsAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }
        public IActionResult ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }
         public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto? response = await _productService.GetProductAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDto product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();

        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.UpdateProductAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product edited successfully";
                    RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto? response = await _productService.GetProductAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDto product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto product)
        {
            ResponseDto? response = await _productService.DeleteProductAsync(product.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }
    }
}
