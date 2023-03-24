using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine.UI;

public class GoogleRequest : MonoBehaviour
{
    static bool log =  true;
    public static async Task<string> GetTranscriptionAsync(string persistentDataPath, string text)
    {
        try
        {
            string wav = Convert.ToBase64String(File.ReadAllBytes(persistentDataPath + "/clip.wav"));
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://speech.googleapis.com/v1/speech:recognize?key=API_KEY"))
                {
                    request.Content = new StringContent(text);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    if (log) Debug.Log("Posting...");
                    var response = await httpClient.PostAsync("https://speech.googleapis.com/v1/speech:recognize?key=API_KEY",
                        new StringContent(text.Replace("b64con", wav)));
                    string content1 = await response.Content.ReadAsStringAsync();
                    string content2 = content1.Split("\"transcript\": \"")[1];
                    string content = content2.Split("\",")[0];
                    Debug.Log("Content: " + content);
                    UpdateScreen.screenContent += "\n" + content;
                    return null;
                }
            }
        }
        catch (FileNotFoundException e) { Debug.Log(e); return null; }
        catch (IOException ex) { Debug.Log(ex); }
        catch (SocketException ex2) { Debug.Log(ex2); }
        return null;
    }
}
