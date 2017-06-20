using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using Intel.RealSense.Hand;
public class SocketDataManager : MonoBehaviour {

    SocketIOComponent socket;

	// Use this for initialization
	void Awake () {
        
        socket = GetComponentInChildren<SocketIOComponent>();
        
        StartCoroutine(ConnectToServer());

        socket.On("identifySuccess", OnIdentifySuccess);
	}
	
    // Connect To Server
	IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["type"] = "IR";
        socket.Emit("identify", new JSONObject(data));
        Subscribe();
        yield break;
    }

    void OnGesture(GestureData gesture)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        if (GameObject.Find("DepthCameraManager").GetComponent<GesturalManager>().GetHandSide(gesture) == BodySideType.BODY_SIDE_LEFT)
            data["hand"] = "left";
        else if (GameObject.Find("DepthCameraManager").GetComponent<GesturalManager>().GetHandSide(gesture) == BodySideType.BODY_SIDE_RIGHT)
            data["hand"] = "right";
        data["gesture"] = gesture.name;
        socket.Emit("updateCurrentGesture", new JSONObject(data));
    }

    void OnLeftHandChange(Vector3 handPosition)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["x"] = handPosition.x.ToString();
        data["y"] = handPosition.y.ToString();
        data["z"] = handPosition.z.ToString();
        socket.Emit("updateLeftHandPosition", new JSONObject(data));
    }

    void OnRightHandChange(Vector3 handPosition)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["x"] = handPosition.x.ToString();
        data["y"] = handPosition.y.ToString();
        data["z"] = handPosition.z.ToString();
        socket.Emit("updateRightHandPosition", new JSONObject(data));
    }

    void Subscribe()
    {
        GameObject.Find("DepthCameraManager").GetComponent<HandPositionManager>().AddSubscriber(gameObject);
        GameObject.Find("DepthCameraManager").GetComponent<GesturalManager>().AddSubscriber(gameObject);
    }

    void OnIdentifySuccess(SocketIOEvent evt)
    {
        Subscribe();
    }
}
