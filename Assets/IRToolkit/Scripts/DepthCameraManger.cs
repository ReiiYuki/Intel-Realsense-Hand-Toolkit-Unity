﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense;

public class DepthCameraManger : MonoBehaviour {

    /**
     * Depth Camera Component
     **/
    public SenseManager senseManager;
    CaptureManager captureManager;
    DeviceInfo deviceInfo;
    public bool isStart;

    // Tag for Log
    string TAG = "Depth Camera : ";

    //Call this before start
    void Awake()
    {
        InititalizeSenseManager();
    }

    // Initialize Sense Manager
    void InititalizeSenseManager()
    {
        Session session = Session.CreateInstance()
        if (session == null)
            throw new System.Exception("Session Initialize Failure!");
        senseManager = session.CreateSenseManager();
        if (senseManager == null)
            throw new System.Exception("Sense Manager Initialize Failure!");
        else
        {
            Debug.Log(TAG + "Sense Manager Initialize Successful");
            InitializeCaptureManager();
        }
    }

    // Initialize Capture Manager
    void InitializeCaptureManager()
    {
        captureManager = senseManager.CaptureManager;
        if (captureManager == null)
            throw new System.Exception("Capture Manager Initialize Failure!");
        else
        {
            Debug.Log(TAG + "Capture Manager Initialize Successful");
            LoadCapture();
        }
    }

    // Load Capture from Group Sensor Info
    void LoadCapture()
    {
        ImplDesc desc = new ImplDesc()
        {
            group = ImplGroup.IMPL_GROUP_SENSOR,
            subgroup = ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE
        };
        Capture capture;
        if (senseManager.Session.CreateImpl<Capture>(senseManager.Session.QueryImpl(desc, 0), out capture) < Status.STATUS_NO_ERROR)
            throw new System.Exception("Initialize Capture Failure!");
        else
        {
            if (capture == null || capture.DeviceInfo == null || capture.DeviceInfo.Length == 0)
                throw new System.Exception("Capturing Failure");
            else
            {
                CaptureDevice(capture);
            }
        }
    }

    // Capture Device
    void CaptureDevice(Capture capture)
    {
        deviceInfo = capture.DeviceInfo[0];
        if (deviceInfo == null)
            throw new System.Exception("Fail to obtain camera!");
        else
        {
            if (captureManager.SetFileName(deviceInfo.name, true) < Status.STATUS_NO_ERROR)
                throw new System.Exception("Fail to connect to " + deviceInfo.name);
            else
            {
                Debug.Log(TAG + "Connect to " + deviceInfo.name);
                GetComponent<HandManager>().InitializeHandManger();
            }
        }
    }

    // Start Device
    public void StartDevice()
    {
        if (senseManager.Init() == Status.STATUS_NO_ERROR)
        {
            Debug.Log(TAG + "Initialization Successful!");
            isStart = true;
        }
        else
            throw new System.Exception("Initialization Failed!");
    }

    // Call when exit application
    void OnApplicationQuit()
    {
        senseManager.Close();
    }
}
