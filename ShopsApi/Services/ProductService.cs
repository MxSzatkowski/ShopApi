
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopsApi.Entities;
using ShopsApi.Exceptions;
using ShopsApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShopsApi.Services
{ 
    public interface IProductService
    {
        public List<ProductDto> Get(int shopId);
        public bool Create(int shopId, CreateProductDto dto);
        public void DeleteById(int shopId, int productId);
    }

    public class ProductService : IProductService
    {
        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ShopDbContext context, IMapper mapper, ILogger<ProductService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public List<ProductDto> Get(int shopId)
        {
            var product = _context
                .Shops
                .Include(x => x.Products)
                .FirstOrDefault(x => x.Id == shopId);
            var productDto = _mapper.Map<List<ProductDto>>(product.Products);

            return productDto;

        }

        public bool Create(int shopId, CreateProductDto dto)
        {
            _logger.LogWarning($"Product in shop with ID :{shopId} has beed deleted");
            var product = _context
                .Shops
                .FirstOrDefault(x => x.Id == shopId);
            var productDto = _mapper.Map<Product>(dto);
            productDto.ShopId = shopId;
            _context.Products.Add(productDto);
            _context.SaveChanges();
            return true;
        }

        public void DeleteById(int shopId, int productId)
        {
            _logger.LogWarning($"Product in shop with ID :{shopId} has beed deleted");
            var shop = _context
                .Shops
                .Include(x => x.Products)
                .FirstOrDefault(x =>x.Id == shopId);

            var product = _context
                .Products
                .FirstOrDefault(x => x.Id == productId);

            if (product is null)
                throw new NotFound("Product not found");

            _context.RemoveRange(product);
            _context.SaveChanges();

        }
    }
    
}
