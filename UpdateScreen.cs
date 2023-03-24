using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScreen : MonoBehaviour
{
    public static string screenContent = "";
    public GameObject AudioManager;
    void Update()
    {
        if (gameObject.name.Equals("Screen")) gameObject.GetComponent<Text>().text = screenContent;
        if (gameObject.name.Equals("Screen2")) gameObject.GetComponent<Text>().text = "Decibel : " + AudioManager.GetComponent<RecordAudio>().db;
    }
}
