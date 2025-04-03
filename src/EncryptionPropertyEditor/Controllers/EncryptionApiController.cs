using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Community.EncryptionPropertyEditor.Models;

namespace Umbraco.Community.EncryptionPropertyEditor.Controllers;
public class EncryptionApiController : UmbracoAuthorizedApiController
{
    private readonly IBackOfficeSecurityAccessor _backofficeUserAccessor;
    private readonly IOptions<EncryptionPropertyEditorSettings> _propertySettings;

    public EncryptionApiController(IBackOfficeSecurityAccessor backofficeUserAccessor, IOptions<EncryptionPropertyEditorSettings> propertySettings)
    {
        _backofficeUserAccessor = backofficeUserAccessor;
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
        string savedPasswordHash = "";

        if (password.StartsWith(hashPrefix))
        {
            return password;
        }

        if (pw == _propertySettings.Value.Password)
        {
            // prepend stored salt to entered pw
            string combo = salt.Trim() + password.Trim();

            // get data as byte array 
            var data = Encoding.ASCII.GetBytes(combo);

            using (SHA256 sha256 = SHA256.Create())
            {
                // hash
                var shadata = sha256.ComputeHash(data);

                // convert to string and remove any trailing whitespace
                savedPasswordHash = ByteArrayToHexString(shadata);
            }

            return hashPrefix + savedPasswordHash;
        }
        else
        {
            return "";
        }
    }

    [HttpGet]
    public string Encrypt(string pw, string stringData, string key, string iv, string format = "")
    {
        if (pw == _propertySettings.Value.Password)
        {
            Aes aes = GetAes(key, iv);

            string encryptionPrefix = "[[ENCRYPTED]]";

            if (stringData.StartsWith(encryptionPrefix) || string.IsNullOrWhiteSpace(stringData))
            {
                return stringData;
            }

            if (format == "camel")
            {
                stringData = char.ToUpper(stringData[0]) + stringData.Substring(1).ToLower();
            }
            else if (format == "upper")
            {
                stringData = stringData.ToUpper();
            }
            else if (format == "lower")
            {
                stringData = stringData.ToLower();
            }

            string encrypted = encryptionPrefix + EncryptStringToBytes_Aes(stringData, key, iv);

            return encrypted;
        }
        else
        {
            return string.Empty;
        }
    }

    [HttpGet]
    public string Decrypt(string pw, string stringData, string key, string iv)
    {
        if (pw == _propertySettings.Value.Password)
        {
            Aes aes = GetAes(key, iv);

            string encryptionPrefix = "[[ENCRYPTED]]";

            stringData = stringData.Replace(encryptionPrefix, "");

            string roundtrip = DecryptStringFromBytes_Aes(HexStringToByteArray(stringData), key, iv);

            return roundtrip;
        }
        else
        {
            return "";
        }
    }

    private static Aes GetAes(string key, string iv)
    {
        Aes aes = Aes.Create();

        aes.Key = HexStringToByteArray(key);

        aes.IV = HexStringToByteArray(iv);

        return aes;
    }

    private static string ByteArrayToHexString(byte[] data)
    {
        StringBuilder hex = new(data.Length * 2);
        foreach (byte b in data)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }

    private static byte[] HexStringToByteArray(string hex)
    {
        if (hex.Length % 2 == 1)
            throw new Exception("The binary key cannot have an odd number of digits");

        byte[] arr = new byte[hex.Length >> 1];

        for (int i = 0; i < hex.Length >> 1; ++i)
        {
            arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));
        }

        return arr;
    }

    private static int GetHexVal(char hex)
    {
        int val = hex;
        //For uppercase A-F letters:
        //return val - (val < 58 ? 48 : 55);
        //For lowercase a-f letters:
        //return val - (val < 58 ? 48 : 87);
        //Or the two combined, but a bit slower:
        return val - (val < 58 ? 48 : val < 97 ? 55 : 87);
    }

    private string EncryptStringToBytes_Aes(string plainText, string key, string iv)
    {
        if (_backofficeUserAccessor.BackOfficeSecurity != null && _backofficeUserAccessor.BackOfficeSecurity.IsAuthenticated())
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("Key");
            if (string.IsNullOrEmpty(iv))
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            Aes aes = GetAes(key, iv);
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            // Return the encrypted bytes from the memory stream.
            return ByteArrayToHexString(encrypted);
        }
        else
        {
            return "You are not authorised to access this resource.";
        }
    }

    private string DecryptStringFromBytes_Aes(byte[] cipherText, string key, string iv)
    {
        if (_backofficeUserAccessor.BackOfficeSecurity != null && _backofficeUserAccessor.BackOfficeSecurity.IsAuthenticated())
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("Key");
            if (string.IsNullOrEmpty(iv))
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create a decrytor to perform the stream transform.
            Aes aes = GetAes(key, iv);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }

            return plaintext;
        }
        else
        {
            return "You are not authorised to access this resource.";
        }
    }
}
