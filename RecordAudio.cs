using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RecordAudio : MonoBehaviour
{
    public float silentTimer = 0.0f;
    public bool isSilent = false;
    public float silentDbLimit = -55.0f;
    public float db; //min -180, max 0 (not accurate)
    public float silentTimeLimit = 1.5f;
    public int talkStartPos;
    public int micPos;
    float[] recordData;
    bool TSPUpdated = false; //if talkStartPos is updated == user started talking
    AudioSource audioSource;
    public float startTimer = 0.5f;
    SavWav savWav = new SavWav();
    public bool checkClip = false;
    public static string pdp; //persistentDataPath
    public static string txt;
    void Start()
    {
        Application.RequestUserAuthorization(UserAuthorization.Microphone);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateScreen.isSilent = isSilent;
        if (startTimer > 0.0f) { startTimer -= Time.deltaTime; return; } //prevents recording while loading

        micPos = Microphone.GetPosition(null);
        db = AudioManager.MicLoudnessinDecibels;
        if (db < silentDbLimit)
        {   
            //when user is silent
            silentTimer += Time.deltaTime;
            if (silentTimer > silentTimeLimit)
            {   
                //when user stopped talking
                silentTimer = silentTimeLimit;
                TSPUpdated = false;
                if (!isSilent)
                {
                    //if recorded audio isn't converted yet
                    isSilent = true;
                    UpdateScreen.status = "loading";

                    //saving recorded audio to AudioClip(clip)
                    try
                    {
                        recordData = new float[micPos - talkStartPos + 1];
                    } 
                    catch(OverflowException)
                    {
                        Debug.Log("OverflowException RA60");
                        UpdateScreen.newContent = "$system$죄송합니다. 텍스트를 정상적으로 인식하지 못했습니다.\n다시 한 번 말씀해주세요.";
                        UpdateScreen.status = "silent";
                        return;
                    }
                    AudioManager._clipRecord.GetData(recordData, talkStartPos);
                    AudioClip clip = AudioClip.Create("talkClip", micPos - talkStartPos + 1, AudioManager._clipRecord.channels, 44100, false);
                    clip.SetData(recordData, 0);
                    
                    //replay the clip if checkClip
                    if(checkClip) audioSource.PlayOneShot(clip);

                    //save the clip to a wav file
                    savWav.Save(Application.persistentDataPath + "/clip.wav", clip);

                    UpdateScreen.status = "converting:start";

                    //send a POST request to the Google TTP API.
                    pdp = Application.persistentDataPath;
                    txt = GameObject.Find("text").GetComponent<Text>().text;
                    Thread thread = new(async () => await GoogleRequest.GetTranscriptionAsync(pdp, txt));
                    thread.Start();
                }
            }
        }
        else
        {   
            //when user is talking
            isSilent = false;
            silentTimer = 0.0f;
            if(!TSPUpdated) talkStartPos = Microphone.GetPosition(null) - AudioManager._clipRecord.frequency;
            TSPUpdated = true;
            UpdateScreen.status = "listening";
        }
    }
}
