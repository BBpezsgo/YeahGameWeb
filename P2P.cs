using System;
using System.Runtime.InteropServices.JavaScript;

namespace YeahGame.Web;

public enum PeerConnectionState
{
    Closed = 1,
    Connecting = 2,
    Connected = 3,
    New = 4,
    Disconnected = 5,
    Failed = 6,
}

public enum ICEConnectionState
{
    Checking = 1,
    Connected = 2,
    Completed = 3,
    Disconnected = 4,
    Closed = 5,
    Failed = 6,
    New = 7,
}

public enum ICEGatheringState
{
    Complete = 1,
    Gathering = 2,
    New = 3,
}

public enum DataChannelReadyState
{
    Closed = 1,
    Connecting = 2,
    Open = 3,
    Closing = 4,
}

public static partial class P2P
{
    public static System.Threading.Tasks.Task<JSObject> LoadAsync()
        => JSHost.ImportAsync("p2plib.js", "/p2plib.js");

    [JSImport("create", "p2plib.js")]
    public static partial void Create([JSMarshalAs<JSType.Function<JSType.String>>] Action<string> canJoin, [JSMarshalAs<JSType.Function<JSType.String>>] Action<string> onMessage);

    [JSImport("join", "p2plib.js")]
    public static partial void Join([JSMarshalAs<JSType.String>] string offer, [JSMarshalAs<JSType.Function<JSType.String>>] Action<string> canJoin, [JSMarshalAs<JSType.Function<JSType.String>>] Action<string> onMessage);

    [JSImport("send", "p2plib.js")]
    public static partial void Send([JSMarshalAs<JSType.String>] string message);

    [JSImport("answer", "p2plib.js")]
    public static partial void Answer([JSMarshalAs<JSType.String>] string answer);

    [JSImport("get_connectionState", "p2plib.js")]
    [return: JSMarshalAs<JSType.Number>]
    private static partial int get_connectionState();
    [JSImport("get_iceConnectionState", "p2plib.js")]
    [return: JSMarshalAs<JSType.Number>]
    private static partial int get_iceConnectionState();
    [JSImport("get_iceGatheringState", "p2plib.js")]
    [return: JSMarshalAs<JSType.Number>]
    private static partial int get_iceGatheringState();
    [JSImport("get_dataChannel_readyState", "p2plib.js")]
    [return: JSMarshalAs<JSType.Number>]
    private static partial int get_dataChannel_readyState();

    public static PeerConnectionState ConnectionState => (PeerConnectionState)get_connectionState();
    public static ICEConnectionState ICEConnectionState => (ICEConnectionState)get_iceConnectionState();
    public static ICEGatheringState ICEGatheringState => (ICEGatheringState)get_iceGatheringState();
    public static DataChannelReadyState? DataChannelReadyState
    {
        get
        {
            int v = get_dataChannel_readyState();
            if (v == -1) return null;
            return (DataChannelReadyState?)v;
        }
    }
}
