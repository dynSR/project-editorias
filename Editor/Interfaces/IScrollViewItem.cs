using System;

namespace Editorias {
    public interface IScrollViewItem : IDrawable, IComparable {
        string Guid { get; }
        string Name { get; }

        void Destroy();
    }
}