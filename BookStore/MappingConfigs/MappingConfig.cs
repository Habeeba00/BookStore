using AutoMapper;
using BookStore.DTOs.Account;
using BookStore.DTOs.Adimn;
using BookStore.DTOs.Author;
using BookStore.DTOs.Book;
using BookStore.DTOs.Category;
using BookStore.DTOs.Customer;
using BookStore.DTOs.Order;
using BookStore.DTOs.OrderItems;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;

namespace BookStore.MappingConfigs
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Book, DisplayBookDTO>().ReverseMap();
            CreateMap<Book, AddBookDTO>().ReverseMap();

            CreateMap<Book, AddBookDTO>()
                .AfterMap(
                (sour, dest) =>
                {
                    dest.CategoryId = sour.category.Id;
                    dest.AuthorId = sour.author.AuthorId;

                }
                );
            //.ForMember(dest => dest.AuthorId, opt => opt.Ignore()).ForMember(dest => dest.CategoryId, opt => opt.Ignore()); // Ignore Author entity


            CreateMap<Author,DisplayAuthorDTO>().ReverseMap();
            CreateMap<Author, AddAuthorDTO>().ReverseMap();

            CreateMap<Category,DisplayCategoryDTO>().ReverseMap();
            CreateMap<Category, AddCategoryDTO>().ReverseMap();

            CreateMap<Order, DisplayOrderDTO>().ReverseMap();
            CreateMap<Order, AddOrderDTO>().ReverseMap();
            CreateMap<Order, AddOrderDTO>().AfterMap(
                (sour,dest) => 
                {
                    dest.CustomertId = sour.customer.Id;
                });


            CreateMap<Customer, AddCustomerDTO>().ReverseMap();
            CreateMap<Customer, EditCustomerDTO>().ReverseMap();
            CreateMap<Customer, DisplayCustomerDTO>().ReverseMap();

            CreateMap<Admin, AddAdminDTO>().ReverseMap();

            CreateMap<OrderItems,DisplayOrderItemsDTO>().ReverseMap();
            CreateMap<OrderItems, AddOrderItemsDTO>().ReverseMap();

            CreateMap<IdentityUser, LoginResponseDTO>().ReverseMap();









        }
    }
}
