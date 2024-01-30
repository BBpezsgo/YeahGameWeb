using System;
using Win32;

namespace YeahGame.Web;

public class CanvasRenderer : BufferedRenderer<ConsoleChar>
{
    ConsoleChar[] DirtyBuffer;
    ConsoleChar[] RenderedBuffer;
    int _width;
    int _height;

    static readonly string[] CharColorMap = new string[0b_1_0000]
    {
        "#000", // 0b_0000,
        "#0f3abd", // 0b_0001,
        "#0c8229", // 0b_0010,
        "#05919c", // 0b_0011,
        "#850303", // 0b_0100,
        "#99059c", // 0b_0101,
        "#b5b502", // 0b_0110,
        "#bbb", // 0b_0111,
        "#666", // 0b_1000,
        "#3570f0", // 0b_1001,
        "#54d13b", // 0b_1010,
        "#3de8eb", // 0b_1011,
        "#eb3d43", // 0b_1100,
        "#d946db", // 0b_1101,
        "#d7e657", // 0b_1110,
        "#fff", // 0b_1111,
    };

    public override short Height => (short)_height;
    public override short Width => (short)_width;

    public readonly int PixelWidth;
    public readonly int PixelHeight;

    public int DrawCalls => _drawCalls;

    public override Span<ConsoleChar> Buffer => DirtyBuffer;

    public override ref ConsoleChar this[int i] => ref DirtyBuffer[i];

    int _drawCalls;

    public CanvasRenderer(int width, int height, int pixelWidth = 1, int pixelHeight = 1)
    {
        _width = width / pixelWidth;
        _height = height / pixelHeight;

        DirtyBuffer = new ConsoleChar[_width * _height];
        RenderedBuffer = new ConsoleChar[_width * _height];

        PixelWidth = pixelWidth;
        PixelHeight = pixelHeight;
    }

    public override void Render()
    {
        bool first = false;
        _drawCalls = 0;
        byte fillColor = byte.MaxValue;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (DirtyBuffer[x + (y * _width)] == RenderedBuffer[x + (y * _width)]) continue;

                if (!first)
                {
                    Canvas.SetFont($"{PixelHeight}px Consolas, monaco, monospace");
                    _drawCalls++;
                    first = true;
                }

                ref ConsoleChar c = ref DirtyBuffer[x + (y * _width)];

                if (c.IsInvisible)
                {
                    SetFillStyle(ref fillColor, 0);
                    Canvas.FillRect(x * PixelWidth, y * PixelHeight, PixelWidth + 2, PixelHeight + 2);
                    _drawCalls++;
                    continue;
                }

                if (c.Char == Ascii.Blocks.Full)
                {
                    SetFillStyle(ref fillColor, c.Foreground);
                    Canvas.FillRect(x * PixelWidth, y * PixelHeight, PixelWidth, PixelHeight);
                    _drawCalls++;
                    continue;
                }

                if (c.Char == Ascii.Blocks.Top)
                {
                    SetFillStyle(ref fillColor, c.Foreground);
                    Canvas.FillRect(x * PixelWidth, y * PixelHeight, PixelWidth, PixelHeight / 2);
                    _drawCalls++;

                    SetFillStyle(ref fillColor, c.Background);
                    Canvas.FillRect(x * PixelWidth, (y * PixelHeight) + (PixelHeight / 2), PixelWidth, PixelHeight / 2);
                    _drawCalls++;
                    continue;
                }

                if (c.Char == Ascii.Blocks.Bottom)
                {
                    SetFillStyle(ref fillColor, c.Background);
                    Canvas.FillRect(x * PixelWidth, y * PixelHeight, PixelWidth, PixelHeight / 2);
                    _drawCalls++;

                    SetFillStyle(ref fillColor, c.Foreground);
                    Canvas.FillRect(x * PixelWidth, (y * PixelHeight) + (PixelHeight / 2), PixelWidth, PixelHeight / 2);
                    _drawCalls++;
                    continue;
                }

                SetFillStyle(ref fillColor, c.Background);
                Canvas.FillRect(x * PixelWidth, y * PixelHeight, PixelWidth, PixelHeight);
                _drawCalls++;

                if (c.Char > ' ')
                {
                    SetFillStyle(ref fillColor, c.Foreground);
                    Canvas.FillText(c.Char.ToString(), x * PixelWidth, (y + 1) * PixelHeight - 3);
                    _drawCalls++;
                }
            }
        }

        Array.Copy(DirtyBuffer, RenderedBuffer, DirtyBuffer.Length);
    }

    void SetFillStyle(ref byte currentColor, byte color)
    {
        if (currentColor == color) return;
        currentColor = color;
        _drawCalls++;

        Canvas.SetFillStyle(CharColorMap[color]);
    }

    public override void RefreshBufferSize()
    {
        _width = Canvas.Width / PixelWidth;
        _height = Canvas.Height / PixelHeight;

        RenderedBuffer = new ConsoleChar[_width * _height];
        DirtyBuffer = new ConsoleChar[_width * _height];
    }
}
