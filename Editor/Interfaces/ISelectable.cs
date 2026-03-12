namespace Editorias.Editor {
    public interface ISelectable {
        bool IsSelected { get; }
        bool CanBeSelected { get; }

        void Select();
        void Deselect();
        void Toggle();

        System.Action<ISelectable> OnSelection { get; set; }
    }
}