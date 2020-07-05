using UnityEngine;
using System;
using System.Text;
using System.Net.Sockets;

public class Client : MonoBehaviour
{
    const int PORT_NO = 5000;
    const string SERVER_IP = "127.0.0.1";

    static void Main(string[] args)
    {
        //---data to send to the server---
        string textToSend = DateTime.Now.ToString();

        //---create a TCPClient object at the IP and port no.---
        TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
        NetworkStream nwStream = client.GetStream();
        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

        //---send the text---
        Console.WriteLine("Sending : " + textToSend);
        Debug.Log("Sending : " + textToSend);
        nwStream.Write(bytesToSend, 0, bytesToSend.Length);

        //---read back the text---
        byte[] bytesToRead = new byte[client.ReceiveBufferSize];
        int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
        Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
        Debug.Log("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
        Console.ReadLine();
        client.Close();
    }
}
