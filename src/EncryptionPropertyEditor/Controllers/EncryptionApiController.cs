using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Community.EncryptionPropertyEditor.Interfaces;
using Umbraco.Community.EncryptionPropertyEditor.Models;

namespace Umbraco.Community.EncryptionPropertyEditor.Controllers;
public class EncryptionApiController : UmbracoAuthorizedApiController
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

    [HttpGet]
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

    [HttpGet]
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

    [HttpGet]
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
