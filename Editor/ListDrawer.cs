using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editorias.Editor {
    public class ListDrawer<TItem> : IDrawable where TItem : IScrollViewItem {
        public TItem[] Items { get; protected set; }
        public HashSet<TItem> SelectedItems { get; } = new();

        public System.Action OnRefreshButtonClicked = delegate { };

        private readonly ScrollView<TItem> scrollView = new();

        private readonly Button selectAllButton;
        private readonly Button unselectAllButton;

        private LabelField itemsCountLabel;
        private LabelField selectedItemsCountLabel;

        private string GetTitle() => "Fonts in project";
        private float GetMaxWidth() => 300;

        public ListDrawer() {
            selectAllButton = new Button.Builder()
                .WithText("All")
                .WithAction(SelectAllItems)
                .Build();

            unselectAllButton = new Button.Builder()
                .WithText("None")
                .WithAction(UnselectAllItems)
                .Build();
        }

        public void Init(TItem[] itemsToDraw) {
            Items = itemsToDraw;

            itemsCountLabel = new LabelField.Builder()
                .WithText($"{Items.Length} fonts found.")
                .Build();
            selectedItemsCountLabel = new LabelField.Builder()
                .WithText($"{SelectedItems.Count} fonts selected.")
                .Build();

            Items.ForEach(item => item.OnSelection += UpdateSelection);
        }

        public void Draw() {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box, GUILayout.MaxWidth(GetMaxWidth()))) {
                DrawTitle(GetTitle());
                DrawControls();
                EditorGUILayout.Separator();
                DrawListItems();
                DrawStatus();
            }
        }

        private void DrawTitle(string title) {
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        }

        private void DrawControls() {
            using (new EditorGUILayout.HorizontalScope()) {
                selectAllButton.Draw();
                unselectAllButton.Draw();
            }

            if (GUILayout.Button("Refresh Font List")) {
                // Need to retrieve all items from repository on list drawer parent
                Refresh();
            }
        }

        private void DrawListItems() => scrollView.DrawItems(Items);

        private void DrawStatus() {
            using (new EditorGUILayout.HorizontalScope()) {
                itemsCountLabel.Draw();
                selectedItemsCountLabel.Draw(TextAnchor.MiddleRight);
            }
        }

        public void SelectAllItems() {
            SelectedItems.Clear();
            SelectedItems.AddRange(Items);
            SelectedItems.ForEach(item => item.Select());
            selectedItemsCountLabel.SetText($"{SelectedItems.Count} fonts selected.");
        }

        private void UnselectAllItems() {
            for (int i = SelectedItems.Count - 1; i >= 0; i--) {
                TItem item = SelectedItems.ElementAt(i);
                item.Deselect();
            }

            SelectedItems.Clear();

            selectedItemsCountLabel.SetText($"{SelectedItems.Count} fonts selected.");
        }

        private void Refresh() {
            UnselectAllItems();
            OnRefreshButtonClicked?.Invoke();
        }

        private void UpdateSelection(ISelectable selectable) {
            if (selectable.IsSelected) {
                SelectedItems.Add((TItem)selectable);
            } else {
                SelectedItems.Remove((TItem)selectable);
            }

            selectedItemsCountLabel.SetText($"{SelectedItems.Count} fonts selected.");
        }

        public void Destroy() {
            selectAllButton.OnClick -= SelectAllItems;
            unselectAllButton.OnClick -= UnselectAllItems;

            Items.ForEach(item => {
                item.OnSelection -= UpdateSelection;
                item.Destroy();
            });
        }
    }
}