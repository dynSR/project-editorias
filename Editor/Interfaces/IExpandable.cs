namespace Editorias {
    public interface IExpandable {
        bool IsExpanded { get; }
        void Expand();
        void Collapse();
    }
}