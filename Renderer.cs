using System;
using System.Drawing;
using Win32;

namespace YeahGame.Web;

public class CanvasRenderer : IRenderer<ConsoleChar>
{
    ConsoleChar[] _buffer;
    byte[] _changed;
    int _width;
    int _height;

    readonly int _pixelWidth;
    readonly int _pixelHeight;

    public short Height => (short)_height;
    public short Width => (short)_width;

    public int PixelWidth => _pixelWidth;
    public int PixelHeight => _pixelHeight;

    public ref ConsoleChar this[int i]
    {
        get
        {
            _changed[i] = 1;
            return ref _buffer[i];
        }
    }

    public ConsoleChar this[int x, int y]
    {
        get => _buffer[x + (y * _width)];
        set
        {
            _buffer[x + (y * _width)] = value;
            _changed[x + (y * _width)] = 1;
        }
    }

    public CanvasRenderer(int width, int height, int pixelWidth = 1, int pixelHeight = 1)
    {
        _width = width / pixelWidth;
        _height = height / pixelHeight;
        _buffer = new ConsoleChar[_width * _height];
        _changed = new byte[_width * _height];
        _pixelWidth = pixelWidth;
        _pixelHeight = pixelHeight;
    }

    public void Render()
    {
        bool cleared = false;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                byte changed = _changed[x + (y * _width)];
                if (changed == 0) continue;

                if (!cleared)
                {
                    Canvas.SetFillStyle("black");
                    Canvas.Clear();

                    Canvas.SetFont($"{_pixelHeight}px Consolas");

                    cleared = true;
                }

                ConsoleChar c = _buffer[x + (y * _width)];

                Color bg = CharColor.GetColor(c.Background);
                Canvas.SetFillStyle($"rgb({bg.R}, {bg.G}, {bg.B})");
                Canvas.FillRect(x * _pixelWidth, (y - 1) * _pixelHeight, _pixelWidth, _pixelHeight);

                Color fg = CharColor.GetColor(c.Foreground);
                Canvas.SetFillStyle($"rgb({fg.R}, {fg.G}, {fg.B})");
                Canvas.FillText(c.Char.ToString(), x * _pixelWidth, y * _pixelHeight - 3);

                // Canvas.SetFillStyle($"rgb({color.R}, {color.G}, {color.B})");
                // Canvas.FillRect(x * _pixelSize, y * _pixelSize, _pixelSize, _pixelSize);
            }
        }
        Array.Clear(_changed);
    }

    public void ClearBuffer()
    {
        Array.Clear(_buffer);
        Array.Clear(_changed);
    }

    public void RefreshBufferSize()
    {
        int width = (int)Canvas.Width;
        int height = (int)Canvas.Height;
        _width = width / _pixelWidth;
        _height = height / _pixelHeight;
        _buffer = new ConsoleChar[_width * _height];
        _changed = new byte[_width * _height];
    }

    public void Clear(SmallRect rect)
    {
        for (int _y = 0; _y < rect.Height; _y++)
        {
            int actualY = rect.Y + _y;
            if (actualY >= Height) break;
            if (actualY < 0) continue;

            int startIndex = (actualY * _width) + Math.Max((short)0, rect.Left);
            int endIndex = (actualY * _width) + Math.Min(_width - 1, rect.Right);
            int length = Math.Max(0, endIndex - startIndex);

            Array.Clear(_buffer, startIndex, length);
            Array.Fill(_changed, (byte)1, startIndex, length);
        }
    }

    public void Fill(ConsoleChar value)
    {
        Array.Fill(_buffer, value);
        Array.Fill(_changed, (byte)1);
    }

    public void Fill(SmallRect rect, ConsoleChar value)
    {
        for (int _y = 0; _y < rect.Height; _y++)
        {
            int actualY = rect.Y + _y;
            if (actualY >= Height) break;
            if (actualY < 0) continue;

            int startIndex = (actualY * _width) + Math.Max((short)0, rect.Left);
            int endIndex = (actualY * _width) + Math.Min(_width - 1, rect.Right);
            int length = Math.Max(0, endIndex - startIndex);

            Array.Fill(_buffer, value, startIndex, length);
            Array.Fill(_changed, (byte)1, startIndex, length);
        }
    }
}
