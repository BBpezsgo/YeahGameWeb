using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.JavaScript;
using Win32;
using Win32.Common;
using Win32.LowLevel;

namespace YeahGame.Web;

public partial class Program
{
    static CanvasRenderer? _canvas;
    static Coord mousePosition;
    static MouseButton mouseButton;
    static ControlKeyState ctrlKeys;
    static bool wasResized;

    static Game? Game;

    public static void Main(string[] args)
    {
        // {
        //     StringBuilder b = new("args: ");
        //     for (int i = 0; i < args.Length; i++)
        //     {
        //         if (i > 0) b.Append(", ");
        //         b.Append('\"');
        //         b.Append(args[i]);
        //         b.Append('\"');
        //     }
        //     Console.WriteLine(b.ToString());
        // }

        _canvas = new CanvasRenderer(Canvas.Width, Canvas.Height, 8, 16);
        Game = new Game(_canvas);

        Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

        Debug.WriteLine("Started");
    }

    static ControlKeyState GetCtrlKeyState(bool altKey, bool ctrlKey, bool shiftKey)
    {
        ControlKeyState result = 0;

        if (altKey)
        {
            result |= ControlKeyState.LEFT_ALT_PRESSED;
            result |= ControlKeyState.RIGHT_ALT_PRESSED;
        }

        if (ctrlKey)
        {
            result |= ControlKeyState.LEFT_CTRL_PRESSED;
            result |= ControlKeyState.RIGHT_CTRL_PRESSED;
        }

        if (shiftKey)
        {
            result |= ControlKeyState.SHIFT_PRESSED;
        }

        return result;
    }

    static MouseButton GetMouseButton(int button) => button switch
    {
        0 => MouseButton.Left,
        1 => MouseButton.Middle,
        2 => MouseButton.Right,
        3 => MouseButton.Button3,
        4 => MouseButton.Button4,
        _ => 0,
    };

    static Coord GetMousePosition(float x, float y)
    {
        if (_canvas is null) return default;
        x /= _canvas.PixelWidth;
        y /= _canvas.PixelHeight;
        return new Coord((int)x, (int)y);
    }

    [JSExport]
    public static void OnResize()
    {
        wasResized = true;
    }

    [JSExport]
    public static void OnMouseDown(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] int button,
        [JSMarshalAs<JSType.Boolean>] bool altKey,
        [JSMarshalAs<JSType.Boolean>] bool ctrlKey,
        [JSMarshalAs<JSType.Boolean>] bool shiftKey)
    {
        mouseButton |= GetMouseButton(button);
        ctrlKeys = GetCtrlKeyState(altKey, ctrlKey, shiftKey);
        mousePosition = GetMousePosition(x, y);
        Mouse.Feed(new MouseEvent(mousePosition, (uint)mouseButton, ctrlKeys, 0));
    }

    [JSExport]
    public static void OnMouseUp(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] int button,
        [JSMarshalAs<JSType.Boolean>] bool altKey,
        [JSMarshalAs<JSType.Boolean>] bool ctrlKey,
        [JSMarshalAs<JSType.Boolean>] bool shiftKey)
    {
        mouseButton &= ~GetMouseButton(button);
        ctrlKeys = GetCtrlKeyState(altKey, ctrlKey, shiftKey);
        mousePosition = GetMousePosition(x, y);
        Mouse.Feed(new MouseEvent(mousePosition, (uint)mouseButton, ctrlKeys, 0));
    }

    [JSExport]
    public static void OnMouseLeave(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] int _,
        [JSMarshalAs<JSType.Boolean>] bool altKey,
        [JSMarshalAs<JSType.Boolean>] bool ctrlKey,
        [JSMarshalAs<JSType.Boolean>] bool shiftKey)
    {
        mouseButton = 0;
        ctrlKeys = GetCtrlKeyState(altKey, ctrlKey, shiftKey);
        mousePosition = GetMousePosition(x, y);
        Mouse.Feed(new MouseEvent(mousePosition, (uint)mouseButton, ctrlKeys, MouseEventFlags.MouseMoved));
    }

    [JSExport]
    public static void OnMouseMove(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] int _,
        [JSMarshalAs<JSType.Boolean>] bool altKey,
        [JSMarshalAs<JSType.Boolean>] bool ctrlKey,
        [JSMarshalAs<JSType.Boolean>] bool shiftKey)
    {
        ctrlKeys = GetCtrlKeyState(altKey, ctrlKey, shiftKey);
        mousePosition = GetMousePosition(x, y);
        Mouse.Feed(new MouseEvent(mousePosition, (uint)mouseButton, ctrlKeys, MouseEventFlags.MouseMoved));
    }

    [JSExport]
    public static void OnTouch(
        [JSMarshalAs<JSType.Array<JSType.Number>>] double[] xs,
        [JSMarshalAs<JSType.Array<JSType.Number>>] double[] ys,
        [JSMarshalAs<JSType.Array<JSType.Number>>] int[] ids)
    {
        Dictionary<int, Point> touches = Touch.UnsafeGetTouches();
        Touch.UnsafeSetIsTouchDevice(true);

        touches.Clear();
        for (int i = 0; i < ids.Length; i++)
        { touches[ids[i]] = GetMousePosition((float)xs[i], (float)ys[i]); }

        if (touches.TryGetValue(0, out Point touchPosition))
        {
            mousePosition = (Coord)touchPosition;
            mouseButton = MouseButton.Left;
            Mouse.Feed(new MouseEvent(mousePosition, (uint)mouseButton, ctrlKeys, MouseEventFlags.MouseMoved));
        }
        else if (mouseButton != 0)
        {
            mouseButton = 0;
            Mouse.Feed(new MouseEvent(mousePosition, (uint)mouseButton, ctrlKeys, 0));
        }

        // KeyValuePair<int, Coord>[] array = Touches.ToArray();
        // foreach ((int id, Coord position) in array)
        // {
        //     if (Array.IndexOf(ids, id) == -1)
        //     {
        //         OnTouchUp(id, position);
        //         Touches.Remove(id);
        //     }
        // }
        // 
        // for (int i = 0; i < ids.Length; i++)
        // {
        //     Coord position = GetMousePosition((float)xs[i], (float)ys[i]);
        //     if (!Touches.ContainsKey(ids[i]))
        //     { OnTouchDown(ids[i], position); }
        //     else
        //     { OnTouchMove(ids[i], position); }
        //     Touches[ids[i]] = position;
        // }
    }

    // static void OnTouchDown(int id, Coord position)
    // {
    // 
    // }
    // 
    // static void OnTouchUp(int id, Coord position)
    // {
    // 
    // }
    // 
    // static void OnTouchMove(int id, Coord position)
    // {
    // 
    // }

    [JSExport]
    public static void OnWheel(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] float delta,
        [JSMarshalAs<JSType.Boolean>] bool altKey,
        [JSMarshalAs<JSType.Boolean>] bool ctrlKey,
        [JSMarshalAs<JSType.Boolean>] bool shiftKey)
    {
        ctrlKeys = GetCtrlKeyState(altKey, ctrlKey, shiftKey);
        mousePosition = GetMousePosition(x, y);
        Mouse.Feed(new MouseEvent(mousePosition, Macros.MAKELONG(unchecked((ushort)(short)delta), (ushort)mouseButton), ctrlKeys, MouseEventFlags.MouseWheeled));
    }

    [JSExport]
    public static void OnKeyDown(
        [JSMarshalAs<JSType.Number>] int keyCode,
        [JSMarshalAs<JSType.String>] string key,
        [JSMarshalAs<JSType.Number>] int location,
        [JSMarshalAs<JSType.Boolean>] bool altKey,
        [JSMarshalAs<JSType.Boolean>] bool ctrlKey,
        [JSMarshalAs<JSType.Boolean>] bool shiftKey)
    {
        ctrlKeys = GetCtrlKeyState(altKey, ctrlKey, shiftKey);
        Keyboard.Feed(new KeyEvent(1, 1, (VirtualKeyCode)keyCode, (ushort)keyCode, key[0], ctrlKeys));
    }

    [JSExport]
    public static void OnKeyUp(
        [JSMarshalAs<JSType.Number>] int keyCode,
        [JSMarshalAs<JSType.String>] string key,
        [JSMarshalAs<JSType.Number>] int location,
        [JSMarshalAs<JSType.Boolean>] bool altKey,
        [JSMarshalAs<JSType.Boolean>] bool ctrlKey,
        [JSMarshalAs<JSType.Boolean>] bool shiftKey)
    {
        ctrlKeys = GetCtrlKeyState(altKey, ctrlKey, shiftKey);
        Keyboard.Feed(new KeyEvent(0, 0, (VirtualKeyCode)keyCode, (ushort)keyCode, key[0], ctrlKeys));
    }

    [JSExport]
    public static void OnAnimation()
    {
        if (_canvas is null) return;
        if (Game is null) return;

        Time.Tick();
        Mouse.Tick();
        Keyboard.Tick();

        if (wasResized)
        {
            _canvas.RefreshBufferSize();
            wasResized = false;
            Game.OnResized();
        }
        else
        {
            _canvas.Clear();
        }

        // foreach ((int id, Coord position) in Touches)
        // {
        //     if (_canvas.IsVisible(position))
        //     { _canvas[position] = new ConsoleChar('X', CharColor.White); }
        // }
        // 
        // if (_canvas.IsVisible(mousePosition))
        // { _canvas[mousePosition] = new ConsoleChar('X', CharColor.BrightYellow); }

        Game.Tick();

        // if (_canvas.IsVisible(Mouse.RecordedConsolePosition))
        // {
        //     _canvas[Mouse.RecordedConsolePosition] = new ConsoleChar('X', CharColor.White);
        // }
        // 
        // if (_canvas.IsVisible(Mouse.LeftPressedAt))
        // {
        //     _canvas[Mouse.LeftPressedAt] = new ConsoleChar('X', CharColor.BrightBlue);
        // }

        _canvas.Render();

        // Console.WriteLine(_canvas.DrawCalls);
    }
}
