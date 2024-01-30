using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using YeahGame.Messages;
using YeahGame.Web;
using ConnectionClientDetails = System.Net.WebSockets.WebSocket;

namespace YeahGame;

public class WebRTCConnection<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TUserInfo> : ConnectionBase<TUserInfo, ConnectionClientDetails> where TUserInfo : ISerializable
{
    public override IPEndPoint? RemoteEndPoint => null;

    public override bool IsServer => _isServer;

    public override ConnectionState State
    {
        get
        {
            DataChannelReadyState? a = P2P.DataChannelReadyState;
            if (!a.HasValue) return ConnectionState.None;
            if (_isServer)
            {
                if (a.Value == DataChannelReadyState.Open)
                { return ConnectionState.Hosting; }
            }
            else
            {
                return a.Value switch
                {
                    DataChannelReadyState.Connecting => ConnectionState.Connecting,
                    DataChannelReadyState.Open => ConnectionState.Connected,
                    _ => ConnectionState.None,
                };
            }
            return ConnectionState.None;
        }
    }

    public override bool IsConnected => State is ConnectionState.Hosting or ConnectionState.Connected;

    bool _isServer;

    readonly Queue<string> _incomingQueue2 = new();

    static readonly IPEndPoint NoIPEndPoint = new(IPAddress.Any, 0);

    void OnMessage(string message)
    {
        _incomingQueue2.Enqueue(message);
    }

    public override void StartClient(IPEndPoint endPoint)
        => throw new NotImplementedException($"Call the other {nameof(StartClient)}");

    public override void StartHost(IPEndPoint endPoint)
        => throw new NotImplementedException($"Call the other {nameof(StartHost)}");

    public void StartClient(string offer)
    {
        _isServer = false;
        P2P.Join(offer, (description) =>
        {
            _isServer = false;
            Console.WriteLine(Web.Program.Encode(description));
        }, OnMessage);
    }

    public void StartHost()
    {
        _isServer = true;
        P2P.Create((description) =>
        {
            _isServer = true;
            Console.WriteLine(Web.Program.Encode(description));
        }, OnMessage);
    }

    public override void Tick()
    {
        while (_incomingQueue2.TryDequeue(out string? message))
        {
            OnReceivedInternal(message);
        }
    }

    void OnReceivedInternal(string message)
    {
        byte[] data = Convert.FromBase64String(message);
        this.OnReceiveInternal(data, NoIPEndPoint);
    }

    #region SendImmediate()

    protected override void SendImmediate(Message message)
    {
        P2P.Send(Convert.ToBase64String(Utils.Serialize(message)));
    }

    protected override void SendImmediate(byte[] data, IEnumerable<Message> messages)
    {
        P2P.Send(Convert.ToBase64String(data));
    }

    protected override void SendImmediateTo(byte[] data, IPEndPoint destination, IEnumerable<Message> messages)
    {
        P2P.Send(Convert.ToBase64String(data));
    }

    #endregion
}
