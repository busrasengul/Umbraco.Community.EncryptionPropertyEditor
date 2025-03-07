using System.Security.Claims;

namespace Umbraco.Community.EncryptionPropertyEditor.Interfaces;
public interface IBackofficeUserAccessor
{
    ClaimsIdentity BackofficeUser { get; }
}
