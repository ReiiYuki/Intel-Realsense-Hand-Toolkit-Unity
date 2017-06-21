using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense.Hand;
public class DataSocketManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("DepthCameraManager").GetComponent<HandPositionManager>().AddSubscriber(gameObject);
        GameObject.Find("DepthCameraManager").GetComponent<GesturalManager>().AddSubscriber(gameObject);
    }

    void OnLeftHandChange(Vector3 handPosition)
    {
        GetComponent<SocketServer>().SendHandPosition("left", handPosition.x, handPosition.y, handPosition.z);
    }

    void OnRightHandChange(Vector3 handPosition)
    {
        GetComponent<SocketServer>().SendHandPosition("right", handPosition.x, handPosition.y, handPosition.z);
    }

    void OnGesture(GestureData gesture)
    {
        string side = "";
        if (GameObject.Find("DepthCameraManager").GetComponent<GesturalManager>().GetHandSide(gesture) == BodySideType.BODY_SIDE_LEFT)
            side = "left";
        else if (GameObject.Find("DepthCameraManager").GetComponent<GesturalManager>().GetHandSide(gesture) == BodySideType.BODY_SIDE_RIGHT)
            side = "right";
        GetComponent<SocketServer>().SendGesture(side, gesture.name);
    }
}
