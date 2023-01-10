using ShopsApi.Entities;
using AutoMapper;
using System;
using ShopsApi.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopsApi.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ShopsApi.Authorization;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ShopsApi.Services
{

    public interface IShopService
    {
        public PagedResault<ShopDto> Get(ShopQuery query);
        public bool Create(CreateShopDto dto);
        public void Delete(int shopId);
        public void Update(int shopId, UpdateShopDto dto);
    }
  public class ShopService : IShopService
    {
        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ShopService> _logger;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public ShopService(ShopDbContext context, IMapper mapper, ILogger<ShopService> logger, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _authorization = authorizationService;
            _userContextService = userContextService;
        }

        public PagedResault<ShopDto>Get(ShopQuery query)
        {

            var baseQuery = _context
                .Shops
                .Include(x => x.Adress)
                .Include(x => x.Products)
                .Where(x => query.SearchPhase == null || (x.Name.ToLower().Contains(query.SearchPhase.ToLower()) || x.Description.ToLower().Contains(query.SearchPhase.ToLower())));
             
            if(!string.IsNullOrEmpty(query.Sort))
            {
                var columsSelectors = new Dictionary<string, Expression<Func<Shop, object>>>
                {
                    {nameof(Shop.Name), x=>x.Name },
                    {nameof(Shop.Description), x=>x.Description }
                };

                var selectedColumn = columsSelectors[query.Sort];
               baseQuery = query.SortDirection == SortDirection.ASC
                ? baseQuery.OrderBy(x => x.Name)
                : baseQuery.OrderByDescending(x => x.Description);
            }

            var shop = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItems = baseQuery.Count();

            var shopDto = _mapper.Map<List<ShopDto>>(shop);

            var resault = new PagedResault<ShopDto>(shopDto, totalItems, query.PageSize, query.PageNumber);
            return resault;

        }
        public bool Create(CreateShopDto dto)
        {
            var shop = _mapper.Map<Shop>(dto);
            shop.CreatedById = _userContextService.GetUserId;
            _context.Shops.Add(shop);
            _context.SaveChanges();
            return true;
        }
        public void Delete(int shopId)
        {
            var shop = _context
                .Shops
                .FirstOrDefault(x => x.Id == shopId);

            if (shop is null)
                throw new NotFound("Shop not found");

            var authorizationResault = _authorization.AuthorizeAsync(_userContextService.User, shop, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResault.Succeeded)
            {
                throw new ForbidException();
            }

            _logger.LogWarning($"Shop with ID :{shopId} has beed deleted");

            _context.Shops.Remove(shop);
            _context.SaveChanges();
        }
        public void Update(int shopId, UpdateShopDto dto)
        {
            var shop = _context
                .Shops
                .Include(x => x.Adress)
                .FirstOrDefault(x => x.Id == shopId);

            if (shop is null)
            { 
                throw new NotFound("Shop not found");
            }

            _logger.LogWarning($"Shop with ID :{shopId} has beed updated");

           var authorizationResault = _authorization.AuthorizeAsync(_userContextService.User, shop, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if(!authorizationResault.Succeeded)
            {
                throw new ForbidException();
            }

            shop.Description = dto.Description;
            shop.Name = dto.Name;
            shop.ContactNumber = dto.ContactNumber;
            shop.Type = dto.Type;
            shop.WorkTime = dto.WorkTime;
            shop.ContactEmail = dto.ContactEmail;
            shop.Adress.Street = dto.Street;
            _context.Shops.Update(shop);
            _context.SaveChanges();
        }

    }
}
