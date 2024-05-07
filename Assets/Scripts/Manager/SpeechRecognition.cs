using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;
using System.Linq;

/// <summary>
/// windows only
/// </summary>
public class SpeechRecognition : MonoSingleton<SpeechRecognition>
{  
    public KeywordRecognizer keywordRecognizer;

    public Dictionary<string, UnityAction> keys = new Dictionary<string, UnityAction>();

    //定死的4个关键词
    private void Start()
    {
        keys.Add("旋转",null);
        keys.Add("发射",null);
        keys.Add("缩放",null);

        keywordRecognizer = new KeywordRecognizer(keys.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += PhraseRecongnized;
    }

    private void PhraseRecongnized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);

        MouseController.Instance.isConnect = false;
        keys[args.text].Invoke();
    }

    public void StartRecognize()
    {
        Debug.Log("开始语音识别");

        var kvp = keys.ToList();
        for(int i=0; i < kvp.Count; i++)
        {
            if (keys[kvp[i].Key] != null)
                keys[kvp[i].Key] = null;
        }
        keywordRecognizer.Start();
    }

    public void UnSubscribe()
    {
        if (keywordRecognizer.IsRunning)
            keywordRecognizer.Stop();

        var kvp = keys.ToList();
        for (int i = 0; i < kvp.Count; i++)
        {
            if (keys[kvp[i].Key] != null)
                keys[kvp[i].Key] = null;
        }
    }

    private void Update()
    {
        //测试用
        if (Input.GetKeyDown(KeyCode.F))
        {
            keys["发射"].Invoke();
            MouseController.Instance.MouseClick_1();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            keys["旋转"].Invoke();
            //MouseController.Instance.MouseClick_1();

        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            keys["缩放"].Invoke();
            //MouseController.Instance.MouseClick_1();

        }
    }
}
