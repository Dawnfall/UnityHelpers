using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.HelperEditor
{
    public static class HelperEditorOnGUI
    {
        public static bool IsUsedEvent
        {
            get { return Event.current.type == EventType.Used; }
            set { if (value == true) Event.current.Use(); }
        }
        public static bool IsMouseReleased(int button)
        {
            return Event.current.type == EventType.MouseUp && Event.current.button == button;
        }
        public static bool IsMousePressed(int button)
        {
            return Event.current.type == EventType.MouseDown && Event.current.button == button;
        }
        public static bool IsKeyPressed(KeyCode keyCode)
        {
            return Event.current.type == EventType.KeyDown && Event.current.keyCode == keyCode;
        }
        public static bool IsKeyReleased(KeyCode keyCode)
        {
            return (Event.current.type == EventType.KeyUp && Event.current.keyCode == keyCode);
        }
        public static Vector2 CurrMousePos()
        {
            return Event.current.mousePosition;
        }
        public static Vector2 GetMouseDragDelta(int button)
        {
            if (Event.current.type == EventType.MouseDrag && Event.current.button == button)
                return Event.current.delta;
            return Vector2.zero;
        }
        public static float GetMouseWheelDelta()
        {
            return (Event.current.type == EventType.ScrollWheel) ? Event.current.delta.y : 0;
        }
        public static bool DetectInRect(Rect rect, bool doUse = true)
        {
            if (rect.Contains(Event.current.mousePosition))
            {
                if (doUse)
                    Event.current.Use();
                return true;
            }
            return false;
        }

    }
}