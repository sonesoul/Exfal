using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Exfal.InputHandling
{
    public static class Input
    {
        public static event Action<Key> KeyPressed, KeyReleased;

        public static Vector2 MousePosition { get; private set; }

        public static ref KeyboardState KeyState => ref _keyState;
        public static ref MouseState MouseState => ref _mouseState;

        private static readonly Key[] _allKeys = Enum.GetValues<Key>();
        private static readonly int _keyCount = _allKeys.Length;
                       
        private static readonly bool[] _previous = new bool[_keyCount];
                       
        private static readonly bool[] _justPressed = new bool[_keyCount];
        private static readonly bool[] _justReleased = new bool[_keyCount];

        private static readonly Dictionary<Key, int> _keyIndexMap = new();

        private static MouseState _mouseState;
        private static KeyboardState _keyState;

        private const int MouseStartIndex = 1000;

        static Input()
        {
            for (int i = 0; i < _allKeys.Length; i++)
            {
                _keyIndexMap[_allKeys[i]] = i;
            }
        }

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

                bool justPressed = !wasDown && isDown;
                bool justReleased = wasDown && !isDown;

                _justPressed[i] = justPressed;
                _justReleased[i] = justReleased;

                if (justPressed)
                    KeyPressed?.Invoke(k);

                if (justReleased)
                    KeyReleased?.Invoke(k);

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

        public static bool JustPressed(Key key) => _justPressed[GetKeyIndex(key)];
        public static bool JustReleased(Key key) => _justReleased[GetKeyIndex(key)];

        private static int GetKeyIndex(Key k)
        {
            if (!_keyIndexMap.TryGetValue(k, out var index))
            {
                throw new ArgumentOutOfRangeException($"\"{k}\" doesn't exist in the {nameof(Key)} enumeration.");
            }

            return index;
        }
    }
}