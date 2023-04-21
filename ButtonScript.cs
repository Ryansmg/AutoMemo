using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                try { averageDecibel = "AverageDecibel\n" + (TestScript.decibelSum / TestScript.decibelCount); }
                catch (DivideByZeroException) {
                    averageDecibel = "DividedByZeroException (temporary)";// occurs if user reset the sum & count
                }
                GameObject.Find("Screen2").GetComponent<UpdateScreen>().InstTextObj(averageDecibel);
                break;
            default:
                if(!warned0) Debug.LogWarning("Bad InstVarTextObj varName.");
                warned0 = true;
                break;
        }
        //GameObject.Find("Screen2").GetComponent<UpdateScreen>().InstTextObj(text);
    }


    //"{varName},, {change}
    //if {change} starts with "$set$" -> works like SetVar()
    public void ChangeVar(string varNameAndChange) 
    {
        string varName = varNameAndChange.Split(",, ")[0];
        string change = varNameAndChange.Split(",, ")[1];
        bool isSet = false;
        if (change.StartsWith("$set$")) { change = change.Replace("$set$", ""); isSet = true; }


        switch (varName)
        {
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
                    TestScript.decibelSum = int.Parse(change) * TestScript.decibelCount;
                    if (int.Parse(change) == 0) { TestScript.decibelCount = 0; }
                }
                else
                { //change
                    TestScript.decibelSum += int.Parse(change);
                    TestScript.decibelCount++;
                }
                break;

            default:
                if (!warned1) Debug.LogWarning("Bad ChangeVar varName.");
                warned1 = true;
                break;
        }
    }
}
