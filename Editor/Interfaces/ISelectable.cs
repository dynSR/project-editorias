namespace Editorias {
    public interface ISelectable {
        bool CanBeSelected { get; }
        bool IsSelected { get; }

        void Toggle();
        void Select();
        void Deselect();

        System.Action<ISelectable> OnSelection { get; set; }
    }
}