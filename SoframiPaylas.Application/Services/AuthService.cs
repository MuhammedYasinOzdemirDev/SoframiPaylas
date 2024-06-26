using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using FirebaseAdmin.Auth;
using Newtonsoft.Json.Linq;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Application.DTOs.User;
using SoframiPaylas.Application.ExternalServices.Interfaces;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailsender;
        public AuthService(IAuthRepository authRepository, IMapper mapper, IEmailSender emailSender)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _emailsender = emailSender;
        }

        public async Task<bool> GetUserByUsernameAsync(string username)
        {
            return await _authRepository.GetUserByUsernameAsync(username);
        }

        public async Task<string> RegisterUserAsync(CreateUserDto userDto, string password)
        {
            var user = _mapper.Map<User>(userDto);

            var userId = await _authRepository.RegisterUserAsync(user, password);
            var link = await _authRepository.GenerateEmailVerificationLink(user.Email);
            await _emailsender.SendEmailAsync(userDto.Email, "Hesabınızı Doğrulayın",
                       $"Lütfen hesabınızı doğrulamak için <a href='{HtmlEncoder.Default.Encode(link)}'>buraya tıklayın</a>.");
            return userId;
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var signInResult = await _authRepository.SignInWithEmailAndPassword(email, password);
            var idToken = JObject.Parse(signInResult)["idToken"].ToString();
            var user = await _authRepository.GetUserDetailsAsync(idToken);

            if (!user.EmailVerified)
            {
                throw new Exception("E-posta adresiniz onaylanmamış. Lütfen e-posta adresinizi onaylayın.");
            }

            return idToken;
        }
        public async Task<FirebaseUser> VerifyUser(string idToken)
        {
            return await _authRepository.GetUserDetailsAsync(idToken);
        }
        public async Task<bool> ChangeUserPassword(ChangeUserPasswordDto dto)
        {
            var signInResult = await _authRepository.SignInWithEmailAndPassword(dto.Email, dto.OldPassword);
            if (signInResult == null)
            {
                throw new Exception("Eski şifreniz yanlış.");
            }
            if (await _authRepository.ChangeUserPassword(dto.UserId, dto.NewPassword))
            {
                return true;
            }

            return false;
        }
    }
}