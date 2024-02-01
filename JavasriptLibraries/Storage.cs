using System.Runtime.InteropServices.JavaScript;

namespace YeahGame.Web;

public static partial class Storage
{
    public static System.Threading.Tasks.Task<JSObject> LoadAsync()
        => JSHost.ImportAsync("storage.js", "/storage.js");

    [JSImport("clear", "storage.js")]
    public static partial void Clear();

    [JSImport("getItem", "storage.js")]
    [return: JSMarshalAs<JSType.String>]
    public static partial string? Get(
        [JSMarshalAs<JSType.String>] string key);

    [JSImport("key", "storage.js")]
    [return: JSMarshalAs<JSType.String>]
    public static partial string? Key(
        [JSMarshalAs<JSType.Number>] int index);

    [JSImport("length", "storage.js")]
    [return: JSMarshalAs<JSType.Number>]
    private static partial int length();

    public static int Length => length();

    [JSImport("removeItem", "storage.js")]
    public static partial void Remove(
        [JSMarshalAs<JSType.String>] string key);

    [JSImport("setItem", "storage.js")]
    public static partial void Set(
        [JSMarshalAs<JSType.String>] string key,
        [JSMarshalAs<JSType.String>] string value);
}
