using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class HashTest1 : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private HashMap<string, Action> actions;
    public int numbOfHashItems;

    void Awake()
    {
        actions = new HashMap<string, Action>(numbOfHashItems);
    }

    void Start()
    {
        actions.put("forward", Forward);
        actions.put("up", Up);
        actions.put("down", Down);
        actions.put("back", Back);

        keywordRecognizer = new KeywordRecognizer(actions.getKeys().ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions.get(speech.text).Invoke();
    }

    private void Forward()
    {
        transform.Translate(1, 0, 0);
    }

    private void Up()
    {
        transform.Translate(0, 1, 0);
    }
    private void Back()
    {
        transform.Translate(-1, 0, 0);
    }
    private void Down()
    {
        transform.Translate(0, -1, 0);
    }
}
