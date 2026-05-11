using UnityEditor;
using UnityEngine;

namespace Editorias {
    public class LabelField : IDrawable {
        private string text = "Unassigned";
        private Color color = Color.white;

        protected LabelField() { }

        public void Draw() => EditorGUILayout.LabelField(text);

        public void Draw(TextAnchor textAnchor) {
            GUI.color = color;
            EditorGUILayout.LabelField(text, new GUIStyle(GUI.skin.label) {
                alignment = textAnchor
            });
            GUI.color = Color.white;
        }

        public void SetText(string value) { text = value; }

        public class Builder {
            private readonly LabelField labelField = new();

            public Builder WithText(string text) {
                labelField.text = text;
                return this;
            }

            public Builder WithColor(Color color) {
                labelField.color = color;
                return this;
            }

            public LabelField Build() { return labelField; }
        }
    }
}