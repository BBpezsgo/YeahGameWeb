using System;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using Win32;

namespace YeahGame.Web;

public partial class Program
{
    static CanvasRenderer? _canvas;
    static (int X, int Y) mousePosition;
    static MouseButton mouseButton;

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
        Game = new Game(_canvas, new WebSocketConnection<PlayerInfo>());

        Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

        Debug.WriteLine("Started");
    }

    static void OnMouse(float x, float y, int button, bool ctrlKey)
    {
        mouseButton = button switch
        {
            0 => MouseButton.Left,
            1 => MouseButton.Middle,
            2 => MouseButton.Right,
            _ => 0,
        };
        OnMouse(x, y, ctrlKey);
    }

    static void OnMouse(float x, float y, bool ctrlKey)
    {
        if (_canvas is null) return;
        x /= _canvas.PixelWidth;
        y /= _canvas.PixelHeight;
        mousePosition = ((int)x, (int)y + 1);

        Mouse.Feed(new MouseEvent(new Coord(mousePosition.X, mousePosition.Y), (uint)mouseButton, ctrlKey ? Win32.LowLevel.ControlKeyState.LEFT_CTRL_PRESSED : 0, 0));
    }

    [JSExport]
    public static void OnResize()
    {
        if (_canvas is null) return;
        _canvas.RefreshBufferSize();
    }

    [JSExport]
    public static void OnMouseDown([JSMarshalAs<JSType.Number>] float x, [JSMarshalAs<JSType.Number>] float y, [JSMarshalAs<JSType.Number>] int button, [JSMarshalAs<JSType.Boolean>] bool ctrlKey) => OnMouse(x, y, button, ctrlKey);
    [JSExport]
    public static void OnMouseUp([JSMarshalAs<JSType.Number>] float x, [JSMarshalAs<JSType.Number>] float y, [JSMarshalAs<JSType.Number>] int button, [JSMarshalAs<JSType.Boolean>] bool ctrlKey) => OnMouse(x, y, -1, ctrlKey);
    [JSExport]
    public static void OnMouseLeave([JSMarshalAs<JSType.Number>] float x, [JSMarshalAs<JSType.Number>] float y, [JSMarshalAs<JSType.Number>] int button, [JSMarshalAs<JSType.Boolean>] bool ctrlKey) => OnMouse(x, y, -1, ctrlKey);
    [JSExport]
    public static void OnMouseMove([JSMarshalAs<JSType.Number>] float x, [JSMarshalAs<JSType.Number>] float y, [JSMarshalAs<JSType.Number>] int button, [JSMarshalAs<JSType.Boolean>] bool ctrlKey) => OnMouse(x, y, ctrlKey);
    [JSExport]
    public static void OnKeyDown([JSMarshalAs<JSType.Number>] int keyCode) => Keyboard.Feed(new KeyEvent(1, 1, (ushort)keyCode, (ushort)keyCode, (char)keyCode, 0));
    [JSExport]
    public static void OnKeyUp([JSMarshalAs<JSType.Number>] int keyCode) => Keyboard.Feed(new KeyEvent(0, 0, (ushort)keyCode, (ushort)keyCode, (char)keyCode, 0));

    [JSExport]
    public static void OnAnimation([JSMarshalAs<JSType.Number>] float time)
    {
        if (_canvas is null) return;
        if (Game is null) return;

        Time.Tick();
        Mouse.Tick();
        Keyboard.Tick();

        Game.Tick();
        _canvas.Render();
    }
}
