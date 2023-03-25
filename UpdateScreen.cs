using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScreen : MonoBehaviour
{
    //silent, listening, loading, converting:{details}
    public static string status = "silent";
    public static string newContent = "";
    public GameObject AudioManager;
    public static int textObjectNum = 0; //number of TO. (not an index)
    public GameObject textObjectPrefab;
    static readonly string[] preKeywords = { "�־�", "��", "��", "�����", "�־�" };
    static readonly string[] lastKeywords = { "��", "��", "����" };
    static readonly string[] wholeKeywords = { "���", "�ʿ�", "����", "�ߵ�", "�ߵǴµ�", "�ߵ�", "������", "������" };
    void Update()
    {
        if (!newContent.Equals(""))
        {
            //if GoogleRequest got a valid response

            //check if text includes one of the keywords
            string text = newContent.Replace(" ", "");
            bool includesKeyword = false;
            foreach (string pre in preKeywords)
            {
                foreach (string last in lastKeywords)
                {
                    if(text.Contains(pre + last)) includesKeyword = true; 
                }
            }
            foreach (string key in wholeKeywords)
            {
                if (text.Contains(key)) includesKeyword = true;
            }
            if (!includesKeyword && !text.Contains("$system$")) { UpdateScreen.status = "silent"; return; }
            newContent = newContent.Replace("$system$", "");

            //Instantiate new TextObject
            GameObject top = Instantiate(textObjectPrefab);
            top.name = $"TextSquare{textObjectNum}";
            top.transform.position = new Vector3(0, 4.3f - (textObjectNum * 1.3f), 0);
            top.GetComponentInChildren<Text>().text = newContent;

            textObjectNum++;
            newContent = "";

            //test script
            //GameObject.Find("Screen").GetComponent<Text>().text += "\n" + newContent;
        }


        //if (gameObject.name.Equals("Screen2")) gameObject.GetComponent<Text>().text
        //        = $"Decibel : {AudioManager.GetComponent<RecordAudio>().db}   Status : {status}";
        if (gameObject.name.Equals("Screen2")) gameObject.GetComponent<Text>().text
                = $"Status : {status}";
    }
}
