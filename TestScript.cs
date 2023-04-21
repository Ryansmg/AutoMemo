using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public string type;
    public static double decibelSum = 0;
    public static int decibelCount = 0;
    bool warned0 = false; //bad type
    void Start()
    {
    }
    void Update()
    {
        switch(type)
        {
            case "showStatusText":
                gameObject.GetComponent<Text>().text
                        = $"Decibel : {UpdateScreen.audioManagerStatic.GetComponent<RecordAudio>().db}\nStatus : {UpdateScreen.status}";
                break;

            case "averageDecibelText":
                if (decibelSum / decibelCount < -1000) { decibelSum = 0; decibelCount = 0; } //prevent -Infinity
                decibelSum += UpdateScreen.audioManagerStatic.GetComponent<RecordAudio>().db;
                decibelCount++;
                try { gameObject.GetComponent<Text>().text = "AverageDecibel\n" + (decibelSum / decibelCount); }
                catch (DivideByZeroException) { /* occurs if user reset the sum & count */}
                break;

            default:
                if(!warned0) Debug.LogWarning("Bad TestScript type.");
                warned0 = true;
                break;
        }
    }
}
