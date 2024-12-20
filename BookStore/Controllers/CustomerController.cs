using AutoMapper;
using BookStore.DTOs.Author;
using BookStore.DTOs.Book;
using BookStore.DTOs.Customer;
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        UserManager<IdentityUser> UserManager;
        RoleManager<IdentityRole> RoleManager;
        UnitOfWorks unit;
        IMapper mapper;
        public CustomerController(UserManager<IdentityUser> UserManager, RoleManager<IdentityRole> RoleManager,IMapper mapper,UnitOfWorks unit)
        {
            this.UserManager = UserManager;
            this.RoleManager = RoleManager;
            this.mapper = mapper;
            this.unit = unit;

        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all customers", Description = "")]
        [SwaggerResponse(200, "Return all customers", typeof(List<DisplayCustomerDTO>))]
        public IActionResult GetAll()
        {
            List<Customer> customers = unit.CustomerGenRepo.GetAll();
            List<DisplayCustomerDTO> displayCustomerDTOs = mapper.Map<List<DisplayCustomerDTO>>(customers);
            return Ok(displayCustomerDTOs);

        }


        [HttpPost("AddCustomer")]
        [SwaggerOperation(Summary = "add new customer")]
        [SwaggerResponse(201, "if customer created succcesfully")]
        [SwaggerResponse(400, "ifinvalid customer data")]
        public IActionResult Create( AddCustomerDTO ct) 
        {
            Customer customer=mapper.Map<Customer>(ct);
            var r = UserManager.CreateAsync(customer,ct.Password).Result;
            DisplayCustomerDTO displayCustomerDTO = mapper.Map<DisplayCustomerDTO>(customer);
            if (r.Succeeded) return Ok(displayCustomerDTO);
            else return BadRequest(r.Errors); 
        }


        [HttpPost("EditProfile")]
        [SwaggerOperation(Summary = "Edit customer data")]
        [SwaggerResponse(204, "If customer updated succcesfully")]
        [SwaggerResponse(400, "If invalid customer data")]
        public IActionResult EditProfile(EditCustomerDTO editCustomerDTO)
        {
            if (ModelState.IsValid)
            {
                Customer customer = (Customer)UserManager.FindByIdAsync(editCustomerDTO.Id).Result;
                if (customer == null) return NotFound();
                mapper.Map<EditCustomerDTO>(customer);
                var r = UserManager.UpdateAsync(customer).Result; 

                if (r.Succeeded) return Ok(editCustomerDTO);
                else return BadRequest(r.Errors);
            }
            else 
            {
                return BadRequest(ModelState);
            }



        }
    }
}
