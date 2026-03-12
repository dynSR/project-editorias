using System;

namespace Editorias.Editor {
    public interface IScrollViewItem : IDrawable, ISelectable, IComparable {
        string ID { get; }
        string Name { get; }

        void Destroy();
    }
}