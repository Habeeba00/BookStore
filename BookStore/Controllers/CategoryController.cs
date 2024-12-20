using AutoMapper;
using BookStore.DTOs.Book;
using BookStore.DTOs.Category;
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        UnitOfWorks unit;
        IMapper mapper;
        public CategoryController(UnitOfWorks unit,IMapper mapper )
        {
            this.unit = unit;
            this.mapper = mapper;
            
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all categories", Description = "")]
        [SwaggerResponse(200, "Return all categories", typeof(List<DisplayCategoryDTO>))]
        public IActionResult GetAll() 
        {
            List<Category> categories=unit.CategoryGenRepo.GetAll();
            List<DisplayCategoryDTO> displayCategoryDTOs = mapper.Map<List<DisplayCategoryDTO>>(categories);
            return Ok(displayCategoryDTOs);
        }

        [HttpPost("Add")]
        [SwaggerOperation(Summary = "add new category")]
        [SwaggerResponse(201, "if category created succcesfully")]
        [SwaggerResponse(400, "ifinvalid category data")]
        public IActionResult Post(AddCategoryDTO addCategoryDTO) 
        {
            if (ModelState.IsValid)
            {
                Category category = mapper.Map<Category>(addCategoryDTO);
                unit.CategoryGenRepo.Add(category);
                unit.save();
                return Ok(addCategoryDTO);
            }
            else 
            {
                return BadRequest(ModelState);
            }
        }
    }
}
