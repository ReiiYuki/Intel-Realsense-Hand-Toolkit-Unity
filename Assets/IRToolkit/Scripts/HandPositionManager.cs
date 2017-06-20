using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense.Hand;
using Intel.RealSense;

public class HandPositionManager : Publisher {
    HandManager handManager;

	// Update is called once per frame
	void Update () {
        UpdateHandPosition();
	}

    // Update and boardcast hand position
    void UpdateHandPosition()
    {
        handManager = GetComponent<HandManager>();
        if (GetComponent<DepthCameraManger>().isStart)
            if (handManager.handData != null)
            {
                for (int i = 0; i < handManager.handData.NumberOfHands; i++)
                {
                    IHand hand;
                    if (handManager.handData.QueryHandData(AccessOrderType.ACCESS_ORDER_BY_TIME,i,out hand) == Status.STATUS_NO_ERROR)
                    {
                        if (hand.BodySide == BodySideType.BODY_SIDE_LEFT)
                            Boardcast("OnLeftHandChange", new Vector3(hand.MassCenterWorld.x, hand.MassCenterWorld.y, hand.MassCenterWorld.z));
                        else if (hand.BodySide == BodySideType.BODY_SIDE_RIGHT)
                            Boardcast("OnRightHandChange", new Vector3(hand.MassCenterWorld.x, hand.MassCenterWorld.y, hand.MassCenterWorld.z));
                    }
                }
            }
    }
}
