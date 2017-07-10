using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense.Hand;
using Intel.RealSense;

public class HandPositionManager : Publisher {
    
    enum State
    {
        Appear,
        Lost
    }

    HandManager handManager;
    State leftHandState, rightHandState;
    public int sign = 1;

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
                State isLeftAppear = State.Lost;
                State isRightAppear = State.Lost;
                for (int i = 0; i < handManager.handData.NumberOfHands; i++)
                {
                    IHand hand;
                    if (handManager.handData.QueryHandData(AccessOrderType.ACCESS_ORDER_BY_TIME,i,out hand) == Status.STATUS_NO_ERROR)
                    {
                        if (hand.BodySide == BodySideType.BODY_SIDE_LEFT)
                        {
                            Boardcast("OnLeftHandChange", new Vector3(hand.MassCenterWorld.x* sign, hand.MassCenterWorld.y , hand.MassCenterWorld.z ));
                            isLeftAppear = State.Appear;
                        }
                        else if (hand.BodySide == BodySideType.BODY_SIDE_RIGHT)
                        {
                            Boardcast("OnRightHandChange", new Vector3(hand.MassCenterWorld.x * sign, hand.MassCenterWorld.y , hand.MassCenterWorld.z ));
                            isRightAppear = State.Appear;
                        }
                    }
                }
                CheckStateChange(isLeftAppear, isRightAppear);
            }
    }

    void CheckStateChange(State isLeftHandAppear,State isRightHandAppear)
    {
        if (leftHandState != isLeftHandAppear)
        {
            if (isLeftHandAppear == State.Appear)
                Boardcast("OnLeftHandAppear");
            else
                Boardcast("OnLeftHandDisappear");
            leftHandState = isLeftHandAppear;
        }
        if (rightHandState != isRightHandAppear)
        {
            if (isRightHandAppear == State.Appear)
                Boardcast("OnRightHandAppear");
            else
                Boardcast("OnRightHandDisappear");
            rightHandState = isRightHandAppear;
        }
    }
}
