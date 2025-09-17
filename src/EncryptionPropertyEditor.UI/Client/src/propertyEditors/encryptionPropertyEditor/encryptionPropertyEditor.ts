import { css, customElement, html, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import type { UmbPropertyEditorUiElement } from '@umbraco-cms/backoffice/property-editor';
import { UmbTextStyles } from '@umbraco-cms/backoffice/style';
import { UmbracoCommunityEncryptionPropertyEditor } from "../../api";

@customElement('encryption-property-editor-ui')
export default class EncryptionPropertyEditor extends LitElement implements UmbPropertyEditorUiElement {
  @property({ type: String })
  public value = '';
  constructor() {
    super();
    UmbracoCommunityEncryptionPropertyEditor.ping()
      .then((response) => {
        console.log(`Ping Response: ${response.data}`);
      });
  }

  override render() {
    return html`
			<uui-input
				id="suggestion-input"
				class="element"
				label="text input"
				.value=${this.value || ''}>
			</uui-input>
		`;
  }

  static override readonly styles = [
    UmbTextStyles,
    css`
			#wrapper {
				margin-top: 10px;
				display: flex;
				gap: 10px;
			}
			.element {
				width: 100%;
			}
		`,
  ];
}

declare global {
  interface HTMLElementTagNameMap {
    'encryption-property-editor-ui': EncryptionPropertyEditor;
  }
}
