using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class SendSlackMessage : MonoBehaviour
{
    string _url = "https://hooks.slack.com/services/XXXXXXXXXXXXXXX"; // スラックAPIでURLを取得する
    string username = "username"; // 通知時のユーザー名

    public void CreateMessage(string username,string message)
    {
        SlackMessageData messageData = new SlackMessageData
        {
            username = username,
            text = message
        };
        
        StartCoroutine(Request(messageData));
    }

    public void CreateErrorMessage(string error_message, string occured_method_name)
    {
        var attachment = new Attachment
        {
            fallback = occured_method_name,
            color = "#D00000",
            pretext = occured_method_name,
            text = error_message,
        };

        SlackMessageData messageData = new SlackMessageData
        {
            username = this.username,
            attachments = new Attachment[] { attachment }
        };

        StartCoroutine(Request(messageData));
    }

    private IEnumerator Request(SlackMessageData data)
    {
        var content = JsonUtility.ToJson(data);
        using (UnityWebRequest request = new UnityWebRequest(_url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(content));
            request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("success");
            }
        }
    }
}


[Serializable]
class SlackMessageData
{
    public string channel;
    public string username;
    public string text;
    public string icon_emoji;
    public string icon_url;
    public Attachment[] attachments;
}

[Serializable]
public sealed class Attachment
{
    public string fallback;
    public string color;
    public string pretext;
    public string author_name;
    public string author_link;
    public string author_icon;
    public string title;
    public string title_link;
    public string text;
    public Field[] fields;
    public string image_url;
    public string thumb_url;
    public string footer;
    public string footer_icon;
    public string ts;
    public string[] mrkdwn_in;
}

[Serializable]
public sealed class Field
{
    public string title;
    public string value;
    public string @short;
}