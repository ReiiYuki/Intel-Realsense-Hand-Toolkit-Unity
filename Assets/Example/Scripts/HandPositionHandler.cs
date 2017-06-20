using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPositionHandler : MonoBehaviour {

    public GameObject leftCursor, rightCursor;

    void Start()
    {
        GameObject.Find("DepthCameraManager").GetComponent<HandPositionManager>().AddSubscriber(gameObject);    
    }

    void OnLeftHandChange(Vector3 handPosition)
    {
        Debug.Log(handPosition);
        leftCursor.transform.position = Camera.main.transform.position + new Vector3(handPosition.x * 100, handPosition.y * 100, handPosition.z * 100) + Camera.main.transform.forward;
    }

    void OnRightHandChange(Vector3 handPosition)
    {
        Debug.Log(handPosition);
        rightCursor.transform.position = Camera.main.transform.position + new Vector3(handPosition.x * 100, handPosition.y * 100, handPosition.z * 100) + Camera.main.transform.forward;
    }

}
