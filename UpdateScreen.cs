using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScreen : MonoBehaviour
{
    //silent, listening, loading, converting:{details}
    //not used
    public static string status = "silent";
    public static string newContent = "";
    public GameObject audioManager;
    public static int textObjectNum = 0; //number of TO. (not an index)
    public GameObject textObjectPrefab;
    public static bool isSilent;
    static readonly string[] preKeywords = { "있어", "써", "사", "만들어", "있었", "가져", "해", "사" };
    static readonly string[] lastKeywords = { "야", "음", "으면", "가야", "갔으" };
    static readonly string[] wholeKeywords = { "필요", "구매", "야돼", "야되", "야됩", "야됨", "면좋겠", "있으면", "준비", "숙제", "제출" };
    void Update()
    {
        //if (gameObject.name.Equals("Screen2")) gameObject.GetComponent<Text>().text
        //        = $"Decibel : {AudioManager.GetComponent<RecordAudio>().db}   Status : {status}";
        // not used due to a bug.
        if (gameObject.name.Equals("Screen2"))
        {
            if (isSilent)
            {
                gameObject.GetComponent<Text>().text = "대기 중입니다.";
            }
            else
            {
                gameObject.GetComponent<Text>().text = "듣는 중입니다.";
            }
            //gameObject.GetComponent<Text>().text = ((int)AudioManager.MicLoudnessinDecibels + 130) + "";
            //gameObject.GetComponent<Text>().text = $"Status : {status}";
        }


        if (!newContent.Equals(""))
        {
            //if GoogleRequest got a valid response

            //check if text includes one of the keywords
            //keywords are in wholeKeywords or preKeywords+lastKeywords
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

            //$system$ means it's a system message and it's hardcoded to pass the keyword detection.
            if (!includesKeyword && !text.Contains("$system$")) { status = "silent"; return; }
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
    }
}
