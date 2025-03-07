using Umbraco.Cms.Core.PropertyEditors;

namespace Umbraco.Community.EncryptionPropertyEditor.DataType;
public class EncryptionPropertyEditorConfiguration
{
    [ConfigurationField("defaultValue", "Default Value", "textarea", Description = "Provide a default value for the property")]
    public string? DefaultValue { get; set; }

    [ConfigurationField("hideLabel", "Hide Label?", "boolean", Description = "Hide the property label.")]
    public bool HideLabel { get; set; }

    [ConfigurationField("key", "Encryption Key?", "textstring", Description = "Add your 64 char encryption key.")]
    public string Key { get; set; }

    [ConfigurationField("iv", "Encryption IV?", "textstring", Description = "Add your 32 char encryption IV.")]
    public string IV { get; set; }

    [ConfigurationField("password", "Password?", "textstring", Description = "Add your password.")]
    public string Pw { get; set; }

    [ConfigurationField("salt", "Salt?", "textstring", Description = "Provide a random string of characters for hashing")]
    public string Salt { get; set; }

    [ConfigurationField("useHash", "Use Hash?", "boolean", Description = "Use this only for password fields. (Irreversable Encryption)")]
    public bool UseHash { get; set; }
}
