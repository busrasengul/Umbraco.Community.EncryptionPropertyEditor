using Umbraco.Cms.Core.Manifest;

namespace Umbraco.Community.EncryptionPropertyEditor.DataType;

public class EncryptionPropertyEditorManifestFilter : IManifestFilter
{
    public void Filter(List<PackageManifest> manifests)
    {
        var assembly = typeof(EncryptionPropertyEditorManifestFilter).Assembly;

        manifests.Add(new PackageManifest
        {
            PackageName = "Encryption Property Editor ",
            Version = assembly.GetName()?.Version?.ToString(3) ?? "1.0.0",
            AllowPackageTelemetry = true,
            Scripts = new string[] {
                "/App_Plugins/EncryptionPropertyEditor/Scripts/encryptionPropertyEditor.resource.js",
                "/App_Plugins/EncryptionPropertyEditor/Scripts/encryptionPropertyEditor.controller.js"
            },
            Stylesheets = new string[]
            {
                // List any Stylesheet files
                // Urls should start '/App_Plugins/EncryptionPropertyEditor/' not '/wwwroot/EncryptionPropertyEditor/', e.g.
                // "/App_Plugins/EncryptionPropertyEditor/Styles/styles.css"
            }
        });
    }
}
