using UnityEditor;
using UnityEngine;

namespace Editorias.Editor {
    public class ScrollView<TItem> where TItem : IScrollViewItem {
        protected Vector2 ScrollPosition;

        public void DrawItems(TItem[] items) {
            ScrollPosition = EditorGUILayout.BeginScrollView(ScrollPosition);

            for (int i = items.Length - 1; i >= 0; i--) {
                TItem item = items[i];
                item.Draw();
            }

            EditorGUILayout.EndScrollView();
        }
    }
}