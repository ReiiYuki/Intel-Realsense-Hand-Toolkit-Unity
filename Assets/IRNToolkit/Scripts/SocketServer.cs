using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketServer : MonoBehaviour {
    public string IP;
    Socket socket;
    IPEndPoint ipEndPoint;
	// Use this for initialization
	void Start () {
        StartSocket();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartSocket() {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ipEndPoint = new IPEndPoint(IPAddress.Parse(IP), 11000);
        Debug.Log("Initialize Socket Server");
        Debug.Log("Send Target " + ipEndPoint.Address + ":" + ipEndPoint.Port);
    }

    public void SendHandPosition(string handside, float x, float y, float z)
    {
        Send("pos " + handside + " " + x + " " + y + " " + z);
    }

    public void SendGesture(string handside,string gesture)
    {
        Send("ges " + handside + " " + gesture);
    }

    void Send(string data)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        socket.SendTo(buffer, ipEndPoint);
    }
}
