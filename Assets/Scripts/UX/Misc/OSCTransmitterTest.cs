using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using UnityEngine.Networking;

public class OSCTransmitterTest : MonoBehaviour
{
    public string RemoteHost = "192.168.0.151";
    public int RemotePort = 9000;
    private OSCTransmitter transmitter;

    private void Start()
    {
        // Creating a transmitter.
        transmitter = gameObject.AddComponent<OSCTransmitter>();

        SetRemoteAdress();
    }

    // Start is called before the first frame update
    void SetRemoteAdress()
    {
        // Set remote host address.
        transmitter.RemoteHost = RemoteHost;

        // Set remote port;
        transmitter.RemotePort = RemotePort;

        Debug.Log($"Setup OSC at Remote Host: {RemoteHost}, Remote Port: {RemotePort}");
        StartCoroutine(SenderA());
    }

    public bool toggle = false;
    public IEnumerator SenderA()
    {
        toggle = !toggle;
        yield return new WaitForSeconds(1.0f);
        if (toggle)
            SendMessage();
        else
            SendMessageB();

        StartCoroutine(SenderA());
        Debug.Log($"Sent Address to {transmitter.RemoteHost}, {transmitter.RemotePort}");
    }

    public void SendMessage()
    {
        // Create message
        var message = new OSCMessage("/blender/message/address");

        // Populate values.
        message.AddValue(OSCValue.String("Hello, world!")); //title? needs a start val o.o
        message.AddValue(OSCValue.Float(1337f));
        message.AddValue(OSCValue.Float(12.555f));

        // Send message
        transmitter.Send(message);
    }

    public void SendMessageB()
    {
        // Create message
        var message = new OSCMessage("/blender/message/sum");

        // Populate values.
        message.AddValue(OSCValue.Int(12));
        message.AddValue(OSCValue.Float(1337f));

        // Send message
        transmitter.Send(message);
    }
}
