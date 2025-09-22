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
    propertyEditorSchemaAlias: "Umb.PropertyEditorUi.TextBox",
    settings: {
      properties: [
        {
          alias: "defaultValue",
          label: "Default Value",
          description: "Provide a default value for the property",
          propertyEditorUiAlias: "Umb.PropertyEditorUi.TextBox"
        },
        {
          alias: "hideLabel",
          label: "Hide Label?",
          description: "Hide the property label.",
          propertyEditorUiAlias: "Umb.PropertyEditorUi.Toggle"
        },
        {
          alias: "key",
          label: "Encryption Key?",
          description: "Add your 64 char encryption key.",
          propertyEditorUiAlias: "Umb.PropertyEditorUi.TextBox"
        },
        {
          alias: "iv",
          label: "Encryption IV?",
          description: "Add your 32 char encryption IV.",
          propertyEditorUiAlias: "Umb.PropertyEditorUi.TextBox"
        },
        {
          alias: "password",
          label: "Password?",
          description: "Add your password.",
          propertyEditorUiAlias: "Umb.PropertyEditorUi.TextBox"
        }, {
          alias: "salt",
          label: "Salt?",
          description: "Provide a random string of characters for hashing",
          propertyEditorUiAlias: "Umb.PropertyEditorUi.TextBox"
        }, {
          alias: "useHash",
          label: "Use Hash?",
          description: "Use this only for password fields. (Irreversable Encryption).",
          propertyEditorUiAlias: "Umb.PropertyEditorUi.Toggle"
        },
      ]
    },

  },
}

export const manifests: Array<UmbExtensionManifest> = [manifest];
