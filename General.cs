using System;
using System.Runtime.InteropServices.JavaScript;

namespace YeahGame.Web;

public static partial class General
{
    public static System.Threading.Tasks.Task<JSObject> LoadAsync()
        => JSHost.ImportAsync("general.js", "/general.js");

    [JSImport("window_innerWidth_get", "general.js")]
    [return: JSMarshalAs<JSType.Number>]
    private static partial float window_innerWidth_get();

    [JSImport("window_innerHeight_get", "general.js")]
    [return: JSMarshalAs<JSType.Number>]
    private static partial float window_innerHeight_get();

    [JSImport("window_outerWidth_get", "general.js")]
    [return: JSMarshalAs<JSType.Number>]
    private static partial float window_outerWidth_get();

    [JSImport("window_outerHeight_get", "general.js")]
    [return: JSMarshalAs<JSType.Number>]
    private static partial float window_outerHeight_get();

    [JSImport("window_location_get", "general.js")]
    [return: JSMarshalAs<JSType.String>]
    private static partial string window_location_get();

    [JSImport("prompt", "general.js")]
    [return: JSMarshalAs<JSType.String>]
    public static partial string? Prompt(string? message, string? _default);

    public static int WindowInnerWidth => (int)window_innerWidth_get();
    public static int WindowInnerHeight => (int)window_innerHeight_get();
    public static int WindowOuterWidth => (int)window_outerWidth_get();
    public static int WindowOuterHeight => (int)window_outerHeight_get();

    public static Uri Location => new(window_location_get());
}
