using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UserProf.Models;
using UserProf.Services;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System;

namespace UserProf.Controllers
{
   // [EnableCors("MyAllowSpecificOrigins")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: Controller{
        private readonly UserService service;
        public UserController(UserService _service){
            service = _service;
        }
         
         [AllowAnonymous]
         [Route("authenticate")]
         [HttpPost]
        public ActionResult Login([FromBody] User user){
          var token = service.Authenicate(user.Email,user.Password);
          if(token==null){
              return Unauthorized();
          }
          return Ok(new{token,user});
        }
  
        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            return service.GetUsers();
        }
         [HttpGet]
         [Route("UserDetails")]
        public ActionResult<User> GetUser()
        {
           var identity = User.Identity as ClaimsIdentity;  
         if (identity != null) {  
          IEnumerable < Claim > claims = identity.Claims;  
         }
          var getEmail = User.Claims.Where(a => a.Type == ClaimTypes.Email).FirstOrDefault().Value;  
          User user = service.GetUser(getEmail);
         return Ok(user);   
         
        }

        [HttpGet("{id:length(24)}")]

        public ActionResult<User> GetUser(string id)
        {
           var user = service.GetUser(id);
           return Json(user);
        }
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create(User user)
        {
         if(ModelState.IsValid)
          user=service.Create(user);
          else{
             return BadRequest("Some error occured");
          }
          if(user==null)
           return BadRequest("EmailId already registered");
          return Ok(Json(user));
        }

    }
}