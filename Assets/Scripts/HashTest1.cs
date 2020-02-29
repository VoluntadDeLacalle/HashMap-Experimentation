using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


/// <summary>
/// Despite the name HashTest, this code was originally designed to be a prototype and nothing more. I liked the idea so much
/// I considered to expand it a bit and added a 3D demo. This class contains the code for the VoiceRecognition Scene and Demo.
/// </summary>
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

    /// <summary>
    /// Here is the initialization of the HashMap with keywords (strings of the words the KeywordRecognizer is to recognize later) and 
    /// Actions. The Actions are named the same as the functions that will be called later on when its keyword is spoken.
    /// After initializing the HashMap, the KeywordRecognizer is initialized using the list of keys from the HashMap.
    /// </summary>
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

    /// <summary>
    /// This Update function is used to start collecting spoken keywords when 'Y' is pressed, and ending this process, thus starting 
    /// the demo, when 'N' is pressed.
    /// </summary>
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

    /// <summary>
    /// Destroys all example "Path Spheres" created from the previous demo.
    /// </summary>
    void eraseSpheres()
    {
        int childCount = environmentTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(environmentTransform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Takes the list of actions obtained during the Update function and invokes all of the spoken commands after running through the
    /// list of functions (the actions listed when the HashMap was initialized) below. This function also spawns what I call a "Path Sphere"
    /// to show where the cube's path from beginning to finish during the demo.
    /// </summary>
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

    /// <summary>
    /// This is the listener that is needed by the KeywordRecognizer library. This stores all of the commands said in a list once the
    /// KeywordRecognizer is started.
    /// </summary>
    /// <param name="speech"></param>
    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        textMesh.text += char.ToUpper(speech.text[0]) + speech.text.Substring(1) + "\n";
        actionNames.Add(speech.text);
    }

    /// <summary>
    /// These next six functions all deal with moving the cube in the demo to a relative space corresponding with the
    /// command spoken.
    /// DOMove and OnComplete is a function of the DOTween library. These functions basically lerp the cube to its target postition relative
    /// to the cube's current position. Once the cube has reached its target position, it will return back to the runActions function and
    /// this process will be repeated until all of the commands spoken have been executed.
    /// </summary>
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
