import { UMB_AUTH_CONTEXT as r } from "@umbraco-cms/backoffice/auth";
import { c as i } from "./client.gen-F4D7rlbq.js";
const a = (n, s) => {
  console.log("Hello from Encryption Property Editor Package extension 🎉 - B Loves you all!"), n.consumeContext(r, async (e) => {
    const o = e == null ? void 0 : e.getOpenApiConfiguration();
    i.setConfig({
      auth: (o == null ? void 0 : o.token) ?? void 0,
      baseUrl: (o == null ? void 0 : o.base) ?? "",
      credentials: (o == null ? void 0 : o.credentials) ?? "same-origin"
    });
  });
}, m = (n, s) => {
  console.log("Goodbye from my extension 👋");
};
export {
  a as onInit,
  m as onUnload
};
//# sourceMappingURL=entrypoint-LW5tvUwM.js.map
