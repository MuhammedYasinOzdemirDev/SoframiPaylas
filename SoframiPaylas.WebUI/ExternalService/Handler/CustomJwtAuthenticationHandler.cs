using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Security.Claims;
using FirebaseAdmin.Auth;
using SoframiPaylas.WebUI.Services.Interfaces;


public class CustomJwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAuthService _authService;
    public CustomJwtAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock, IAuthService authService)
        : base(options, logger, encoder, clock)
    {
        _authService = authService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Cookies.TryGetValue("AuthToken", out var token))
        {
            return AuthenticateResult.Fail("Token not found in cookies.");
        }


        try
        {
            // Token doğrulama işlemi
            var validatedToken = await _authService.VerifyUser(token);
            Console.WriteLine("\n\n" + validatedToken.localId + "ss\n\n");
            if (validatedToken == null)
            {
                return AuthenticateResult.Fail("Invalid Firebase token.");
            }

            // Claim'lerin oluşturulması
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, validatedToken.localId) ,
                new Claim(ClaimTypes.Email, validatedToken.Email),
                new Claim(ClaimTypes.Name, validatedToken.DisplayName),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail(ex.Message);
        }
    }
}
