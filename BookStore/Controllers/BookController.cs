using AutoMapper;
using BookStore.DTOs.Book;
using BookStore.MappingConfigs;
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
    public class BookController : ControllerBase
    {
        UnitOfWorks unit;
        IMapper mapper;
        public BookController(UnitOfWorks unit, IMapper mapper)
        {
            this.unit = unit;
           this.mapper = mapper;
            
        }

        [HttpGet]
        [SwaggerOperation(Summary ="Get all books",Description ="")]
        [SwaggerResponse(200,"Return all books",typeof(List<DisplayBookDTO>))]
        public IActionResult GetAll()
        {
            List<Book> books=unit.BookGenRepo.GetAll();
            List<DisplayBookDTO> displayBookDTOs =mapper.Map<List<DisplayBookDTO>>(books);
            return Ok(displayBookDTOs);

        }

        [HttpGet("id")]
        [SwaggerOperation (Summary = "Can search on book by book id ", Description ="")]
        [SwaggerResponse(200, "Return book data", typeof(DisplayBookDTO))]
        [SwaggerResponse(404, "If no book founded")]
        public IActionResult GetById(int id) 
        {
            Book book =unit.BookGenRepo.GetById(id);
            DisplayBookDTO displayBookDTO=mapper.Map<DisplayBookDTO>(book);
            return Ok(displayBookDTO);

        }



        [HttpPost("Add")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(
        Summary = "Add a new book (Admin Only)",
        Description = "This endpoint allows administrators to add new book. It is restricted to users with the 'admin' role.")]
        [SwaggerResponse(201, "if book created succcesfully")]
        [SwaggerResponse(400, "ifinvalid book data")]
        [SwaggerResponse(401, "Unauthorized if the user is not authenticated")]
        public IActionResult Post(AddBookDTO addBookDTO) 
        {
            if (ModelState.IsValid) 
            {
                Book book =mapper.Map<Book>(addBookDTO);
                unit.BookGenRepo.Add(book);

                string path = $"{Directory.GetCurrentDirectory()}\\Uploads\\{addBookDTO.Photo.FileName}";
                FileStream fileStream = new FileStream(path, FileMode.Create);
                book.Photo.CopyTo(fileStream);
                book.PhotoPath = path;

                unit.save();
                return Ok(addBookDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



        [HttpPut("id")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(
        Summary = "Edit book data(Admin Only)", 
        Description = "This endpoint allows administrators to edit book. It is restricted to users with the 'admin' role.")]
        [SwaggerResponse(204, "If book updated succcesfully")]
        [SwaggerResponse(400, "If invalid book data")]
        [SwaggerResponse(401, "Unauthorized if the user is not authenticated")]
        public IActionResult Edit(int id, AddBookDTO bookDTO) 
        {
            //Book book=unit.BookGenRepo.GetById(id);
            //if (book == null)
            //{
            //    return NotFound($"Book with ID {id} not found.");
            //}
            Book book = mapper.Map<Book>(bookDTO);
            book.BookId = id;
            string path = $"{Directory.GetCurrentDirectory()}\\Uploads\\{bookDTO.Photo.FileName}";
            FileStream fileStream = new FileStream(path, FileMode.Create);
            book.Photo.CopyTo(fileStream);
            book.PhotoPath = path;
            unit.BookGenRepo.Edit(book);
            unit.save();
            return Ok(bookDTO);  

        }



        [HttpDelete("id")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(
        Summary = "Delete book from datatbase (Admin Only)",
        Description = "This endpoint allows administrators to delete book . It is restricted to users with the 'admin' role.")]
        [SwaggerResponse(200, "If book deleted succcesfully")]
        [SwaggerResponse(401, "Unauthorized if the user is not authenticated")]

        public IActionResult Delete(int id) 
        {
            Book book=unit.BookGenRepo.Delete(id);
            unit.save();
            return Ok();
        }
    }

}
