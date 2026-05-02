using System;
using UnityEditor;
using Utilitas;

namespace Editorias {
    public abstract class ScrollViewItem : IScrollViewItem, ISelectable {
        public string Guid { get; protected set; }
        public string Name { get; protected set; }

        public bool CanBeSelected { get; protected set; }
        public bool IsSelected { get; protected set; }

        public abstract void Draw();

        public virtual void Toggle() {
            if (!IsSelected) Select();
            else Deselect();
        }
        public virtual void Select() => IsSelected = true;
        public virtual void Deselect() => IsSelected = false;

        public virtual int CompareTo(object other) {
            if (other is not ScrollViewItem otherItem) {
                return -1;
            }

            int nameComparison = string.Compare(Name, otherItem.Name, StringComparison.OrdinalIgnoreCase);
            if (nameComparison != 0) {
                return nameComparison;
            }

            return string.Compare(Guid, otherItem.Guid, StringComparison.Ordinal);
        }

        public class Builder<T> where T : ScrollViewItem, new() {
            private bool hasCanBeSelectedFlagBeenSet;
            private bool hasSelectedFlagBeenSet;
            protected T Item { get; } = new();

            public Builder<T> WithGuid(string guid) {
                Item.Guid = !guid.IsNullOrWhiteSpace() ? guid : System.Guid.NewGuid().ToString();
                return this;
            }

            public Builder<T> WithName(string name) {
                Item.Name = !name.IsNullOrWhiteSpace() ? name : "Unassigned Name";
                return this;
            }


            public Builder<T> CanBeSelected(bool value) {
                hasCanBeSelectedFlagBeenSet = true;
                Item.CanBeSelected = value;
                return this;
            }

            public Builder<T> IsSelected(bool value) {
                hasSelectedFlagBeenSet = true;
                Item.IsSelected = value;
                return this;
            }

            public ScrollViewItem Build() {
                if (Item.Guid.IsNullOrWhiteSpace()) Item.Guid = System.Guid.NewGuid().ToString();
                if (Item.Name.IsNullOrWhiteSpace()) Item.Name = "Unassigned Name";
                if (!hasCanBeSelectedFlagBeenSet) Item.CanBeSelected = true;
                if (!hasSelectedFlagBeenSet) Item.IsSelected = false;

                return Item;
            }
        }
    }
}