using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    bool warned0 = false; //bad InstVarTextObj varName
    bool warned1 = false; //bad ChangeVar varName
    public void InstTextObj(string text)
    {
        GameObject.Find("Screen2").GetComponent<UpdateScreen>().InstTextObj(text);
    }

    public void InstVarTextObj(string varName)
    {
        switch (varName)
        {
            case "averageDecibel":
                string averageDecibel;
                try { averageDecibel = "AverageDecibel\n" + (ShowScript.decibelSum / ShowScript.decibelCount); }
                catch (DivideByZeroException)
                {
                    averageDecibel = "DividedByZeroException (temporary)";// occurs if user reset the sum & count
                }
                GameObject.Find("Screen2").GetComponent<UpdateScreen>().InstTextObj(averageDecibel);
                break;
            default:
                if (!warned0) Debug.LogWarning("Bad InstVarTextObj varName.");
                warned0 = true;
                break;
        }
        //GameObject.Find("Screen2").GetComponent<UpdateScreen>().InstTextObj(text);
    }


    //"{varName},, {change}
    //if {varName} or {change} starts with "$set$" -> works like SetVar()
    public void ChangeVar(string varNameAndChange)
    {
        string varName = varNameAndChange.Split(",, ")[0];
        string change = varNameAndChange.Split(",, ")[1];
        bool isSet = false;
        if (change.StartsWith("$set$")) { change = change.Replace("$set$", ""); isSet = true; }
        if (varName.StartsWith("$set$")) { varName = varName.Replace("$set$", ""); isSet = true; }


        switch (varName)
        {
            case "silentDbLimit":
                if (isSet)
                { //set
                    GameObject.Find("AudioManager").GetComponent<RecordAudio>().silentDbLimit = float.Parse(change);
                }
                else
                { //change
                    GameObject.Find("AudioManager").GetComponent<RecordAudio>().silentDbLimit += float.Parse(change);
                }
                break;

            case "cameraMoveCycle":
                if (isSet)
                { //set
                    GameObject.Find("Main Camera").GetComponent<MoveCamera>().cameraMoveCycle = float.Parse(change);
                }
                else
                { //change
                    GameObject.Find("Main Camera").GetComponent<MoveCamera>().cameraMoveCycle += float.Parse(change);
                }
                break;

            case "moveUp":
                if (isSet)
                { //set
                    GameObject.Find("Main Camera").GetComponent<MoveCamera>().moveUp = bool.Parse(change);
                }
                else
                { //change
                    bool preUp = GameObject.Find("Main Camera").GetComponent<MoveCamera>().moveUp;
                    GameObject.Find("Main Camera").GetComponent<MoveCamera>().moveUp = !preUp;
                }
                break;

            case "moveDown":
                if (isSet)
                { //set
                    GameObject.Find("Main Camera").GetComponent<MoveCamera>().moveDown = bool.Parse(change);
                }
                else
                { //change
                    bool preUp = GameObject.Find("Main Camera").GetComponent<MoveCamera>().moveDown;
                    GameObject.Find("Main Camera").GetComponent<MoveCamera>().moveDown = !preUp;
                }
                break;

            case "MoveCamera.yLimit":
                if (isSet)
                { //set
                    GameObject.Find("Main Camera").GetComponent<MoveCamera>().yLimit = int.Parse(change);
                }
                else
                { //change
                    GameObject.Find("Main Camera").GetComponent<MoveCamera>().yLimit += int.Parse(change);
                }
                break;

            //averageDecibel is treated specially.
            case "averageDecibel":
                if (isSet)
                { //set
                    ShowScript.decibelSum = int.Parse(change) * ShowScript.decibelCount;
                    if (int.Parse(change) == 0) { ShowScript.decibelCount = 0; }
                }
                else
                { //change
                    ShowScript.decibelSum += int.Parse(change);
                    ShowScript.decibelCount++;
                }
                break;

            default:
                if (!warned1) Debug.LogWarning($"Bad ChangeVar varName. ({varName})");
                warned1 = true;
                break;
        }
    }


    public void Save()
    {
        List<string> list = new();
        for (int i = 0; i < UpdateScreen.textObjectNum; i++)
        {
            try
            {
                GameObject ts = GameObject.Find($"TextSquare{i}");
                list.Add(ts.GetComponentInChildren<Text>().text);
            }
            catch (NullReferenceException) { /* if TextSquare{i} isn't generated by user input */ }
        }

        string writeText = "";
        foreach (string s in list)
        {
            writeText += s;
            writeText += "\\";
        }
        if (writeText.Equals("")) { writeText = "\\"; }
        writeText = writeText.Remove(writeText.Length - 1, 1);
        //Debug.Log(writeText);

        string filePath = Application.persistentDataPath + "/save.amt"; //AutoMemoText
        if (!File.Exists(filePath)) File.Create(filePath);
        File.WriteAllText(filePath, writeText);
    }
    public void Load()
    {
        string filePath = Application.persistentDataPath + "/save.amt";
        string[] fileTexts = File.ReadAllText(filePath).Split('\\');
        if (fileTexts[0].Equals("")) return;
        foreach (string s in fileTexts)
        {
            InstTextObj(s);
        }
    }
    public static Vector3 testPos; //used to produce NullReferenceException
    public void Delete()
    {
        int preSquares = 0;
        for (int i = 0; i < UpdateScreen.textObjectNum; i++)
        {
            try
            {
                GameObject ts = GameObject.Find($"TextSquare{i}");
                testPos = ts.transform.position;
                Destroy(ts);
            }
            catch (NullReferenceException) {
                preSquares++;
                /* if TextSquare{i} isn't generated by user input */ 
            }
        }
        UpdateScreen.textObjectNum = preSquares;
    }

    public float adblTimer = 0f;
    public int adblStep = 0;
    public bool adbl = false; //AutoDBLimit
    public static GameObject AutoDbCanvas;
    public void AutoDbLimit()
    {
        adbl = true;
    }
    private void Start()
    {
        if(gameObject.name.Equals("Script") && gameObject.tag.Equals("ButtonScript"))
        {
            AutoDbCanvas = gameObject.GetComponent<ObjContainer>().obj;
        }
    }
    private void Update()
    {
        if (adbl && gameObject.name.Equals("Script") && gameObject.tag.Equals("ButtonScript"))
        {
            switch (adblStep)
            {
                case 0:
                    ChangeVar("$set$averageDecibel,, 0");
                    adblStep = 1;
                    break;
                case 1:
                    AutoDbCanvas.SetActive(true);
                    adblStep = 2;
                    break;
                case 2:
                    adblTimer += Time.deltaTime;
                    if(adblTimer > 5f) {
                        adblTimer = 0f;
                        float x = (float) Math.Truncate(ShowScript.decibelSum / ShowScript.decibelCount * 10) / 10 + 180; //데시벨
                        //125.561332 -> 125.5 형태로 format
                        float sdl; //silentDblimit
                        if(x < 80)
                        {
                            sdl = x + 41.077f;
                        } else
                        {
                            sdl = (-17000 / (x + 101)) + 255 + (-2000 / (x - 30));
                        }
                        ChangeVar($"$set$silentDbLimit,, {sdl - 180}");
                        adblStep = 0;
                        AutoDbCanvas.SetActive(false);
                        adbl = false;
                    }
                    break;
            }
        }
    }
}
