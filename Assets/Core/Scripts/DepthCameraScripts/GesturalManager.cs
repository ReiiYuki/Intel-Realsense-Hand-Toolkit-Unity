using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense;
using Intel.RealSense.Hand;

public class GesturalManager : Publisher {

    public bool isEnableAllGesture;
    public string[] gestureActions;
    public GameObject L, R;

    HandManager handManager;

    string TAG = "Gestural Manager : ";

	// Update is called once per frame
	void Update () {
        UpdateHandGesture();
    }

    // Enable Gesture
    public void EnableGestures()
    {
        handManager = GetComponent<HandManager>();
        if (isEnableAllGesture)
        {
            Debug.Log(TAG + "All Gesture Enabled!");
            handManager.handConfiguration.EnableAllGestures();
        }else
        {
            foreach (string gesture in gestureActions)
            {
                Debug.Log(TAG + gesture + " Enabled!");
                handManager.handConfiguration.EnableGesture(gesture, true);
            }
        }
        handManager.handConfiguration.ApplyChanges();
        GetComponent<DepthCameraManger>().StartDevice();
    }

    // Update Gesture
    public void UpdateHandGesture()
    {
        if (GetComponent<DepthCameraManger>().isStart)
            if (handManager.handData != null)
                if (handManager.handData.FiredGestureData != null)
                    foreach (GestureData gesture in handManager.handData.FiredGestureData)
                    {
                        string camelCaseGestureName = CreateCamelCase(gesture.name);
                        if (GetHandSide(gesture)==BodySideType.BODY_SIDE_LEFT)
                        {
                            Boardcast("OnLeftHand" + camelCaseGestureName);
                            L.GetComponent<TextMesh>().text = "L : " + camelCaseGestureName;
                        }
                        else if (GetHandSide(gesture) == BodySideType.BODY_SIDE_RIGHT)
                        {
                            Boardcast("OnRightHand" + camelCaseGestureName);
                            R.GetComponent<TextMesh>().text = "R : " + camelCaseGestureName;
                        }
                        Boardcast("On" + camelCaseGestureName);
                        Boardcast("OnGesture", gesture);
                    }
    }

    // Create Camel Case String
    string CreateCamelCase(string normalString)
    {
        string[] arrStr = normalString.Split('_');
        for (int i = 0;i<arrStr.Length;i++)
            arrStr[i] = (arrStr[i][0] + "").ToUpper() + arrStr[i].Substring(1);
        return string.Join("", arrStr);
    }

    // Get Hand Side from gesture
    public BodySideType GetHandSide(GestureData gesture)
    {
        IHand hand;
        if (GetComponent<HandManager>().handData.QueryHandDataById(gesture.handId, out hand) == Status.STATUS_NO_ERROR)
        {
            return hand.BodySide;
        }
        return BodySideType.BODY_SIDE_UNKNOWN;
    }

    //ExampleMethod
    void OnSpreadfingers()
    {
        Debug.Log("Yes");
    }

    void OnLeftHandSpreadfingers()
    {
        Debug.Log("LYes");
    }

    void OnRightHandSpreadfingers()
    {
        Debug.Log("RYes");
    }
}