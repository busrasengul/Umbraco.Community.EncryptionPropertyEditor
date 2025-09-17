const t = [
  {
    name: "Encryption Property Editor UIEntrypoint",
    alias: "EncryptionPropertyEditor.UI.Entrypoint",
    type: "backofficeEntryPoint",
    js: () => import("./entrypoint-LW5tvUwM.js")
  }
], o = {
  type: "propertyEditorUi",
  alias: "Umbraco.Community.EncryptionPropertyEditor",
  name: "Encryption Property Editor",
  element: () => import("./encryptionPropertyEditor-DW2Cwy3r.js"),
  meta: {
    label: "Encryption Property Editor",
    group: "common",
    icon: "icon-list",
    propertyEditorSchemaAlias: "Umbraco.Plain.String"
  }
}, r = [o], n = [
  ...t,
  ...r
];
export {
  n as manifests
};
//# sourceMappingURL=encryption-property-editor-ui.js.map
