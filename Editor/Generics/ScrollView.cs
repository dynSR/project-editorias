using UnityEditor;
using UnityEngine;

namespace Editorias {
    public class ScrollView {
        protected Vector2 ScrollPosition;

        public void DrawItems(IScrollViewItem[] items) {
            ScrollPosition = EditorGUILayout.BeginScrollView(ScrollPosition);

            for (int i = items.Length - 1; i >= 0; i--)
                items[i].Draw();

            EditorGUILayout.EndScrollView();
        }
    }
}