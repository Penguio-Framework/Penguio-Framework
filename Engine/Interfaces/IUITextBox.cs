using System;

namespace Engine.Interfaces
{
    public interface IUITextBox
    {
        void Focus();
        IUIManager UIManager { get; set; }
        Rectangle Rectangle { get; set; }
        ILayoutView LayoutView { get; set; }
        Action<string> OnTextChange { get; set; }
        bool Focused { get; set; }
        void Blur();
    }
}