namespace CommonLib.Net {
  public delegate void NetClientConnectedEventHandler(object sender, int id);

  public delegate void NetServerDataReceivedEventHandler(object sender, byte[] data);

  public delegate void NetClientDataReceivedEventHandler(object sender, byte[] data, int clientId);

  public delegate void NetErrorEventHandler(object sender, NetException e);
}