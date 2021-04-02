using CacheAsidePattern.CacheManager.Interfaces;
using CacheAsidePattern.CacheStore.Interfaces;
using CacheWebApi.Context;
using CacheWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CacheWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICacheStore _cacheStore;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public CountriesController(AppDbContext context, ICacheStore cacheStore, ICacheManager cacheManager, IConfiguration configuration)
        {
            _context = context;
            _cacheStore = cacheStore;
            _cacheManager = cacheManager;
            _configuration = configuration;
        }

        // GET: api/Countries/CacheManager
        [HttpGet]
        [Route("CacheManager")]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountriesUsingCacheManager()
        {
            // Trater de obtener el Dato (La Lista de Paises) desde el cache y adicionarlo si no esta
            List<Country> countries = await _cacheManager.GetOrAddAsync<List<Country>>("Countries", async () =>
            {
                return await GetCountries();
            });

            return countries;
        }

        // GET: api/Countries/CacheStore
        [HttpGet]
        [Route("CacheStore")]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountriesUsingCacheStore()
        {
            // Trater de obtener el Dato (La Lista de Paises) desde el cache
            List<Country> countries = _cacheStore.Get<List<Country>>("Countries");

            // Validar si el dato NO esta en el cache
            if (countries is null)
            {
                // Obtener el dato desde la Fuente
                countries = await GetCountries();

                // Adicionar el dato al cache
                _cacheStore.Add<List<Country>>("Countries", countries);
            }
            else
            {
                Console.WriteLine("Trayendo datos del Cache");
            }

            return countries;
        }

        private async Task<List<Country>> GetCountries()
        {
            Console.WriteLine("Leyendo La Fuente");

            return await _context.Countries.ToListAsync();
        }
    }
}