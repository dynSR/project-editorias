using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editorias {
    // TODO
    // - Add selection
    // - Add buttons to select all groups
    // - Add buttons to select all group elements
    // - Add buttons to refresh content
    // - Add buttons to expand / reduce everything

    public class GroupListDrawer<TItem> : IDrawable where TItem : IScrollViewItem, ISelectable, IGroupable {
        public TItem[] Items { get; protected set; }
        private string GetTitle() => "Fonts in project";

        private bool allSelected;
        private bool isExpanded;
        private LabelField itemsCountLabel;

        private IScrollViewItem[] scrollItems = Array.Empty<IScrollViewItem>();
        private readonly ScrollView scrollView = new();

        public void Init(TItem[] itemsToDraw) {
            Items = itemsToDraw;

            scrollItems = GetItemsByGroup()
                .Select(group => (IScrollViewItem)group)
                .ToArray();

            itemsCountLabel = new LabelField.Builder()
                .WithText($"{Items.Length} fonts found.")
                .Build();
        }

        public void Draw() {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box,
                       GUILayout.MinWidth(EditorSizes.MIN_WIDTH),
                       GUILayout.MaxWidth(EditorSizes.MAX_WIDTH)
                   )) {
                DrawTitle(GetTitle());
                scrollView.DrawItems(scrollItems);
                DrawStatus();
            }
        }

        private HashSet<Group<TItem>> GetItemsByGroup() {
            var groups = new HashSet<Group<TItem>>();
            var miscGroup = new Group<TItem>("Misc", Array.Empty<TItem>());

            var groupedItems = Items
                .GroupBy(item => item.GroupingCriteria)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList()
                );

            foreach (var itemGroup in groupedItems) {
                if (itemGroup.Value.Count > 1) {
                    var group = new Group<TItem>(itemGroup.Key, itemGroup.Value.ToArray());
                    groups.Add(group);
                } else miscGroup.Items.Add(itemGroup.Value.First());
            }

            if (miscGroup.Items.Any()) groups.Add(miscGroup);

            Debug.Log($"Group of fonts {groups.Count}");
            return groups;
        }

        private void DrawTitle(string title) {
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        }


        private void DrawStatus() {
            using (new EditorGUILayout.HorizontalScope()) {
                itemsCountLabel.Draw(TextAnchor.MiddleLeft);
            }
        }

        public int GetTotalItemCount() => Items.Length;

        public void Destroy() { }
    }
}