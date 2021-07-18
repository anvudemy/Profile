using System.Security.Claims;
using UserProf.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using UserProf.Models;
using UserProf.Services;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace UserProfTests
{
    public class UnitTest1
    {
          [Theory]
            [InlineData("test@123.com","test123")]
        public void Test_Login(string email,string password)
        {

      var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();


          IConfiguration config = builder.Build();
            var controller = new UserController(new UserService(config));
            User user = new User();
            user.Email=email;
            user.Password=password;
            var result = controller.Login(user);
            // Assert.Equal((int)HttpStatusCode.OK,result.StatusCode);

            //controller.GetUser()
            //Console.WriteLine(result);
        }

 
    }
    }

