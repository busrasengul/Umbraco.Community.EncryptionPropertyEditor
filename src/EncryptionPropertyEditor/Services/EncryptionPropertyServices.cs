using System.Security.Cryptography;
using System.Text;
using Umbraco.Cms.Core.Security;
using Umbraco.Community.EncryptionPropertyEditor.Interfaces;
using Umbraco.Community.EncryptionPropertyEditor.Models;

namespace Umbraco.Community.EncryptionPropertyEditor.Services;

internal class EncryptionPropertyServices : IEncryptionPropertyService
{
    private const string EncryptionPrefix = "[[ENCRYPTED]]";
    private const string HashPrefix = "[[HASHED]]";
    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

    public EncryptionPropertyServices(IBackOfficeSecurityAccessor backOfficeSecurityAccessor)
    {
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
    }

    public string Decrypt(string stringData, string key, string iv)
    {
        if (string.IsNullOrWhiteSpace(stringData) || !stringData.StartsWith(EncryptionPrefix))
        {
            return stringData;
        }

        stringData = stringData.Replace(EncryptionPrefix, string.Empty);

        string roundtrip = DecryptStringFromBytes_Aes(HexStringToByteArray(stringData), key, iv);

        return roundtrip;
    }

    public string Encrypt(string stringData, string key, string iv, StringFormat format)
    {
        if (string.IsNullOrWhiteSpace(stringData) || stringData.StartsWith(EncryptionPrefix))
        {
            return stringData;
        }

        var stringToEncrypt = format switch
        {
            StringFormat.CamelCase => ConvertToCamelCase(stringData),
            StringFormat.UpperCase => stringData.ToUpper(),
            StringFormat.LowerCase => stringData.ToLower(),
            _ => stringData
        };
        
        return $"{EncryptionPrefix}{EncryptStringToBytes_Aes(stringToEncrypt, key, iv)}";
    }

    public string Hash(string stringToHash, string salt)
    {
        // prepend stored salt to entered pw
        string combo = salt.Trim() + stringToHash.Trim();

        // get data as byte array 
        var data = Encoding.ASCII.GetBytes(combo);
        // hash
        var shadata = SHA256.HashData(data);

        // convert to string and remove any trailing whitespace
        var savedPasswordHash = ByteArrayToHexString(shadata);

        return $"{HashPrefix}{savedPasswordHash}";
    }

    private static Aes GetAes(string key, string iv)
    {
        Aes aes = Aes.Create();

        aes.Key = HexStringToByteArray(key);

        aes.IV = HexStringToByteArray(iv);

        return aes;
    }



    //note: this is not technically camelCase it is making the first character upper case, which is more TitleCase
    private static string ConvertToCamelCase(string sourceString)
    {
        if (string.IsNullOrWhiteSpace(sourceString))
        {
            return sourceString;
        }

        var stringBuilder = new StringBuilder();

        stringBuilder.Append(char.ToUpper(sourceString[0]));

        if(sourceString.Length > 1)
        {
            stringBuilder.Append(sourceString[1..]);
        }

        return stringBuilder.ToString();
    }

    private string EncryptStringToBytes_Aes(string plainText, string key, string iv)
    {
        if (_backOfficeSecurityAccessor.BackOfficeSecurity != null && _backOfficeSecurityAccessor.BackOfficeSecurity.IsAuthenticated())
        {
            // Check arguments.
            if (string.IsNullOrWhiteSpace(plainText))
            {
                throw new ArgumentNullException(nameof(plainText));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrWhiteSpace(iv))
            {
                throw new ArgumentNullException(nameof(iv));
            }

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
        if (_backOfficeSecurityAccessor.BackOfficeSecurity != null && _backOfficeSecurityAccessor.BackOfficeSecurity.IsAuthenticated())
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException(nameof(cipherText));
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (string.IsNullOrEmpty(iv))
            {
                throw new ArgumentNullException(nameof(iv));
            }

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

    private static string ByteArrayToHexString(byte[] data)
    {
        StringBuilder hex = new(data.Length * 2);
        foreach (byte b in data)
        {
            hex.AppendFormat("{0:x2}", b);
        }

        return hex.ToString();
    }

    private static byte[] HexStringToByteArray(string hex)
    {
        if (hex.Length % 2 == 1)
        {
            throw new Exception("The binary key cannot have an odd number of digits");
        }

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

}
