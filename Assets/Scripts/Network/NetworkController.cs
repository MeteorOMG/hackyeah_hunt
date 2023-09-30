using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

public class NetworkController : MonoBehaviour
{
    [SerializeField] private string serverAdress;
    [SerializeField] private string ms;

    private enum SslProtocolsHack
    {
        Tls = 192,
        Tls11 = 768,
        Tls12 = 3072
    }

    public WebSocket ws;

    [ContextMenu("Connect")]
    public void Connect()
    {
        Connect(null);
    }

    public void Connect(UnityAction OnConnected)
    {
        ws = new WebSocket(serverAdress);
        var sslProtocolHack = (System.Security.Authentication.SslProtocols)(SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
        ws.SslConfiguration.EnabledSslProtocols = sslProtocolHack;
        ws.Connect();
        OnConnected?.Invoke();
    }

    [ContextMenu("Send")]
    public void Send()
    {
        ws.Send(ms);
        Debug.Log(ws.IsAlive);
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);
    }

    /*public void SendPlayerTip(HintMessage message)
    {

    }

    public void SendStart(StartMessage message)
    {
        ws.Send(JsonUtility.ToJson(message));
    }*/

    public void SendData(string data)
    {
        ws.Send(data);
    }
}
