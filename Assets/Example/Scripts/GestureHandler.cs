using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense.Hand;

namespace Example
{
    public class GestureHandler : MonoBehaviour
    {

        public GameObject leftCursor, rightCursor;

        //Call when inti
        void Start()
        {
            GameObject.Find("DepthCameraManager").GetComponent<GesturalManager>().AddSubscriber(gameObject);
        }

        // Call When Detect Gesture
        void OnGesture(GestureData gesture)
        {
            if (GameObject.Find("DepthCameraManager").GetComponent<GesturalManager>().GetHandSide(gesture) == BodySideType.BODY_SIDE_LEFT)
            {
                leftCursor.GetComponent<TextMesh>().text = "L : " + gesture.name;
            }
            else if (GameObject.Find("DepthCameraManager").GetComponent<GesturalManager>().GetHandSide(gesture) == BodySideType.BODY_SIDE_RIGHT)
            {
                rightCursor.GetComponent<TextMesh>().text = "R : " + gesture.name;
            }
        }
    }
}