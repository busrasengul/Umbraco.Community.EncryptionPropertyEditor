using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.EncryptionPropertyEditor.DataType;
using Umbraco.Community.EncryptionPropertyEditor.Interfaces;
using Umbraco.Community.EncryptionPropertyEditor.Models;
using Umbraco.Community.EncryptionPropertyEditor.Services;

namespace Umbraco.Community.EncryptionPropertyEditor;

public class EncryptionPropertyEditorComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.ManifestFilters().Append<EncryptionPropertyEditorManifestFilter>();
        builder.Services.AddScoped<IBackofficeUserAccessor, BackofficeUserAccessor>();
        builder.Services.Configure<EncryptionPropertyEditorSettings>(builder.Config.GetSection(EncryptionPropertyEditorSettings.SectionName));
    }
}
