using Microsoft.AspNetCore.Mvc;
using Core.Models;
using Core.Interfaces;
using Core.Specifications;
using Api.DTOs;
using AutoMapper;
using Api.Errors;
using Api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IGenericRepository<Products> _productsRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public ProductsController(IGenericRepository<Products> productsRepo,
            IGenericRepository<ProductType> productTypeRepo,
            IGenericRepository<ProductBrand> productBrandRepo,
            IMapper mapper,
            IConfiguration config)
        {
            _mapper = mapper;
            _productsRepo = productsRepo;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,
                productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<ProductToReturnDto>(product);
        }

        //brands and types actions
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var type = await _productTypeRepo.ListAllAsync();
            var typeToReturn = new List<ProductType>();

            foreach (var item in type)
            {
                typeToReturn.Add(new ProductType { Id = item.Id ,
                    Name=item.Name ,
                    PictureUrl= _config["ApiUrl"] + item.PictureUrl
                });
            }
            return Ok(typeToReturn);
        }
    
        // product crud opertions
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Products>> CreateProduct(ProductCreateDto productToCreate)
        {
            var product = _mapper.Map<ProductCreateDto, Products>(productToCreate);
            product.PictureUrl = "images/products/placeholder.png";
          
            var result = await _productsRepo.AddEntityAsync(product);

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating product"));

            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Products>> UpdateProduct(int id, ProductCreateDto productToUpdate)
        {
            var product = await _productsRepo.GetByIdAsync(id);

            _mapper.Map(productToUpdate, product);
           
            var result = await _productsRepo.UpdateEntityAsync(product);

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating product"));

            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productsRepo.GetByIdAsync(id);
          
            var result = await _productsRepo.DeleteEntityAsync(product);

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting product"));

            return Ok();
        }

        [HttpGet]
        [Route("LastAdded")]
        public async Task<List<ProductToReturnDto>> GetLastAddedProducts()
        {
            var products = await _productsRepo.GetLatestAddedProductsAsync(8);
            var data = _mapper.Map<List<ProductToReturnDto>>(products);

            return data;   
        }

        [HttpPut]
        [Route("{id}/photo")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UplaodImg(IFormFile productImg , int id)
        {
            var product = await _productsRepo.GetByIdAsync(id);

            if (productImg != null)
            {
                using (FileStream fs = new FileStream($"./wwwroot/images/products/{productImg.FileName}", FileMode.Create))
                {
                    productImg.CopyTo(fs);
                }
                product.PictureUrl = $"images/products/{productImg.FileName}";
                var result = await _productsRepo.UpdateEntityAsync(product);
                if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating image"));
            }
            return Ok();
        }
            
    }
}

