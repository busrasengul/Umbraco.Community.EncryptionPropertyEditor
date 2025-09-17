export const manifests: Array<UmbExtensionManifest> = [
  {
    name: "Encryption Property Editor UIEntrypoint",
    alias: "EncryptionPropertyEditor.UI.Entrypoint",
    type: "backofficeEntryPoint",
    js: () => import("./entrypoint.js"),
  },
];
