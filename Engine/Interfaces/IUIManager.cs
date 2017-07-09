using System;
using System.Collections.Generic;

namespace Engine.Interfaces
{
    public interface IUIManager
    {
        BaseLayout Layout { get; set; }

        bool ProcessTouchEvent(TouchType touchType, int x, int y);
        IUITextBox CreateTextBox(Rectangle rectangle, BaseLayoutView layoutView, Action<string> onTextChange = null);
        List<IUITextBox> TextBoxes { get; set; }
        void ClearFocus();
    }
}