using TMPro;
using UnityEngine;

public class HideStatusDropDn : MonoBehaviour
{
    public TMP_Dropdown role_drop;
    public TMP_Dropdown status_drop;
    public TMP_Text status_text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (role_drop.options[role_drop.value].text == "Student")
        {
            status_text.gameObject.SetActive(false);
            status_drop.gameObject.SetActive(false);
        }
        else
        {
            status_text.gameObject.SetActive(true);
            status_drop.gameObject.SetActive(true);
        }
    }
}
