using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HashTest1 : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private HashMap<string, Action> actions;
    public int numbOfHashItems;

    public List<string> actionNames;
    public TextMeshProUGUI textMesh;
    public TextMeshProUGUI collectingFeedback;
    public TextMeshProUGUI commandList;
    public bool gettingNames = false;

    public GameObject startSphere;
    public GameObject middleSphere;
    public Transform environmentTransform;

    private int actionCount = 0;

    void Awake()
    {
        actions = new HashMap<string, Action>(numbOfHashItems);
    }

    void Start()
    {
        actions.put("forward", Forward);
        actions.put("top", Top);
        actions.put("down", Down);
        actions.put("back", Back);
        actions.put("left", Left);
        actions.put("right", Right);

        keywordRecognizer = new KeywordRecognizer(actions.getKeys().ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) && !gettingNames)
        {
            actionNames.Clear();
            actionCount = 0;
            eraseSpheres();

            textMesh.text = "Functions:\n";
            transform.position = Vector3.zero;

            collectingFeedback.gameObject.SetActive(true);
            commandList.gameObject.SetActive(true);

            keywordRecognizer.Start();
            gettingNames = true;
        }

        if ((Input.GetKeyDown(KeyCode.N) || actionNames.Count >= 7) && gettingNames)
        {
            collectingFeedback.gameObject.SetActive(false);
            commandList.gameObject.SetActive(false);

            keywordRecognizer.Stop();
            gettingNames = false;
            runActions();
        }
    }

    void eraseSpheres()
    {
        int childCount = environmentTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(environmentTransform.GetChild(i).gameObject);
        }
    }

    void runActions()
    {
        if (actionCount < actionNames.Count)
        {
            if (actionCount == 0)
            {
                GameObject tempGO = Instantiate(startSphere, environmentTransform);
                tempGO.transform.position = transform.position;
            }
            else
            {
                GameObject tempGO = Instantiate(middleSphere, environmentTransform);
                tempGO.transform.position = transform.position;
            }

            actions.get(actionNames[actionCount]).Invoke();

            actionCount++;
        }
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        textMesh.text += char.ToUpper(speech.text[0]) + speech.text.Substring(1) + "\n";
        actionNames.Add(speech.text);
    }

    private void Forward()
    {
        transform.DOMove(transform.position + new Vector3(1, 0, 0), 1).OnComplete(runActions);
    }

    private void Top()
    {
        transform.DOMove(transform.position + new Vector3(0, 1, 0), 1).OnComplete(runActions);
    }
    private void Back()
    {
        transform.DOMove(transform.position + new Vector3(-1, 0, 0), 1).OnComplete(runActions);
    }
    private void Down()
    {
        transform.DOMove(transform.position + new Vector3(0, -1, 0), 1).OnComplete(runActions);
    }
    private void Left()
    {
        transform.DOMove(transform.position + new Vector3(0, 0, 1), 1).OnComplete(runActions);
    }
    private void Right()
    {
        transform.DOMove(transform.position + new Vector3(0, 0, -1), 1).OnComplete(runActions);
    }
}
