using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace Umbraco.Community.EncryptionPropertyEditor.DataType;
public class EncryptionPropertyEditorConfigurationEditor : ConfigurationEditor<EncryptionPropertyEditorConfiguration>
{
    [Obsolete]
    public EncryptionPropertyEditorConfigurationEditor(IIOHelper ioHelper) : base(ioHelper)
    {
    }
}
