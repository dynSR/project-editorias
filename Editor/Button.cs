using UnityEngine;

namespace Editorias.Editor {
    public class Button : IDrawable {
        public string Text { get; protected set; } = "Unassigned";
        public System.Action OnClick { get; set; } = delegate { };

        protected Button() { }

        public void Draw() {
            if (GUILayout.Button(Text)) {
                OnClick?.Invoke();
            }
        }

        public class Builder {
            private readonly Button button = new();

            public Builder WithText(string text) {
                button.Text = text;
                return this;
            }

            public Builder WithAction(System.Action onClick) {
                button.OnClick += onClick;
                return this;
            }

            public Button Build() {
                return button;
            }
        }
    }
}