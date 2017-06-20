using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class SocketDataManager : MonoBehaviour {

    SocketIOComponent socket;

	// Use this for initialization
	void Awake () {
        
        socket = GetComponentInChildren<SocketIOComponent>();
        StartCoroutine(ConnectToServer());
	}
	
    // Connect To Server
	IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["type"] = "IR";
        socket.Emit("identify", new JSONObject(data));
        yield break;
    }
}
