﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour {

    private string results;

    public String Results
    {
        get
        {
            return results;
        }
    }

    public WWW GET(string url, System.Action onComplete)
    {

        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www, onComplete));
        return www;
    }

    public WWW POST(string url, Dictionary<string, string> post, System.Action onComplete)
    {
        WWWForm form = new WWWForm();

        foreach (KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }

        WWW www = new WWW(url, form);

        StartCoroutine(WaitForRequest(www, onComplete));
        return www;
    }

    private IEnumerator WaitForRequest(WWW www, System.Action onComplete)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            results = www.text;
            onComplete();
        }
        else
        {
            Debug.Log(www.error);
        }
    }


    void Start ()
    {
        GET("http://scarounet.pythonanywhere.com/scores", ShowScores);
    }

    void ShowScores()
    {
        ScoreList scores = JsonUtility.FromJson<ScoreList>(results);

        foreach (var score in scores.Score)
        {
            Debug.Log(score.name + " " + score.score);
        }
    }

    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.A))
        {
            print("A");
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("username", "Scarounet");
            args.Add("score", "50");
            
            POST("http://scarounet.pythonanywhere.com/scores", args, OnPost);
        }
	}

    void OnPost()
    {
    }
}

