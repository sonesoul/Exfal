using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Exfal.InputHandling
{
    public static class Input
    {
        public static event Action<Key> KeyPressed, KeyReleased;

        public static Vector2 MousePosition { get; private set; }

        public static ref KeyboardState KeyState => ref _keyState;
        public static ref MouseState MouseState => ref _mouseState;

        private readonly static Key[] _allKeys = Enum.GetValues<Key>();
        private readonly static int _keyCount = _allKeys.Length;
        
        private readonly static bool[] _previous = new bool[_keyCount];

        private static MouseState _mouseState;
        private static KeyboardState _keyState;

        private const int MouseStartIndex = 1000;

        public static void Update()
        {
            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();

            MousePosition = _mouseState.Position.ToVector2();

            for (int i = 0; i < _keyCount; i++)
            {
                Key k = _allKeys[i];  
                
                bool isDown = IsKeyDown(k);
                bool wasDown = _previous[i];

                if (!wasDown && isDown)
                {
                    KeyPressed?.Invoke(k);
                }
                if (wasDown && !isDown)
                {
                    KeyReleased?.Invoke(k);
                }

                _previous[i] = isDown;
            }
        }

        public static bool IsKeyDown(Key key)
        {
            if ((int)key >= MouseStartIndex) //is it a mouse key
            {
                return IsMouseKeyDown(key);
            }
            else
            {
                return KeyState.IsKeyDown((Keys)key);
            }
        }
        private static bool IsMouseKeyDown(Key key) => key switch
        {
            Key.MouseLeft => MouseState.LeftButton == ButtonState.Pressed,
            Key.MouseRight => MouseState.RightButton == ButtonState.Pressed,
            Key.MouseMiddle => MouseState.MiddleButton == ButtonState.Pressed,
            Key.MouseX1 => MouseState.XButton1 == ButtonState.Pressed,
            Key.MouseX2 => MouseState.XButton2 == ButtonState.Pressed,
            _ => throw new InvalidOperationException($"\"{key}\" is not a mouse key or it doesn't exist in the {nameof(Key)} enumeration."),
        };
    }
}