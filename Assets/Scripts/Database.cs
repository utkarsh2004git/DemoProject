using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField ageInputField;
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
        string name = nameInputField.text;
        string ageText = ageInputField.text;
        string status = statusDrop.options[statusDrop.value].text;
        string role = roleDrop.options[roleDrop.value].text;
        string deviceToken = pushNotificationHandler.DeviceToken;
        
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(ageText))
        {
            Debug.LogError("Name or age is empty!");
            return;
        }

        if (!int.TryParse(ageText, out int age))
        {
            Debug.LogError("Invalid age input!");
            return;
        }
        if(role=="Student"){
            AddStudent(name, age, deviceToken);
        }
        else{
            AddTeacher(name, age,status, deviceToken);
        }

    }

    public void AddTeacher(string name, int age, string status ,string deviceToken)
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        Teacher user = new Teacher(name, age,status, deviceToken);
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
    public void AddStudent(string name, int age,string deviceToken)
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        Student user = new Student(name, age, deviceToken);
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
    public string name;
    public string status;
    public int age;
    public string deviceToken;
    
    public Teacher(string name, int age,string status ,string deviceToken)
    {
        this.name = name;
        this.age = age;
        this.status = status;
        this.deviceToken = deviceToken;
    }
}

[Serializable]
public class Student
{
    public string name;
    public int age;
    public string deviceToken;
    
    public Student(string name, int age ,string deviceToken)
    {
        this.name = name;
        this.age = age;
        this.deviceToken = deviceToken;
    }
}
