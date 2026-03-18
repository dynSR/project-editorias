using UnityEngine;

namespace Editorias {
    public class SelectableButton : IDrawable {
        public string Text { get; protected set; } = "Unassigned";
        public System.Action OnClick { get; set; } = delegate { };

        private bool isSelected;

        private GUIStyle unselectedStyle;
        private GUIStyle selectedStyle;

        protected SelectableButton() { }

        public void Draw() {
            unselectedStyle = new GUIStyle(GUI.skin.button);
            unselectedStyle.normal.background =
                SetButtonBackgroundColor(2, 2, EditorButtonColors.BackgroundDarkThemeIdle);

            selectedStyle = new GUIStyle(GUI.skin.button);
            selectedStyle.normal.background =
                SetButtonBackgroundColor(2, 2, EditorButtonColors.BackgroundDarkThemePressed);

            if (GUILayout.Button(Text, isSelected ? selectedStyle : unselectedStyle)) {
                Toggle();
                OnClick?.Invoke();
            }
        }

        private void Toggle() {
            if (!isSelected) Select();
            else Deselect();
        }

        public void Select() {
            isSelected = true;
        }

        public void Deselect() {
            isSelected = false;
        }

        private Texture2D SetButtonBackgroundColor(int width, int height, Color color) {
            var pix = new Color[width * height];
            for (var i = 0; i < pix.Length; ++i) {
                pix[i] = color;
            }

            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        public class Builder {
            private readonly SelectableButton selectableButton = new();

            public Builder WithText(string text) {
                selectableButton.Text = text;
                return this;
            }

            public Builder WithAction(System.Action onClick) {
                selectableButton.OnClick += onClick;
                return this;
            }

            public SelectableButton Build() {
                return selectableButton;
            }
        }
    }
}