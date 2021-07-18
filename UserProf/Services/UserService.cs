using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserProf.Database;
using UserProf.Models;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace UserProf.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> users;
        private readonly string key;

        public UserService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("HyphenDb"));
            var database = client.GetDatabase("HyphenDb");
            users=database.GetCollection<User>("Users");
            this.key= configuration.GetSection("JwtKey").ToString();
        }

        public List<User> GetUsers()=>users.Find(users=>true).ToList();
       
        public User GetUser(string email)=>users.Find<User>(user=>user.Email==email).FirstOrDefault();
        public User Create(User user){
            if(GetUser(user.Email)==null){
            users.InsertOne(user);
            Email(user);
            }
            else {user=null; return user;}
            return user;
        }  
        public string Authenicate(string email, string password)
        {
            var user= this.users.Find(x=>x.Email==email&&x.Password==password).FirstOrDefault();
           if(user==null)return null;
           var tokenHandler= new JwtSecurityTokenHandler();
           var tokenKey= Encoding.ASCII.GetBytes(key);
           var tokenDescriptor = new SecurityTokenDescriptor()
           {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName as string),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email as string),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  
                }),
                Expires=DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                    )
           };
             var token=tokenHandler.CreateToken(tokenDescriptor);
             return tokenHandler.WriteToken(token);   
           
        }       

     public void Email(User user)
       {

             try
             {
                    string body="";
                    body += "<table align ='center'>";
                    
                    body += "<tr>";
                    body += "<td align = 'right' > UserName :  </td>";
                    body += "<td >" + user.UserName + "</td>";
                    body += "</tr>";
                    body += "<tr>";
                    body += "<td align = 'right' > Email No :</td>";
                    body += "<td >" + user.Email+ " </td>";
                    body += "</tr>";
                    body += "<tr>";
                    body += "<td align = 'right' > Date :  </td>";
                    body += "<td >" + user.FullName + "</td>";
                    body += "</tr>";
                    body += "</table><br>";
                    MailMessage NewEmail = new MailMessage();
                    NewEmail.From = new MailAddress("test@gmail.com");
                    NewEmail.To.Add(new MailAddress(user.Email));
                    NewEmail.Subject = "Registration Success!! Welcome";
                    NewEmail.Body = body;
                    NewEmail.IsBodyHtml = true;
                    SmtpClient mSmtpClient = new SmtpClient();
                    // Send the mail message
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com",587);  
                   // smtp.Port = 587;  
                    //smtp.Host = "smtp.gmail.com"; //for gmail host  
                    smtp.EnableSsl = true;  
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new System.Net.NetworkCredential("anvudemy1@gmail.com","ThisisUdemy1");
                    mSmtpClient.Send(NewEmail);
                    //this.Label1.Text = "Email sent successful.";
                }
                    catch(Exception ex)
                    {
                    ex.ToString();
                    //this.Label1.Text = "Email sent failed";
                    }

             
        }

         
      




    }
}