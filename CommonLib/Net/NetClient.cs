using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using CommonLib.Extensions;

namespace CommonLib.Net {
  public class NetClient : IDisposable {
    private readonly TcpClient _client;

    public NetClient() {
      _client = new TcpClient();
    }

    ~NetClient() {
      Dispose();
    }

    // properties
    public bool IsConnected { get; private set; }

    public bool IsConnecting { get; private set; }

    public bool IsDisconnecting { get; private set; }

    public bool IsSending { get; private set; }

    // methods
    public void Connect(string destination, int port) {
      if (IsConnected) {
        Disconnect();
      }

      var ipAddress = destination.ToIpAddress();

      if (ipAddress.Equals(IPAddress.Any)) {
        throw new NetException($"Cannot resolve {destination}.");
      }

      IsConnecting = true;
      IsConnected = false;
      IsDisconnecting = false;
      IsSending = false;

      Connecting(this, EventArgs.Empty);

      try {
        _client
          .ConnectAsync(ipAddress, port)
          .ContinueWith(x => {
            IsConnecting = false;

            if (!_client.Connected) {
              Error(this, new NetException("Connection failed."));
            }
            else {
              IsConnected = true;
              Connected(this, EventArgs.Empty);

              ReceiveData();
            }
          });
      }
      catch (Exception e) {
        Error(this, new NetException("An error occurred while trying to connect.", e));
      }
    }

    public void Disconnect() {
      if (!IsConnected) {
        return;
      }

      IsConnecting = false;
      IsDisconnecting = true;
      IsSending = false;

      _client.GetStream().Close();
      _client.Close();

      IsConnected = false;
      IsConnecting = false;
      IsDisconnecting = false;

      Disconnected(this, EventArgs.Empty);
    }

    public void Send(byte[] data) {
      IsSending = true;
      Sending(this, EventArgs.Empty);

      try {
        Task
          .Run(() => _client.Client.Send(data))
          .ContinueWith(x => Sent(this, EventArgs.Empty));
      }
      catch (Exception e) {
        Error(this, new NetException("An error occurred while trying to send.", e));
      }

      IsSending = false;
    }

    private void ReceiveData() {
      Task.Run(() => {
        while (IsConnected) {
          var buffer = new byte[1024];
          byte[] data;

          using (var stream = _client.GetStream()) {
            using (var ms = new MemoryStream()) {
              int bytesRead;

              while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0) {
                ms.Write(buffer, 0, bytesRead);
              }

              data = ms.ToArray();
            }
          }

          DataReceived(this, data);
        }
      });
    }

    // events
    public event EventHandler Connecting = delegate { };

    public event EventHandler Connected = delegate { };

    public event EventHandler Disconnected = delegate { };

    public event EventHandler Sending = delegate { };

    public event EventHandler Sent = delegate { };

    public event NetServerDataReceivedEventHandler DataReceived = delegate { };

    public event NetErrorEventHandler Error = delegate { };

    // IDisposable
    public void Dispose() {
      try {
        _client.GetStream().Close();
        _client.Close();
        _client.Client.Dispose();
      }
      catch (ObjectDisposedException) {
        // ignore
      }
    }
  }
}
