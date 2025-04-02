using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Community.EncryptionPropertyEditor.DataType;
using Umbraco.Community.EncryptionPropertyEditor.Models;
using Umbraco.Community.EncryptionPropertyEditor.NotificationHandlers;

namespace Umbraco.Community.EncryptionPropertyEditor;

public class EncryptionPropertyEditorComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.ManifestFilters().Append<EncryptionPropertyEditorManifestFilter>();
        builder.AddNotificationHandler<ServerVariablesParsingNotification, VariableParserNotificationHandler>();
        builder.Services.Configure<EncryptionPropertyEditorSettings>(builder.Config.GetSection(EncryptionPropertyEditorSettings.SectionName));
    }
}
