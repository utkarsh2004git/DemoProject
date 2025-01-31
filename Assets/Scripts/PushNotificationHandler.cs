using System;
using Firebase.Messaging;
using UnityEngine;

public class PushNotificationHandler : MonoBehaviour
{
    [NonSerialized] public string DeviceToken;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Device token: " + token.Token);
        DeviceToken = token.Token;
    }

    void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message: " + e.Message.Notification.Body);
    }
}