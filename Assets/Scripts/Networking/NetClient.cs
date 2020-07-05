using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class NetClient : Singleton<NetClient>
{
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";     //local host
    public int port = 5000;
    public int myId = 0;
    public TCP tcp;

    private void OnEnable()
    {
        tcp = new TCP();
        Debug.Log("Init TCP");
    }

    public void ConnectToServer()
    {
        tcp.Connect();
    }

    [System.Serializable]
    public class TCP
    {
        public TcpClient socket;
        private NetworkStream stream;
        private byte[] receiveBuffer;

        public void Connect()
        {
            Debug.Log("Connect got called");
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(NetClient.Instance.ip, NetClient.Instance.port, ConnectCallback, socket);

        }

        private void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    //todo: disconnect
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(receiveBuffer, data, byteLength);
                //todo: handle data

                //beginning stream to continiously reading data
                stream.BeginRead(buffer: receiveBuffer, offset: 0, size: 0, callback: ReceiveCallback, state: null);
            }
            catch (Exception e)
            {
                Console.Write("Error receiving Tcp Data: " + e);
                //todo: disconnect
            }
        }
    }
}
