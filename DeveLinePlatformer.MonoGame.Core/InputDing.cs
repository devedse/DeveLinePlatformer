﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Linq;

namespace DeveLinePlatformer.MonoGame.Core
{
    public static class InputDing
    {
        public static MouseState CurMouse = Mouse.GetState();
        public static KeyboardState CurKey = Keyboard.GetState();
        public static MouseState PreMouse = Mouse.GetState();
        public static KeyboardState PreKey = Keyboard.GetState();
        public static TouchCollection CurTouch = TouchPanel.GetState();
        public static TouchCollection PreTouch = TouchPanel.GetState();

        public static void PreUpdate()
        {
            CurMouse = Mouse.GetState();
            CurKey = Keyboard.GetState();
            CurTouch = TouchPanel.GetState();
        }

        public static void AfterUpdate()
        {
            PreMouse = CurMouse;
            PreKey = CurKey;
            PreTouch = CurTouch;
        }

        public static Vector2 GetCurMousePos()
        {
            return new Vector2(CurMouse.X, CurMouse.Y);
        }

        public static bool KeyDownUp(Keys key)
        {
            return CurKey.IsKeyDown(key) && PreKey.IsKeyUp(key);
        }

        public static bool TouchedOrMouseClickedInRect(Rectangle rect)
        {
            if (CurMouse.LeftButton == ButtonState.Pressed && PreMouse.LeftButton == ButtonState.Released)
            {
                if (rect.Contains(CurMouse.X, CurMouse.Y))
                {
                    return true;
                }
            }
            if (CurTouch.Any(t =>
            {
                if (t.State == TouchLocationState.Pressed && PreTouch.All(z => z.Id == t.Id && t.State != TouchLocationState.Pressed))
                {
                    return rect.Contains(t.Position);
                }
                return false;
            }))
            {
                return true;
            }
            return false;
        }
    }
}
