using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utilitas;

namespace Editorias {
    public class Group<TItem> : ScrollViewItem, IExpandable
        where TItem : IGroupable, IScrollViewItem, ISelectable {
        public List<TItem> Items { get; } = new();
        public bool IsExpanded { get; private set; }

        private bool AreAllItemsSelected =>
            !GetSelectedItems().IsNullOrEmpty() && GetSelectedItems().Length == Items.Count;

        public override void Draw() {
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue =
                    !IsSelected && !GetSelectedItems().IsEmpty() || IsSelected && !AreAllItemsSelected;
                IsSelected = EditorGUILayout.ToggleLeft(
                    string.Empty,
                    IsSelected,
                    GUILayout.MaxWidth(EditorSizes.TOGGLE_MAX_WIDTH)
                );
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck()) Toggle();

                IsExpanded = EditorGUILayout.Foldout(
                    IsExpanded,
                    $"{Name} ({Items.Count})",
                    true
                );
            }

            if (IsExpanded) {
                EditorGUI.indentLevel++;
                foreach (var item in Items)
                    item.Draw();
                EditorGUI.indentLevel--;
            }
        }

        public override void Toggle() {
            if (IsSelected) Select();
            else Deselect();
        }

        public override void Select() {
            IsSelected = true;
            foreach (var item in Items)
                item.Select();
        }

        public override void Deselect() {
            IsSelected = false;
            foreach (var item in Items)
                item.Deselect();
        }

        public void Expand() { IsExpanded = true; }
        public void Collapse() { IsExpanded = false; }

        public TItem[] GetSelectedItems() => Items.Filter(item => item.IsSelected).ToArray();

        public override int CompareTo(object other) {
            if (other is not Group<TItem> otherGroup) {
                return -1;
            }

            int nameComparison = string.Compare(Name, otherGroup.Name, StringComparison.OrdinalIgnoreCase);
            if (nameComparison != 0) {
                return nameComparison;
            }

            return string.Compare(Guid, otherGroup.Guid, StringComparison.Ordinal);
        }

        public class Builder : Builder<Group<TItem>> {
            private bool hasIsExpandedFlagBeenSet;

            public Builder WithItems(IEnumerable<TItem> items) {
                Item.Items.AddRange(items);
                return this;
            }

            public Builder IsExpanded(bool value) {
                hasIsExpandedFlagBeenSet = true;
                Item.IsExpanded = value;
                return this;
            }

            public new Builder WithGuid(string guid) {
                base.WithGuid(guid);
                return this;
            }

            public new Builder WithName(string name) {
                base.WithName(name);
                return this;
            }


            public new Builder CanBeSelected(bool value) {
                base.CanBeSelected(value);
                return this;
            }

            public new Builder IsSelected(bool value) {
                base.IsSelected(value);
                return this;
            }

            public new Group<TItem> Build() {
                base.Build();

                if (!hasIsExpandedFlagBeenSet) Item.IsExpanded = false;
                return Item;
            }
        }
    }
}