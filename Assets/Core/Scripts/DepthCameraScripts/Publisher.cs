using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Publisher : MonoBehaviour {

    List<GameObject> subscribers;

    // Use this for initialization
    void Awake () {
        subscribers = new List<GameObject>();
    }

    // Add Subscribers
    public void AddSubscriber(GameObject obj)
    {
        subscribers.Add(obj);
    }

    // Boardcast to subscriber
    protected void Boardcast(string method,object obj)
    {
        foreach (GameObject subscriber in subscribers)
            subscriber.SendMessage(method, obj, SendMessageOptions.DontRequireReceiver);
    }

    protected void Boardcast(string method)
    {
        foreach (GameObject subscriber in subscribers)
            subscriber.SendMessage(method, SendMessageOptions.DontRequireReceiver);
    }
}