using AutoMapper;
using BookStore.DTOs.Adimn;
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        UserManager<IdentityUser> UserManager;
        RoleManager<IdentityRole> RoleManager;
        UnitOfWorks unit;
        IMapper mapper;

        public AdminController(UserManager<IdentityUser> UserManager, RoleManager<IdentityRole> RoleManager, IMapper mapper, UnitOfWorks unit)
        {
            this.UserManager = UserManager;
            this.RoleManager = RoleManager;
            this.unit = unit;
            this.mapper=mapper;
            
        }

        [HttpPost("Add Admin")]
        [SwaggerOperation(Summary = "add new admin ")]
        [SwaggerResponse(201, "if admin created succcesfully")]
        [SwaggerResponse(400, "ifinvalid admin data")]
        public IActionResult Create(AddAdminDTO addAdminDTO)
        {
           
            Admin admin = mapper.Map<Admin>(addAdminDTO);
            var r = UserManager.CreateAsync(admin, addAdminDTO.Password).Result;
            if (r.Succeeded)
            {
                IdentityResult rr = UserManager.AddToRoleAsync(admin, "admin").Result;
                if (rr.Succeeded) 
                {
                    return Ok();
                }
                else 
                {
                    return BadRequest(rr.Errors);
                }
                
            }
            else 
                return BadRequest(r.Errors);
                   
        }

        //[HttpGet]
        //public IActionResult Get() 
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return Ok();
        //    }
        //    else
        //        return Unauthorized();
        //}


    }
}
