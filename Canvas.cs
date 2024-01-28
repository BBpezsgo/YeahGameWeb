using System;
using System.Runtime.InteropServices.JavaScript;

namespace YeahGame.Web;

public static partial class Canvas
{
    public static System.Threading.Tasks.Task<JSObject> LoadAsync()
        => JSHost.ImportAsync("canvas.js", "/canvas.js");

    public static void SetFillStyle(Win32.Gdi32.GdiColor color)
        => Canvas.SetFillStyle($"rgb({color.R},{color.G},{color.B})");

    public static void SetFillStyle(System.Drawing.Color color)
        => Canvas.SetFillStyle($"rgb({color.R},{color.G},{color.B})");

    public static void SetFillStyle(byte r, byte g, byte b)
        => Canvas.SetFillStyle($"rgb({r},{g},{b})");

    [JSImport("fillStyle_set", "canvas.js")]
    public static partial void SetFillStyle(
        [JSMarshalAs<JSType.String>] string fillStyle);

    [JSImport("lineWidth_set", "canvas.js")]
    public static partial void SetLineWidth(
        [JSMarshalAs<JSType.Number>] float width);

    [JSImport("fillRect", "canvas.js")]
    public static partial void FillRect(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] float width,
        [JSMarshalAs<JSType.Number>] float height);

    [JSImport("strokeRect", "canvas.js")]
    public static partial void StrokeRect(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] float width,
        [JSMarshalAs<JSType.Number>] float height);

    [JSImport("clearRect", "canvas.js")]
    public static partial void ClearRect(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] float width,
        [JSMarshalAs<JSType.Number>] float height);

    [JSImport("ellipse1", "canvas.js")]
    public static partial void Ellipse(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] float radiusX,
        [JSMarshalAs<JSType.Number>] float radiusY,
        [JSMarshalAs<JSType.Number>] float rotation,
        [JSMarshalAs<JSType.Number>] float startAngle,
        [JSMarshalAs<JSType.Number>] float endAngle);

    [JSImport("ellipse2", "canvas.js")]
    public static partial void Ellipse(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] float radiusX,
        [JSMarshalAs<JSType.Number>] float radiusY,
        [JSMarshalAs<JSType.Number>] float rotation,
        [JSMarshalAs<JSType.Number>] float startAngle,
        [JSMarshalAs<JSType.Number>] float endAngle,
        [JSMarshalAs<JSType.Boolean>] bool counterclockwise);

    [JSImport("fillText1", "canvas.js")]
    public static partial void FillText(
        [JSMarshalAs<JSType.String>] string text,
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y);

    [JSImport("fillText2", "canvas.js")]
    public static partial void FillText(
        [JSMarshalAs<JSType.String>] string text,
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] float maxWidth);

    [JSImport("strokeText1", "canvas.js")]
    public static partial void StrokeText(
        [JSMarshalAs<JSType.String>] string text,
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y);

    [JSImport("strokeText2", "canvas.js")]
    public static partial void StrokeText(
        [JSMarshalAs<JSType.String>] string text,
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y,
        [JSMarshalAs<JSType.Number>] float maxWidth);

    [JSImport("beginPath", "canvas.js")]
    public static partial void BeginPath();

    [JSImport("moveTo", "canvas.js")]
    public static partial void MoveTo(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y);

    [JSImport("lineTo", "canvas.js")]
    public static partial void LineTo(
        [JSMarshalAs<JSType.Number>] float x,
        [JSMarshalAs<JSType.Number>] float y);

    [JSImport("closePath", "canvas.js")]
    public static partial void ClosePath();

    [JSImport("stroke", "canvas.js")]
    public static partial void Stroke();

    [JSImport("font_set", "canvas.js")]
    public static partial void SetFont(
        [JSMarshalAs<JSType.String>] string font);

    [JSImport("fill", "canvas.js")]
    public static partial void Fill();

    [JSImport("clear", "canvas.js")]
    public static partial void Clear();

    [JSImport("width_get", "canvas.js")]
    [return: JSMarshalAs<JSType.Number>]
    private static partial float width_get();

    [JSImport("width_set", "canvas.js")]
    private static partial float width_set([JSMarshalAs<JSType.Number>] float value);

    public static int Width
    {
        get => (int)width_get();
        set => width_set(value);
    }

    [JSImport("height_get", "canvas.js")]
    [return: JSMarshalAs<JSType.Number>]
    private static partial float height_get();

    [JSImport("height_set", "canvas.js")]
    private static partial float height_set([JSMarshalAs<JSType.Number>] float value);

    public static int Height
    {
        get => (int)height_get();
        set => height_set(value);
    }

    [JSImport("data_get", "canvas.js")]
    [return: JSMarshalAs<JSType.MemoryView>]
    private static partial ArraySegment<byte> data_get();
    [JSImport("data_set", "canvas.js")]
    private static partial void data_set([JSMarshalAs<JSType.MemoryView>] ArraySegment<byte> data);

    public static ArraySegment<byte> Data
    {
        get => data_get();
        set => data_set(value);
    }
}