import { ManifestPropertyEditorUi } from '@umbraco-cms/backoffice/property-editor';

const manifest: ManifestPropertyEditorUi = {
  type: 'propertyEditorUi',
  alias: 'Umbraco.Community.EncryptionPropertyEditor',
  name: 'Encryption Property Editor',
  element: () => import('./encryptionPropertyEditor'),
  meta: {
    label: "Encryption Property Editor",
    group: 'common',
    icon: "icon-list",
    propertyEditorSchemaAlias: "Umbraco.Plain.String"
  },
}

export const manifests: Array<UmbExtensionManifest> = [manifest];
