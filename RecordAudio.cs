using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RecordAudio : MonoBehaviour
{
    public float silentTimer = 0.0f;
    public bool isSilent = false;
    public float silentDbLimit = -55.0f;
    public float db;
    public float silentTimeLimit = 1.5f;
    public int talkStartPos;
    public int micPos;
    float[] recordData;
    bool TSPUpdated = false;
    AudioSource audioSource;
    public float startTimer = 1.0f;
    SavWav savWav = new SavWav();
    public bool checkClip = false;
    void Start()
    {
        Application.RequestUserAuthorization(UserAuthorization.Microphone);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (startTimer > 0.0f) { startTimer -= Time.deltaTime; return; }
        micPos = Microphone.GetPosition(null);
        db = AudioManager.MicLoudnessinDecibels;
        if (db < silentDbLimit)
        {
            silentTimer += Time.deltaTime;
            if (silentTimer > silentTimeLimit)
            {
                silentTimer = silentTimeLimit;
                TSPUpdated = false;
                if (!isSilent)
                {
                    recordData = new float[micPos - talkStartPos + 1];
                    AudioManager._clipRecord.GetData(recordData, talkStartPos);
                    AudioClip clip = AudioClip.Create("talkClip", micPos - talkStartPos + 1, AudioManager._clipRecord.channels, 44100, false);
                    clip.SetData(recordData, 0);
                    if(checkClip) audioSource.PlayOneShot(clip);
                    isSilent = true;
                    Debug.Log("Saving Clip...");
                    savWav.Save(Application.persistentDataPath + "/clip.wav", clip);
                    Debug.Log("Clip Saved.");
                    Debug.Log(GoogleRequest.GetTranscription());
                }
            }
        }
        else
        {
            isSilent = false;
            silentTimer = 0.0f;
            if(!TSPUpdated) talkStartPos = Microphone.GetPosition(null) - AudioManager._clipRecord.frequency;
            TSPUpdated = true;
        }
    }
}
