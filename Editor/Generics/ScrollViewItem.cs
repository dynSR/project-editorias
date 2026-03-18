using System;
using UnityEditor;
using UnityEngine;

namespace Editorias {
    public class ScrollViewItem : IScrollViewItem, ISelectable {
        public string Guid { get; }
        public string Name { get; }

        public bool CanBeSelected { get; }
        public bool IsSelected { get; protected set; }
        public Action<ISelectable> OnSelection { get; set; } = delegate { };

        protected readonly SelectableButton SelectableButton;

        public ScrollViewItem(string guid, string name, bool canBeSelected = true, bool isSelected = false) {
            Guid = guid;
            Name = name;
            CanBeSelected = canBeSelected;
            IsSelected = isSelected;

            SelectableButton = new SelectableButton.Builder()
                .WithText($"{Name}")
                .WithAction(Toggle)
                .Build();
        }

        public virtual void Draw() {
            EditorGUI.BeginDisabledGroup(!CanBeSelected);
            SelectableButton.Draw();
            EditorGUI.EndDisabledGroup();
        }

        public virtual void Toggle() {
            if (!IsSelected) Select();
            else Deselect();

            Debug.Log("TOGGLED");
        }

        public virtual void Select() {
            IsSelected = true;
            OnSelection?.Invoke(this);
            SelectableButton.Select();
        }

        public virtual void Deselect() {
            IsSelected = false;
            OnSelection?.Invoke(this);
            SelectableButton.Deselect();
        }

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

        public virtual void Destroy() {
            SelectableButton.OnClick -= Toggle;
        }
    }
}