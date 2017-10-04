using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CommonLib.Net {
  public class NetServer : IDisposable {
    private TcpListener _listener;
    private readonly Dictionary<int, TcpClient> _clients = new Dictionary<int, TcpClient>();

    ~NetServer() {
      Dispose();
    }

    // properties
    public bool IsListening { get; private set; }

    public bool IsSending { get; private set; }

    // methods
    public void Start(int port) {
      _listener = new TcpListener(IPAddress.Any, port);
      _listener.Start();

      IsListening = true;

      Task.Run(() => {
        while (IsListening) {
          var client = _listener.AcceptTcpClient();

          if (!client.Connected) {
            continue;
          }

          var id = client.GetHashCode();
          _clients.Add(id, client);

          ClientConnected(this, id);

          ReceiveData(id);
        }
      });
    }

    public void Stop() {
      if (!IsListening) {
        return;
      }

      IsListening = false;

      _listener.Stop();
      _listener.Server.Dispose();
    }

    public TcpClient GetClientById(int id) {
      return _clients[id];
    }

    public void Send(int id, byte[] data) {
      var client = _clients[id];

      IsSending = true;
      Sending(this, EventArgs.Empty);

      try {
        Task
          .Run(() => client.Client.Send(data))
          .ContinueWith(x => Sent(this, EventArgs.Empty));
      }
      catch (Exception e) {
        Error(this, new NetException("An error occurred while trying to send.", e));
      }

      IsSending = false;
    }

    private void ReceiveData(int id) {
      var client = _clients[id];

      Task.Run(() => {
        while (IsListening) {
          var buffer = new byte[1024];
          byte[] data;

          using (var stream = client.GetStream()) {
            using (var ms = new MemoryStream()) {
              int bytesRead;

              while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0) {
                ms.Write(buffer, 0, bytesRead);
              }

              data = ms.ToArray();
            }
          }

          DataReceived(this, data, id);
        }
      });
    }

    // events
    public event NetClientConnectedEventHandler ClientConnected = delegate { };

    public event EventHandler Sending = delegate { };

    public event EventHandler Sent = delegate { };

    public event NetClientDataReceivedEventHandler DataReceived = delegate { };

    public event NetErrorEventHandler Error = delegate { };

    // IDisposable
    public void Dispose() {
      try {
        Stop();
        _listener.Server.Dispose();
      }
      catch (ObjectDisposedException) {
        // ignore
      }
    }
  }
}