using System;
using System.Collections.Generic;
using Bridge.Html5;
using Engine.Interfaces;

namespace Engine.Web
{
    public class WebLayout : BaseLayout
    {
        public HTMLDivElement Element { get; set; }
        public WebLayout(IScreen screen, int width, int height)
        {
            Screen = screen;
            Width = width;
            Height = height;

            ScreenOrientation = ScreenOrientation.Vertical;
            LayoutPosition = new LayoutPosition(new Size(width, height));

            Element = (HTMLDivElement)Document.CreateElement("div");

            UIManager = new WebUIManager(this);
        }
    }




    public class WebUIManager : IUIManager
    {
        public BaseLayout Layout { get; set; }
        public List<IUITextBox> TextBoxes { get; set; }

        public WebUIManager(WebLayout webLayout)
        {
            Layout = webLayout;
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
            var webUiTextBox = new WebUITextBox(this, rectangle, layoutView, onTextChange);
            TextBoxes.Add(webUiTextBox);
            return webUiTextBox;
        }

        public void ClearFocus()
        {

            foreach (var uiTextBox in TextBoxes)
            {
                uiTextBox.Blur();
            }
        }
    }

    public class WebUITextBox : IUITextBox
    {
        public WebUITextBox(IUIManager uiManager, Rectangle rectangle, BaseLayoutView layoutView, Action<string> onTextChange)
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
            UIManager.Layout.Screen.ScreenManager.Client.ClientSettings.GetKeyboardInput((text) =>
            {
                Text = text;
            });
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