using AutoMapper;
using BookStore.DTOs.Author;
using BookStore.DTOs.Book;
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        UnitOfWorks unit;
        IMapper mapper;
        public AuthorController(UnitOfWorks unit,IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
            
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all books", Description = "")]
        [SwaggerResponse(200, "Return all books", typeof(List<DisplayAuthorDTO>))]
        public IActionResult GetAll() 
        {
            List<Author> authors = unit.AuthorGenRepo.GetAll();
            List<DisplayAuthorDTO> displayauthorDTO = mapper.Map<List<DisplayAuthorDTO>>(authors);
            return Ok(displayauthorDTO);
        }

        

        [HttpGet("id")]
        [SwaggerOperation(Summary = "Can search on book by author id ", Description = "")]
        [SwaggerResponse(200, "Return author data", typeof(DisplayAuthorDTO))]
        [SwaggerResponse(404, "If no author founded")]
        public IActionResult GetById(int id)
        {
            Author author = unit.AuthorGenRepo.GetById(id);
            DisplayAuthorDTO displayAuthorDTO = mapper.Map<DisplayAuthorDTO>(author);
            return Ok(displayAuthorDTO);

        }



        [HttpPost("Add")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(
        Summary = "Add a new author (Admin Only)",
        Description = "This endpoint allows administrators to add new authors. It is restricted to users with the 'admin' role.")]
        [SwaggerResponse(201, "if author created succcesfully")]
        [SwaggerResponse(400, "ifinvalid author data")]
        [SwaggerResponse(401, "Unauthorized if the user is not authenticated")]

        public IActionResult Post(AddAuthorDTO addAuthorDTO)
        {
            if (ModelState.IsValid)
            {
                Author author = mapper.Map<Author>(addAuthorDTO);
                unit.AuthorGenRepo.Add(author);

                unit.save();
                return Ok(addAuthorDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



        [HttpPut("id")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(
        Summary = "Edit author data(Admin Only)",
        Description = "This endpoint allows administrators to edit authors. It is restricted to users with the 'admin' role.")]
        [SwaggerResponse(204, "If author updated succcesfully")]
        [SwaggerResponse(400, "If invalid author data")]
        [SwaggerResponse(401, "Unauthorized if the user is not authenticated")]

        public IActionResult Edit(int id, AddAuthorDTO addAuthorDTO)
        {
 
            Author author = mapper.Map<Author>(addAuthorDTO);
            author.AuthorId = id;
            unit.AuthorGenRepo.Edit(author);
            unit.save();
            return Ok(addAuthorDTO);

        }



        [HttpDelete("id")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(
        Summary = "Delete author from datatbase(Admin Only)",
        Description = "This endpoint allows administrators delete authors. It is restricted to users with the 'admin' role.")]
        [SwaggerResponse(200, "If author deleted succcesfully")]
        [SwaggerResponse(401, "Unauthorized if the user is not authenticated")]

        public IActionResult Delete(int id)
        {
            Author author = unit.AuthorGenRepo.Delete(id);
            unit.save();
            return Ok();
        }


    }
}

