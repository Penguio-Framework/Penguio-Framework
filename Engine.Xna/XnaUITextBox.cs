using System;
using Engine.Interfaces;

namespace Engine.Xna
{
    public class XnaUITextBox : IUITextBox
    {
        public XnaUITextBox(IUIManager uiManager, Rectangle rectangle, BaseLayoutView layoutView, Action<string> onTextChange)
        {
            UIManager = uiManager;
            Rectangle = rectangle;
            LayoutView = layoutView;
            OnTextChange = onTextChange;
        }

        public bool Focused { get; set; }

        public IUIManager UIManager { get; set; }

        public string Text { get; set; }

        public Rectangle Rectangle { get; set; }

        public BaseLayoutView LayoutView { get; set; }

        public Action<string> OnTextChange { get; set; }

        public void Focus()
        {
            Focused = true;
            UIManager.Layout.Screen.ScreenManager.Client.ClientSettings.GetKeyboardInput((text) => { Text = text; });
//            var content = j.Result;
        }

        public void Draw()
        {
/*
            Draw text box draw blinking cursor when focused
                take in the font u want
                    on focus if applicable, open up the keyboard and move the whole layout up to y-height-idk10?
                        Capture all keyboard input, if any textbox is focused, pass it along, update OnTextChange 
*/
        }

        public void Blur()
        {
            Focused = false;
        }
    }
}