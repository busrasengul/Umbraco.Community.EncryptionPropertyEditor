using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;

namespace Umbraco.Community.EncryptionPropertyEditor.DataType;

[DataEditor(
    alias: "Encryption Property Editor",
    name: "Encryption Property Editor",
    view: "~/App_Plugins/EncryptionPropertyEditor/encryptionPropertyEditor.html",
    Group = "Common",
    Icon = "icon-autofill")]
public class EncryptionPropertyEditor : DataEditor
{
    private readonly IIOHelper _ioHelper;

    public EncryptionPropertyEditor(IDataValueEditorFactory dataValueEditorFactory,
        IIOHelper ioHelper)
        : base(dataValueEditorFactory)
    {
        _ioHelper = ioHelper;
    }

    protected override IConfigurationEditor CreateConfigurationEditor()
        => new EncryptionPropertyEditorConfigurationEditor(_ioHelper);

    public override IDataValueEditor GetValueEditor(object? configuration)
    {
        var editor = base.GetValueEditor(configuration);

        if (editor is DataValueEditor valueEditor && configuration is EncryptionPropertyEditorConfiguration config)
        {
            valueEditor.HideLabel = config.HideLabel;
        }

        return editor;
    }
}
