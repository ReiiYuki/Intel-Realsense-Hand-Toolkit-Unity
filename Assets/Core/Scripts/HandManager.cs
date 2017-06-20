using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense;
using Intel.RealSense.Hand;

#pragma warning disable CS0618 // Type or member is obsolete

public class HandManager : MonoBehaviour {

    // Hand Manger Component
    HandModule handModule;
    public HandConfiguration handConfiguration;
    DepthCameraManger depthCameraManager;
    public HandData handData;

    // Tag for Log
    string TAG = "Hand Manager : ";

    // Call when init
    void Awake()
    {
        depthCameraManager = GetComponent<DepthCameraManger>();
    }

    //Call every frame
    void Update()
    {
        UpdateFrame();
    }

    // Initialize HandManager
    public void InitializeHandManger()
    {
        SetupHandModule();
    }

    // Activate Hand Module
    void SetupHandModule()
    {
        depthCameraManager = GetComponent<DepthCameraManger>();
        handModule = HandModule.Activate(depthCameraManager.senseManager);
        if (handModule == null)
            throw new System.Exception("Failed Loading Hand Module");
        else
        {
            Debug.Log(TAG + "Hand Module is loaded successful");
            SetupHandConfiguration();
        }
    }

    // Setup Hand Configuration
    void SetupHandConfiguration()
    {
        handConfiguration = handModule.CreateActiveConfiguration();
        if (handConfiguration == null)
            throw new System.Exception("Failed Creating Hand Configuration");
        else
        {
            Debug.Log(TAG + "Successful Creating Hand Configuration");
            CreateHandData();
        }
    }

    // Setup Output variable
    void CreateHandData()
    {
        handData = handModule.CreateOutput();
        if (handData == null)
            throw new System.Exception("Failed to create hand output");
        else
        {
            Debug.Log(TAG + "Successful to create output");
            SetupTrackingMode();
        }
    }

    // Setup Tracking Mode
    void SetupTrackingMode()
    {
        TrackingModeType trackingMode = TrackingModeType.TRACKING_MODE_FULL_HAND;
        handConfiguration.TrackingMode = trackingMode;
        Debug.Log(TAG + "Setup Hand Configuration Property");
        GetComponent<GesturalManager>().EnableGestures() ;
     }

    // Update Data In Each Frame
    public void UpdateFrame()
    {
        if (depthCameraManager.isStart)
        {
            if (depthCameraManager.senseManager.AcquireFrame(true).IsSuccessful())
            {
                handData.Update();
                depthCameraManager.senseManager.ReleaseFrame();
            }
            else
                throw new System.Exception("Failed Updating Frame!");
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
