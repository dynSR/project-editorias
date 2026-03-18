using System;
using System.Collections.Generic;
using UnityEditor;

namespace Editorias {
    public class Group<TItem> : IScrollViewItem where TItem : IGroupable, IScrollViewItem, ISelectable {
        public string Guid { get; }
        public string Name { get; }
        public List<TItem> Items { get; } = new();

        private bool isExpanded;

        public Group(string name, TItem[] items) {
            Guid = System.Guid.NewGuid().ToString();
            Name = name;
            Items.AddRange(items);
            isExpanded = false;
        }

        public void Draw() {
            isExpanded = EditorGUILayout.Foldout(
                isExpanded,
                $"{Name} ({Items.Count})",
                true
            );
            if (isExpanded) {
                EditorGUI.indentLevel++;
                foreach (var item in Items)
                    item.Draw();
                EditorGUI.indentLevel--;
            }
        }

        public int CompareTo(object other) {
            if (other is not Group<TItem> otherGroup) {
                return -1;
            }

            int nameComparison = string.Compare(Name, otherGroup.Name, StringComparison.OrdinalIgnoreCase);
            if (nameComparison != 0) {
                return nameComparison;
            }

            return string.Compare(Guid, otherGroup.Guid, StringComparison.Ordinal);
        }

        public void Destroy() {
            throw new System.NotImplementedException();
        }
    }
}