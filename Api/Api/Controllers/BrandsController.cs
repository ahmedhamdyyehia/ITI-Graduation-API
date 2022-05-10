using Api.DTOs;
using Api.Errors;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BrandsController : BaseApiController
    {
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IMapper _mapper;

        public BrandsController(
            IGenericRepository<ProductBrand> productBrandRepo,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _productBrandRepo = productBrandRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductBrand>> GetBrandById(int id)
        {
            var type = await _productBrandRepo.GetByIdAsync(id);
            return type;
        }

      
        [HttpPost]
        public async Task<ActionResult<ProductBrand>> AddBrand(BrandToCreate brandToCreate)
        {
            var brand = _mapper.Map<BrandToCreate, ProductBrand>(brandToCreate);
           
            var result = await _productBrandRepo.AddEntityAsync(brand);

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating brand"));

            return Ok(brandToCreate);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductBrand>> UpdateBrand(int id, BrandToCreate brandToUpdate)
        {
            var brand = await _productBrandRepo.GetByIdAsync(id);
            _mapper.Map(brandToUpdate, brand);
                 
            var result = await _productBrandRepo.UpdateEntityAsync(brand);
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating brand"));

            return Ok(brand);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var brand = await _productBrandRepo.GetByIdAsync(id);

            var result = await _productBrandRepo.DeleteEntityAsync(brand);

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting brand"));

            return Ok();
        }
     
    }
}
