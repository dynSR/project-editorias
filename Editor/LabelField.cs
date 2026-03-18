using UnityEditor;
using UnityEngine;

namespace Editorias {
    public class LabelField : IDrawable {
        public string Text { get; protected set; } = "Unassigned";

        public void Draw() => EditorGUILayout.LabelField(Text);

        public void Draw(TextAnchor textAnchor) => EditorGUILayout.LabelField(Text, new GUIStyle(GUI.skin.label) {
            alignment = textAnchor
        });

        public void SetText(string value) {
            Text = value;
        }

        public class Builder {
            private readonly LabelField labelField = new();

            public Builder WithText(string text) {
                labelField.Text = text;
                return this;
            }

            public LabelField Build() {
                return labelField;
            }
        }
    }
}