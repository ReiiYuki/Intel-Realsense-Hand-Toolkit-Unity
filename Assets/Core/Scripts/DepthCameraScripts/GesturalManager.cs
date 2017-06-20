using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense;
using Intel.RealSense.Hand;

public class GesturalManager : MonoBehaviour {

    public string[] gestureActions;
    public GameObject L, R;

    HandManager handManager;

    string TAG = "Gestural Manager : ";

	// Update is called once per frame
	void Update () {
        UpdateHandGesture();
    }

    public void EnableGestures()
    {
        handManager = GetComponent<HandManager>();
        foreach (string gesture in gestureActions)
        {
            Debug.Log(TAG + gesture+" Enabled!");
            handManager.handConfiguration.EnableGesture(gesture, true);
        }
        handManager.handConfiguration.ApplyChanges();
        GetComponent<DepthCameraManger>().StartDevice();
    }

    public void UpdateHandGesture()
    {
        if (GetComponent<DepthCameraManger>().isStart)
            if (handManager.handData != null)
                if (handManager.handData.FiredGestureData != null)
                    foreach (GestureData gesture in handManager.handData.FiredGestureData)
                    {
                        IHand hand;
                        if (GetComponent<HandManager>().handData.QueryHandDataById(gesture.handId,out hand) == Status.STATUS_NO_ERROR)
                        {
                            string camelCaseGestureName = CreateCamelCase(gesture.name);
                            if (hand.BodySide == BodySideType.BODY_SIDE_LEFT)
                            {
                                this.SendMessage("OnLeftHand" + camelCaseGestureName, SendMessageOptions.DontRequireReceiver);
                                L.GetComponent<TextMesh>().text = "L : " + camelCaseGestureName;
                                this.SendMessage("OnGesture", gesture, SendMessageOptions.DontRequireReceiver);
                            }
                            else if (hand.BodySide == BodySideType.BODY_SIDE_RIGHT)
                            {
                                this.SendMessage("OnRightHand" + camelCaseGestureName, SendMessageOptions.DontRequireReceiver);
                                R.GetComponent<TextMesh>().text = "R : " + camelCaseGestureName;
                                this.SendMessage("OnGesture", gesture, SendMessageOptions.DontRequireReceiver);
                            }
                            this.SendMessage("On" + camelCaseGestureName, SendMessageOptions.DontRequireReceiver);
                        }
                    }
    }

    string CreateCamelCase(string normalString)
    {
        string[] arrStr = normalString.Split('_');
        for (int i = 0;i<arrStr.Length;i++)
            arrStr[i] = (arrStr[i][0] + "").ToUpper() + arrStr[i].Substring(1);
        return string.Join("", arrStr);
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
