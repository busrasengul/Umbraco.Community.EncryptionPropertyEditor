# Umbraco.Community.EncrytionPropertyEditor 

[![Downloads](https://img.shields.io/nuget/dt/Umbraco.Community.Umbraco.Community.EncrytionPropertyEditor?color=cc9900)](https://www.nuget.org/packages/Umbraco.Community.Umbraco.Community.EncrytionPropertyEditor/)
[![NuGet](https://img.shields.io/nuget/vpre/Umbraco.Community.Umbraco.Community.EncrytionPropertyEditor?color=0273B3)](https://www.nuget.org/packages/Umbraco.Community.Umbraco.Community.EncrytionPropertyEditor)
[![GitHub license](https://img.shields.io/github/license/busrasengul/Umbraco.Community.EncryptionPropertyEditor?color=8AB803)](../LICENSE)

## Installation

Add the package to an existing Umbraco website (v13+) from nuget:

`dotnet add package Umbraco.Community.EncrytionPropertyEditor`

### Install the Package:

Open your Umbraco project in Visual Studio.
Right-click on your solution and select Manage NuGet Packages.
Click on Browse and search for EncryptionPropertyEditor.
Install the package.

### Configure the Property Editor:

In the Umbraco backoffice, navigate to Settings.
Under Data Types, create a new data type and select Encryption Property Editor as the editor.
Save the data type.
In the data type configuration, set the encryption key and algorithm as per your requirements.

### Use the Property Editor:

Add the newly created data type to your document types.
Use it in your content nodes to encrypt data.

### Usage
The Encryption Property Editor allows you to securely encrypt data entered in the Umbraco backoffice.
If the Backoffice user is allowed to see the encrypted value, they can view it in the backoffice.
Note that if you use hash, the value will not be decryptable, and the original value cannot be retrieved.
Ensure you have the necessary encryption keys configured in your appsettings.json.

```
"EncryptionPropertyEditor": {
  "Password" :  "your-password"
}
```

## Contributing

Contributions to this package are most welcome! Please read the [Contributing Guidelines](CONTRIBUTING.md).

## Acknowledgments

A massive thank you to [Nik Rimington](https://github.com/NikRimington) for his most valuable contributions!
