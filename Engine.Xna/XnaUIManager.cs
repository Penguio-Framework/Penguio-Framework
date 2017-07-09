using System;
using System.Collections.Generic;
using Engine.Interfaces;

namespace Engine.Xna
{
    public class XnaUIManager : IUIManager
    {
        public BaseLayout Layout { get; set; }
        public List<IUITextBox> TextBoxes { get; set; }

        public XnaUIManager(XnaLayout xnaLayout)
        {
            Layout = xnaLayout;
            TextBoxes = new List<IUITextBox>();
        }

        public bool ProcessTouchEvent(TouchType touchType, int x, int y)
        {
            foreach (var layout in Layout.Screen.Layouts)
            {
                layout.UIManager.ClearFocus();
            }

            foreach (var uiTextBox in TextBoxes)
            {
                if (uiTextBox.Rectangle.Contains(x, y))
                {
                    uiTextBox.Focus();

                    return true;
                }
            }
            return false;
        }

        public IUITextBox CreateTextBox(Rectangle rectangle, BaseLayoutView layoutView, Action<string> onTextChange = null)
        {
            var xnaUiTextBox = new XnaUITextBox(this, rectangle, layoutView, onTextChange);
            TextBoxes.Add(xnaUiTextBox);
            return xnaUiTextBox;
        }

        public void ClearFocus()
        {
            foreach (var uiTextBox in TextBoxes)
            {
                uiTextBox.Blur();
            }
        }
    }
}