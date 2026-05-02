using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utilitas;

namespace Editorias {
    public class GroupListDrawer<TItem> : IDrawable
        where TItem : IScrollViewItem, ISelectable, IGroupable {
        public TItem[] Items { get; protected set; }
        private string GetTitle() => "Fonts in project";

        private LabelField itemsCountLabel;

        private IScrollViewItem[] groups = Array.Empty<IScrollViewItem>();
        private readonly ScrollView scrollView = new();

        private Button refreshListButton;
        public Action OnRefreshButtonClicked = delegate { };

        private Button selectAllGroupsButton;
        private Button deselectAllGroupsButton;
        private Button expandAllGroupsButton;
        private Button collapseAllGroupsButton;

        private const string TEXT_SELECT_ALL_GROUPS = "Select all groups";
        private const string TEXT_DESELECT_ALL_GROUPS = "Deselect all groups";

        private const string EXPAND_ALL_GROUPS_TEXT = "Expand all groups";
        private const string COLLAPSE_ALL_GROUPS_TEXT = "Collapse all groups";

        public void Init(TItem[] itemsToDraw) {
            Items = itemsToDraw;

            groups = GetItemsByGroup()
                .Select(group => (IScrollViewItem)group)
                .ToArray();

            itemsCountLabel = new LabelField.Builder()
                .WithText($"{Items.Length} fonts found.")
                .Build();

            refreshListButton = new Button.Builder()
                .WithText("Refresh List")
                .WithAction(Refresh)
                .Build();

            selectAllGroupsButton = new Button.Builder()
                .WithText(TEXT_SELECT_ALL_GROUPS)
                .WithAction(SelectAllGroups)
                .Build();
            deselectAllGroupsButton = new Button.Builder()
                .WithText(TEXT_DESELECT_ALL_GROUPS)
                .WithAction(DeselectAllGroups)
                .Build();

            expandAllGroupsButton = new Button.Builder()
                .WithText(EXPAND_ALL_GROUPS_TEXT)
                .WithAction(ExpandAllGroups)
                .Build();
            collapseAllGroupsButton = new Button.Builder()
                .WithText(COLLAPSE_ALL_GROUPS_TEXT)
                .WithAction(CollapseAllGroups)
                .Build();
        }

        public void Draw() {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box,
                       GUILayout.MinWidth(EditorSizes.MIN_WIDTH),
                       GUILayout.MaxWidth(EditorSizes.MAX_WIDTH)
                   )) {
                DrawTitle(GetTitle());
                EditorGUILayout.Separator();
                DrawControls();
                EditorGUILayout.Separator();
                scrollView.DrawItems(groups);
                DrawStatus();
            }
        }

        private void DrawTitle(string title) { EditorGUILayout.LabelField(title, EditorStyles.boldLabel); }

        private void DrawControls() {
            refreshListButton.Draw();

            using (new EditorGUILayout.HorizontalScope()) {
                selectAllGroupsButton.Draw();
                deselectAllGroupsButton.Draw();
            }

            using (new EditorGUILayout.HorizontalScope()) {
                expandAllGroupsButton.Draw();
                collapseAllGroupsButton.Draw();
            }
        }

        private void DrawStatus() {
            using (new EditorGUILayout.HorizontalScope()) {
                itemsCountLabel.Draw(TextAnchor.MiddleLeft);
            }
        }

        public void Refresh() => OnRefreshButtonClicked?.Invoke();

        private void SelectAllGroups() {
            foreach (var item in groups) {
                ((ISelectable)item).Select();
            }
        }

        private void DeselectAllGroups() {
            foreach (var item in groups) {
                ((ISelectable)item).Deselect();
            }
        }

        private void ExpandAllGroups() { groups.ForEach(g => ((Group<TItem>)g).Expand()); }

        private void CollapseAllGroups() { groups.ForEach(g => ((Group<TItem>)g).Collapse()); }

        private IEnumerable<Group<TItem>> GetItemsByGroup() {
            var sortedGroups = new SortedSet<Group<TItem>>();
            var miscGroup = new Group<TItem>.Builder()
                .WithName("Misc")
                .Build();

            var groupedItems = Items
                .GroupBy(item => item.GroupingCriteria)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList()
                );

            foreach (var itemGroup in groupedItems) {
                if (itemGroup.Value.Count > 1) {
                    var group = new Group<TItem>.Builder()
                        .WithName(itemGroup.Key)
                        .WithItems(itemGroup.Value.ToArray())
                        .Build();
                    sortedGroups.Add(group);
                } else miscGroup.Items.Add(itemGroup.Value.First());
            }

            if (miscGroup.Items.Any()) sortedGroups.Add(miscGroup);
            return sortedGroups.Reverse();
        }

        public TItem[] GetSelectedItems() => Items.Filter(item => item.IsSelected).ToArray();

        public void Destroy() {
            refreshListButton.OnClick -= Refresh;
            selectAllGroupsButton.OnClick -= SelectAllGroups;
            deselectAllGroupsButton.OnClick -= DeselectAllGroups;
            expandAllGroupsButton.OnClick -= ExpandAllGroups;
            collapseAllGroupsButton.OnClick -= CollapseAllGroups;
        }
    }
}