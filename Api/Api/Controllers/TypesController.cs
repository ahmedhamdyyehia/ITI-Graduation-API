using Api.DTOs;
using Api.Errors;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles ="Admin")]
    public class TypesController : BaseApiController
    {
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public TypesController(
            IGenericRepository<ProductType> productTypeRepo,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductType>> GetTypeById(int id)
        {
            var type = await _productTypeRepo.GetByIdAsync(id);
            return type;
        }

        [HttpPost]       
        public async Task<ActionResult<ProductType>> AddType(TypeToCreate typeToCreate)
        {
            var type = _mapper.Map<TypeToCreate, ProductType>(typeToCreate);
            type.PictureUrl = "";

            var result = await _productTypeRepo.AddEntityAsync(type);

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating category"));

            return Ok(type);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductType>> UpdateType(int id, TypeToCreate typeToUpdate)
        {
            var type = await _productTypeRepo.GetByIdAsync(id);

            _mapper.Map(typeToUpdate, type);

            var result = await _productTypeRepo.UpdateEntityAsync(type);

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating Type"));

            return Ok(type);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteType(int id)
        {
            var type = await _productTypeRepo.GetByIdAsync(id);

            var result = await _productTypeRepo.DeleteEntityAsync(type);

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting type"));

            return Ok();
        }

        [HttpPut]
        [Route("photoTypeEdit/{id}")]
        public async Task<ActionResult> UplaodTypeImg(IFormFile typeImg, int id)
        {
            var type = await _productTypeRepo.GetByIdAsync(id);

            if (typeImg != null)
            {
                using (FileStream fs = new FileStream($"./wwwroot/images/Types/{typeImg.FileName}", FileMode.Create))
                {
                    typeImg.CopyTo(fs);
                }
                type.PictureUrl = $"images/products/{typeImg.FileName}";
                var result = await _productTypeRepo.UpdateEntityAsync(type);
                if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating image"));
            }
            return Ok();
        }


    }
}
