import { LitElement as n, html as m, css as s, property as h, customElement as v } from "@umbraco-cms/backoffice/external/lit";
import { UmbTextStyles as b } from "@umbraco-cms/backoffice/style";
import { c as p } from "./client.gen-F4D7rlbq.js";
class g {
  static ping(e) {
    return ((e == null ? void 0 : e.client) ?? p).get({
      security: [
        {
          scheme: "bearer",
          type: "http"
        }
      ],
      url: "/umbraco/encryptionproperty/api/v1",
      ...e
    });
  }
  static decrypt(e) {
    return ((e == null ? void 0 : e.client) ?? p).get({
      security: [
        {
          scheme: "bearer",
          type: "http"
        }
      ],
      url: "/umbraco/encryptionproperty/api/v1/decrypt",
      ...e
    });
  }
  static encrypt(e) {
    return ((e == null ? void 0 : e.client) ?? p).get({
      security: [
        {
          scheme: "bearer",
          type: "http"
        }
      ],
      url: "/umbraco/encryptionproperty/api/v1/encrypt",
      ...e
    });
  }
  static hash(e) {
    return ((e == null ? void 0 : e.client) ?? p).get({
      security: [
        {
          scheme: "bearer",
          type: "http"
        }
      ],
      url: "/umbraco/encryptionproperty/api/v1/hash",
      ...e
    });
  }
}
var d = Object.defineProperty, f = Object.getOwnPropertyDescriptor, i = (t, e, a, c) => {
  for (var r = c > 1 ? void 0 : c ? f(e, a) : e, l = t.length - 1, y; l >= 0; l--)
    (y = t[l]) && (r = (c ? y(e, a, r) : y(r)) || r);
  return c && r && d(e, a, r), r;
};
let u = class extends n {
  constructor() {
    super(), this.value = "", g.ping().then((t) => {
      console.log(`Ping Response: ${t.data}`);
    });
  }
  render() {
    return m`
			<uui-input
				id="suggestion-input"
				class="element"
				label="text input"
				.value=${this.value || ""}>
			</uui-input>
		`;
  }
};
u.styles = [
  b,
  s`
			#wrapper {
				margin-top: 10px;
				display: flex;
				gap: 10px;
			}
			.element {
				width: 100%;
			}
		`
];
i([
  h({ type: String })
], u.prototype, "value", 2);
u = i([
  v("encryption-property-editor-ui")
], u);
export {
  u as default
};
//# sourceMappingURL=encryptionPropertyEditor-DW2Cwy3r.js.map
