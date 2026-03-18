using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editorias {
    public class ListDrawer<TItem> : IDrawable where TItem : IScrollViewItem, ISelectable {
        public TItem[] Items { get; protected set; }
        public HashSet<TItem> SelectedItems { get; } = new();

        public System.Action OnRefreshButtonClicked = delegate { };

        private readonly ScrollView scrollView = new();

        private readonly Button selectAllButton;
        private readonly Button unselectAllButton;
        private readonly Button refreshListButton;

        // TODO
        // - Make Filter buttons
        // - Make Sorting buttons
        // - Add a search bar

        private LabelField itemsCountLabel;
        private LabelField selectedItemsCountLabel;

        private string GetTitle() => "Fonts in project";

        public ListDrawer() {
            selectAllButton = new Button.Builder()
                .WithText("All")
                .WithAction(SelectAllItems)
                .Build();

            unselectAllButton = new Button.Builder()
                .WithText("None")
                .WithAction(UnselectAllItems)
                .Build();

            refreshListButton = new Button.Builder()
                .WithText("Refresh Fonts")
                .WithAction(Refresh)
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
            using (new EditorGUILayout.VerticalScope(GUI.skin.box,
                       GUILayout.MinWidth(EditorSizes.MIN_WIDTH),
                       GUILayout.MaxWidth(EditorSizes.MAX_WIDTH)
                   )) {
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

            refreshListButton.Draw();
        }

        private void DrawListItems() => scrollView.DrawItems(Items as IScrollViewItem[]);

        private void DrawStatus() {
            using (new EditorGUILayout.HorizontalScope()) {
                itemsCountLabel.Draw();
                selectedItemsCountLabel.Draw(TextAnchor.MiddleRight);
            }
        }

        public void SelectAllItems() {
            SelectedItems.Clear();
            SelectedItems.UnionWith(Items);
            SelectedItems.ForEach(item => item.Select());
            SetSelectedItemsCountLabelText();
        }

        public void UnselectAllItems() {
            for (int i = SelectedItems.Count - 1; i >= 0; i--) {
                TItem item = SelectedItems.ElementAt(i);
                item.Deselect();
            }

            SelectedItems.Clear();
            SetSelectedItemsCountLabelText();
        }

        public void Refresh() {
            UnselectAllItems();
            OnRefreshButtonClicked?.Invoke();
        }

        private void UpdateSelection(ISelectable selectable) {
            if (selectable.IsSelected) SelectedItems.Add((TItem)selectable);
            else SelectedItems.Remove((TItem)selectable);

            SetSelectedItemsCountLabelText();
        }

        private void SetSelectedItemsCountLabelText() =>
            selectedItemsCountLabel.SetText($"{SelectedItems.Count} fonts selected.");

        public void Destroy() {
            selectAllButton.OnClick -= SelectAllItems;
            unselectAllButton.OnClick -= UnselectAllItems;
            refreshListButton.OnClick -= Refresh;

            Items.ForEach(item => {
                item.OnSelection -= UpdateSelection;
                item.Destroy();
            });
        }
    }
}