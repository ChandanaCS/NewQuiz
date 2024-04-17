using AutoMapper;
using BLL;
using DAL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            AutoMapperConfigAPI.Initialize();
            _mapper = AutoMapperConfigAPI.Mapper;
            _userService = userService;
        }
        [HttpPost("RegisterForm")]
        [EnableCors("AllowReactApp")]
        public IActionResult RegisterNewUser(String Name, String EmailId, String Password, String OrganisationName
            , string PhoneNo)
        {
            try
            {
                bool IsUserAlreadyExist = _userService.DoesUserExist(EmailId);
                if(IsUserAlreadyExist){
                    return Ok("EmailId already exist....!! Try Loging In");
                }
                var userDTO = new UserDTO
                {
                    Name = Name,
                    EmailId = EmailId,
                    Password = Password,
                    PhoneNumber = PhoneNo
                };
                Organisation Organisation = _userService.GetOrganisationByName(OrganisationName);
                if(Organisation == null)
                {
                    int id = _userService.AddOrganisationName(OrganisationName);
                    userDTO.OrganisationId = id;
                }
                else 
                {
                    userDTO.OrganisationId = Organisation.Id;
                }
                
                //userDTO.IsActive = true;
                //userDTO.RoleId = 3;
                bool result = _userService.RegisterNewUser(userDTO);
                if (result)
                {
                    List<string> adminEmails = _userService.GetAdminEmailsByOrganisation(OrganisationName);

                    // Send email to each admin
                    foreach (var adminEmail in adminEmails)
                    {
                        SendNewUserAlertToAdmin(adminEmail, Name, EmailId, OrganisationName);
                    }
                    return Ok("Data Inserted");
                }
                else
                {
                    return BadRequest("Something went wrong try again ");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetOrganisation")]
        public List<OrganisationModel> GetOrganisation()
        {
            List<OrganisationDTO> organisationDTO = _userService.GetOrganisations();
            List<OrganisationModel> organisationModels = _mapper.Map<List<OrganisationModel>>(organisationDTO);
            return organisationModels;

        }

        [HttpPost("forgotpassword")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                //string EmailID = email;

                // Generate OTP (You can use any logic to generate OTP)
                Random random = new Random();
                string otp = random.Next(100000, 999999).ToString();

                // Send OTP to email
                SendEmail(email, otp);

                // Store the OTP in session
                HttpContext.Session.SetString("OTP", otp); // Assuming you're using ASP.NET Core and have session configured


                return Ok(new { message = "OTP sent successfully", otp });
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to send OTP: " + ex.Message);
            }
        }

        private void SendEmail(string email, string otp)
        {
            // Configure SMTP client (Update with your SMTP server details)
            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("mail-id", "password"),
                EnableSsl = true
            };

            // Create email message
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("mail-id"),
                Subject = "OTP for Password Reset",
                Body = $"Your OTP is: {otp}"
            };

            mailMessage.To.Add(email);

            // Send email
            smtpClient.Send(mailMessage);
        }

        private void SendNewUserAlertToAdmin(string adminEmail,string Name,string EmailId,string OrganisationName)
        {
            // Configure SMTP client (Update with your SMTP server details)
            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("mail-id", "password"),
                EnableSsl = true
            };

            // Create email message
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("mail-id"),
                Subject = "New User Registration",
                Body = $"Dear Admin,\n\nA new user has registered with the following details:\n\nName: {Name}\nEmail: {EmailId}\nOrganisation: {OrganisationName}\n\nPlease take necessary action.\n\nRegards,\nYour App Team"
            };

            mailMessage.To.Add(adminEmail);

            // Send email
            smtpClient.Send(mailMessage);
        }

        [HttpPost("validateOTP")]
        public IActionResult VerifyOTP(string otp)
        {
            string storedOTP = HttpContext.Session.GetString("OTP");

            if (otp == storedOTP)
            {
                return Ok("Valid OTP");
            }
            else
            {
                return BadRequest("Invalid OTP");
            }
        }

        [HttpPost("resetpassword")]
        public IActionResult ResetPassword(string email, string newPassword)
        {
            try
            {
                //string email = data.email;
                //string newPassword = data.newPassword;

                if (_userService.UpdatePassword(email, newPassword))
                { 
                    return Ok("Password updated successfully.");
                }
                else
                {
                    return BadRequest("Failed to reset password");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to reset password: " + ex.Message);
            }
        }
    }
}
