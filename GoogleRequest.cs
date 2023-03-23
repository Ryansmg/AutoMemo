using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;

public class GoogleRequest : MonoBehaviour
{
    public static string GetTranscription()
    {

        FileStream fileStream = File.OpenRead(Application.persistentDataPath + "/clip.wav");
        MemoryStream memoryStream = new MemoryStream();
        memoryStream.SetLength(fileStream.Length);
        fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);
        byte[] BA_AudioFile = memoryStream.GetBuffer();
        HttpWebRequest _HWR_SpeechToText;
        _HWR_SpeechToText =
                    (HttpWebRequest)WebRequest.Create(
                        "https://www.google.com/speech-api/v2/recognize?output=json&lang=ko-kr&key=API_KEY");
        _HWR_SpeechToText.Credentials = CredentialCache.DefaultCredentials;
        _HWR_SpeechToText.Method = "POST";
        _HWR_SpeechToText.ContentType = "audio.wav; rate=44100";
        _HWR_SpeechToText.ContentLength = BA_AudioFile.Length;
        _HWR_SpeechToText.KeepAlive = false;
        Stream stream = _HWR_SpeechToText.GetRequestStream();
        stream.Write(BA_AudioFile, 0, BA_AudioFile.Length);
        stream.Close();

        HttpWebResponse HWR_Response = (HttpWebResponse)_HWR_SpeechToText.GetResponse();
        if (HWR_Response.StatusCode == HttpStatusCode.OK)
        {
            StreamReader SR_Response = new StreamReader(HWR_Response.GetResponseStream());
            return SR_Response.ReadToEnd();
        }


        Debug.Log("Error");
        return null;
    }
}
