using System;
using System.Collections.Generic;

namespace Engine.Interfaces
{
    public interface IUIManager
    {
        ILayout Layout { get; set; }

        bool ProcessTouchEvent(TouchType touchType, int x, int y);
        IUITextBox CreateTextBox(Rectangle rectangle, ILayoutView layoutView, Action<string> onTextChange = null);
        List<IUITextBox> TextBoxes { get; set; }
        void ClearFocus();
    }
}