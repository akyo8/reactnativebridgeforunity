using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public class Word
    {
        public string[] data;
    }

    public Text messageText;

    // Use this for initialization
    void Awake()
    {
        instance = this;
        UnityMessageManager.Instance.OnRNMessage += OnMessage;
    }

    void onDestroy()
    {
        UnityMessageManager.Instance.OnRNMessage -= OnMessage;
    }

    // Update is called once per frame
    void Update()
    {
    }

	void OnMessage(MessageHandler message)
    {
        var msgName = message.name;
        var json = message.getData<string>();

        messageText.text = "Words Received:\n";
        Word word1 = JsonUtility.FromJson<Word>(json);
        Word word2 = new Word();
        word2.data = new string[2];

        for (int i = 0; i < word1.data.Length; i++)
        {
            if (i == 0)
                messageText.text += word1.data[i];
            else
                messageText.text += ", " + word1.data[i];

            if (i < 2)
                word2.data[i] = word1.data[i];
        }

        UnityMessageManager.Instance.SendMessageToRN(new UnityMessage()
        {
            name = "onSendWords",
            data = JObject.Parse(JsonUtility.ToJson(word2)),
            callBack = (dt) =>
            {
                Debug.Log("onClickCallBack:" + dt);
            }
        });

        message.send(new { CallbackTest = "I am Unity callback" });
    }
}
