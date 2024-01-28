using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public int movesScore;
    public int finalMovesScore;
    public ChatManager chat;
    public Vector2 calibrationBounds;
    public int standUpTime = 10;
    public float messageDelay = 2;
    public ScaleFromAudio audio;

    [Header("Chair")] public Vector2 sittingBounds;
    private BodySourceView _bodyView;
    private int currentStand;

    [Header("Messages")]
    [SerializeField] private Message lookingForPlayer;
    [SerializeField] private Message moveIntoPosition;
    [SerializeField] private Message arrivedInPosition;
    [SerializeField] private Message sitDown;
    [SerializeField] private Message satDownSuccess;
    [SerializeField] private List<Message> standUpMessages;
    [SerializeField] private Message wellDone;
    [SerializeField] private Message finished;
    [SerializeField] private Message calculating;
    [SerializeField] private Message audienceScore;
    [SerializeField] private List<Message> robotScore;
    [SerializeField] private Message initialRobotScore;

    [Header("Actions")]
    [SerializeField] private List<StandUpAction> colliderActions;
    [SerializeField] private List<StandUpAction> flavourActions;

    private void Awake()
    {
        instance = this;
        _bodyView = GetComponent<BodySourceView>();
    }

    private void Start()
    {

    }

    public void StartGame()
    {
        StartCoroutine(WaitForCalibration());

        currentStand = 0;
        finalMovesScore = 0;
    }

    public IEnumerator WaitForCalibration()
    {
        chat.WriteMessage(lookingForPlayer);
        yield return new WaitForSeconds(messageDelay);

        yield return new WaitUntil(() => _bodyView.FoundPlayer());

        chat.WriteMessage(moveIntoPosition);
        yield return new WaitForSeconds(messageDelay);

        float distance = 0f;

        while (distance < calibrationBounds.x || distance < calibrationBounds.x)
        {
            Vector3 ancaPos = _bodyView.GetPlayerPosition();

            if (ancaPos != Vector3.zero)
            {
                distance = ancaPos.z - Camera.main.transform.position.z;
            }

            yield return null;
        }

        chat.WriteMessage(arrivedInPosition);
        yield return new WaitForSeconds(messageDelay);

        StartCoroutine(SitOnChair());
    }

    public IEnumerator SitOnChair()
    {
        chat.WriteMessage(sitDown);
        yield return new WaitForSeconds(messageDelay);

        float distance = 0f;
        while (distance < sittingBounds.x || distance < sittingBounds.y)
        {
            Vector3 ancaPos = _bodyView.GetPlayerPosition();

            if (ancaPos != Vector3.zero)
            {
                distance = Camera.main.transform.position.y - ancaPos.y;
            }

            yield return null;
        }

        chat.WriteMessage(satDownSuccess);
        yield return new WaitForSeconds(messageDelay);

        yield return new WaitForSeconds(1f);

        StartCoroutine(NowGetUp());
    }

    public IEnumerator NowGetUp()
    {
        movesScore = 0;

        //GenerateModifier
        List<StandUpAction> actions = new List<StandUpAction>();
        for(int i = 0; i < currentStand; i++)
        {
            if(currentStand % 2 == 0)
            {
                actions.Add(flavourActions[UnityEngine.Random.Range(0, flavourActions.Count)]);
            }
            else
            {
                actions.Add(colliderActions[UnityEngine.Random.Range(0, colliderActions.Count)]);
            }
        }

        chat.WriteModifiedMessage(standUpMessages[currentStand], actions);
        yield return new WaitForSeconds(messageDelay);

        audio.StartScoring();

        float distance = 0f;
        
        while (distance > 40f || distance == 0)
        {
            Vector3 ancaPos = _bodyView.GetPlayerPosition();

            if (ancaPos != Vector3.zero)
            {
                distance = Camera.main.transform.position.y - ancaPos.y;
            }

            yield return null;
        }

        chat.WriteMessage(calculating);
        yield return new WaitForSeconds(standUpTime);

        float audScore = audio.StopRecording();
        finalMovesScore += movesScore;

        audienceScore.message = audienceScore.message.Replace("{score}", audScore.ToString());
        int rand = UnityEngine.Random.Range(0, robotScore.Count);
        robotScore[rand].message = robotScore[rand].message.Replace("{score}", movesScore.ToString());

        //subsittuir por maquina rating
        if(currentStand == 0)
        {
            chat.WriteMessage(initialRobotScore);
        }
        else
        { chat.WriteMessage(robotScore[rand]); }
        chat.WriteMessage(audienceScore);
        yield return new WaitForSeconds(messageDelay);

        if(currentStand < standUpMessages.Count - 1)
        {
            StartCoroutine(NowGetUp());
            currentStand++;
            FindObjectOfType<CreateColliders>().gameObject.SetActive(true);
        }
        else
        {
            finished.message = finished.message.Replace("{player}", audio.GetFinalScore().ToString()).Replace("{robot}", finalMovesScore.ToString());

            chat.WriteMessageEnd(finished);
        }
    }
}