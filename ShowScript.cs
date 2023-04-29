using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScript : MonoBehaviour
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
            case "silentDbLimit":
                float sdl = GameObject.Find("AudioManager").GetComponent<RecordAudio>().silentDbLimit;
                gameObject.GetComponent<Text>().text = $"최소 인식 데시벨 ({sdl+180f})";
                break;
            case "cameraMoveCycle":
                float cmc = GameObject.Find("Main Camera").GetComponent<MoveCamera>().cameraMoveCycle;
                gameObject.GetComponent<Text>().text = $"카메라 이동 주기 ({cmc}초)";
                break;
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

            case "averageDecibel":
                if (decibelSum / decibelCount < -1000) { decibelSum = 0; decibelCount = 0; } //prevent -Infinity
                decibelSum += UpdateScreen.audioManagerStatic.GetComponent<RecordAudio>().db;
                decibelCount++;
                double formatAverage = decibelSum / decibelCount;
                formatAverage = Math.Truncate(formatAverage * 10) / 10;
                try { gameObject.GetComponent<Text>().text = "" + (formatAverage+180f); }
                catch (DivideByZeroException) { /* occurs if user reset the sum & count */}
                break;

            case "averageDecibel+40":
                double formatAverage2 = decibelSum / decibelCount;
                formatAverage2 = Math.Truncate(formatAverage2 * 10) / 10;
                try { gameObject.GetComponent<Text>().text = "" + (formatAverage2 + 220f); }
                catch (DivideByZeroException) { /* occurs if user reset the sum & count */}
                break;
            default:
                if(!warned0) Debug.LogWarning("Bad ShowScript type.");
                warned0 = true;
                break;
        }
    }
}
