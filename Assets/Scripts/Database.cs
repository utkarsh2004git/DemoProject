using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
    private DatabaseReference databaseReference;

    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Dropdown statusDrop;
    [SerializeField] private TMP_Dropdown roleDrop;
    [SerializeField] private Button submitButton;
    [SerializeField] private PushNotificationHandler pushNotificationHandler;
    
    
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log("Firebase Initialized");
        });

        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        string email = emailInputField.text;
        string nameText = nameInputField.text;
        string status = statusDrop.options[statusDrop.value].text;
        string role = roleDrop.options[roleDrop.value].text;
        string deviceToken = pushNotificationHandler.DeviceToken;
        
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(nameText))
        {
            Debug.LogError("email or name is empty!");
            return;
        }

        
        if(role=="Student"){
            AddStudent(email, nameText, deviceToken);
        }
        else{
            AddTeacher(email, nameText,status, deviceToken);
        }

    }

    public void AddTeacher(string email, string name, string status ,string deviceToken)
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        Teacher user = new Teacher(email, name,status, deviceToken);
        string json = JsonUtility.ToJson(user);

        databaseReference.Child("Teacher").Child(deviceId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("User data successfully added to Firebase.");
            }
            else
            {
                Debug.LogError("Failed to add user data: " + task.Exception);
            }
        });
    }
    public void AddStudent(string email, string name,string deviceToken)
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        Student user = new Student(email, name, deviceToken);
        string json = JsonUtility.ToJson(user);

        databaseReference.Child("Student").Child(deviceId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("User data successfully added to Firebase.");
            }
            else
            {
                Debug.LogError("Failed to add user data: " + task.Exception);
            }
        });
    }
}

[Serializable]
public class Teacher
{
    public string email;
    public string status;
    public string name;
    public string deviceToken;
    
    public Teacher(string email, string name,string status ,string deviceToken)
    {
        this.email = email;
        this.name = name;
        this.status = status;
        this.deviceToken = deviceToken;
    }
}

[Serializable]
public class Student
{
    public string email;
    public string name;
    public string deviceToken;
    
    public Student(string email, string name ,string deviceToken)
    {
        this.email = email;
        this.name = name;
        this.deviceToken = deviceToken;
    }
}
