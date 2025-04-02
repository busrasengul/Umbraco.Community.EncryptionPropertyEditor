using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Community.EncryptionPropertyEditor.Interfaces;
using Umbraco.Extensions;

namespace Umbraco.Community.EncryptionPropertyEditor.Services;
public class BackofficeUserAccessor : IBackofficeUserAccessor
{
    private readonly IOptionsSnapshot<CookieAuthenticationOptions> _cookieOptionsSnapshot;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<BackofficeUserAccessor> _logger;
    public BackofficeUserAccessor(
        IOptionsSnapshot<CookieAuthenticationOptions> cookieOptionsSnapshot,
        IHttpContextAccessor httpContextAccessor,
        ILogger<BackofficeUserAccessor> logger
    )
    {
        _cookieOptionsSnapshot = cookieOptionsSnapshot;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public ClaimsIdentity BackofficeUser
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logger.LogWarning("BackofficeAUserAccessor: HttpContext is null.");
                return new ClaimsIdentity();
            }

            CookieAuthenticationOptions cookieOptions = _cookieOptionsSnapshot.Get(Umbraco.Cms.Core.Constants.Security.BackOfficeAuthenticationType);

            string? backOfficeCookie = httpContext.Request.Cookies[cookieOptions.Cookie.Name!];
            if (string.IsNullOrEmpty(backOfficeCookie))
            {
                _logger.LogWarning("BackofficeAUserAccessor: BackOffice cookie is null or empty.");
                return new ClaimsIdentity();
            }
            AuthenticationTicket? unprotected;
            try
            {
                unprotected = cookieOptions.TicketDataFormat.Unprotect(backOfficeCookie!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BackofficeAUserAccessor: Failed to unprotect the BackOffice cookie.");
                return new ClaimsIdentity();
            }
            if (unprotected == null)
            {
                _logger.LogWarning("BackofficeAUserAccessor: Unprotected authentication ticket is null.");
                return new ClaimsIdentity();
            }
            ClaimsIdentity? backOfficeIdentity = unprotected.Principal.GetUmbracoIdentity();
            if (backOfficeIdentity == null)
            {
                _logger.LogWarning("BackofficeAUserAccessor: BackOffice identity is null.");
            }
            else
            {
                _logger.LogInformation("BackofficeAUserAccessor: User authenticated.");
            }
            return backOfficeIdentity;
        }
    }
}
