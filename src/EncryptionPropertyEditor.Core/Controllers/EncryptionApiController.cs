using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Web.Common.Authorization;
using Umbraco.Cms.Web.Common.Routing;
using Umbraco.Community.EncryptionPropertyEditor.Interfaces;
using Umbraco.Community.EncryptionPropertyEditor.Models;

namespace Umbraco.Community.EncryptionPropertyEditor.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Umbraco.Community.EncryptionPropertyEditor")]
[BackOfficeRoute("encryptionproperty/api/v{version:apiVersion}")]
[Authorize(Policy = AuthorizationPolicies.SectionAccessSettings)]
[MapToApi(Constants.ApiName)]
public class EncryptionApiController : ManagementApiControllerBase
{
    private readonly IEncryptionPropertyService _encryptionPropertyService;
    private readonly IOptions<EncryptionPropertyEditorSettings> _propertySettings;

    public EncryptionApiController(IEncryptionPropertyService encryptionPropertyService, IOptions<EncryptionPropertyEditorSettings> propertySettings)
    {
        _encryptionPropertyService = encryptionPropertyService;
        _propertySettings = propertySettings;
    }

    [HttpGet]
    public bool Ping()
    {
        return true;
    }

    [HttpGet("hash")]
    public string Hash(string pw, string password, string salt)
    {
        string hashPrefix = "[[HASHED]]";

        if (password.StartsWith(hashPrefix))
        {
            return password;
        }

        if (pw == _propertySettings.Value.Password)
        {
            return _encryptionPropertyService.Hash(password, salt);
        }
        else
        {
            return "";
        }
    }

    [HttpGet("encrypt")]
    public IActionResult Encrypt(string pw, string stringData, string key, string iv, string format = "")
    {
        if (pw == _propertySettings.Value.Password)
        {
            var res = _encryptionPropertyService.Encrypt(stringData, key, iv, format?.ToLower() switch
            {
                "lower" => StringFormat.LowerCase,
                "upper" => StringFormat.UpperCase,
                "camel" => StringFormat.CamelCase,
                _ => StringFormat.Default
            });

            return string.IsNullOrWhiteSpace(res) || res.Contains("not authorised") ? this.BadRequest("Unable to encrypt") : this.Ok(res);
        }
        else
        {

            return Forbid();
        }
    }

    [HttpGet("decrypt")]
    public IActionResult Decrypt(string pw, string stringData, string key, string iv)
    {
        if (pw == _propertySettings.Value.Password)
        {
            var res = _encryptionPropertyService.Decrypt(stringData, key, iv);
            return string.IsNullOrWhiteSpace(res) || res.Contains("not authorised") ? this.BadRequest("Unable to encrypt") : this.Ok(res);

        }
        else
        {
            return Forbid();
        }
    }    
}
