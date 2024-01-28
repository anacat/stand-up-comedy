using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] TMP_Text tmpProText;
    string writer;
    [SerializeField] private Coroutine coroutine;

    [SerializeField] float delayBeforeStart = 0f;
    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] string leadingChar = "";
    [SerializeField] bool leadingCharBeforeDelay = false;
    [Space(10)][SerializeField] private bool startOnEnable = false;

    [Header("Collision-Based")]
    [SerializeField] private bool clearAtStart = false;
    [SerializeField] private bool startOnCollision = false;
    enum options { clear, complete }
    [SerializeField] options collisionExitOptions;

    [SerializeField] private UnityEvent typedEvent;
    public UnityEvent finishedEvent;

    // Use this for initialization
    void Awake()
    {
        if (tmpProText != null)
        {
            writer = tmpProText.text;
        }
    }

    void Start()
    {
        if (!clearAtStart) return;

        if (tmpProText != null)
        {
            tmpProText.text = "";
        }
    }

    private void OnEnable()
    {
        print("On Enable!");
        if (startOnEnable)
        {
            if (tmpProText != null)
            {
                writer = tmpProText.text;
            }

            StartTypewriter();
        }
    }

    private void StartTypewriter()
    {
        StopAllCoroutines();

        if (tmpProText != null)
        {
            tmpProText.text = "";

            StartCoroutine("TypeWriterTMP");
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator TypeWriterTMP()
    {
        tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        bool insideAngleBrackets = false;
        string currentText = "";

        foreach (char c in writer)
        {
            if (c == '<')
            {
                insideAngleBrackets = true;
                currentText = "";
            }
            else if (c == '>')
            {
                insideAngleBrackets = false;
                tmpProText.text += $"<{currentText}>";
            }
            else if (insideAngleBrackets)
            {
                currentText += c;
            }
            else
            {
                if (tmpProText.text.Length > 0)
                {
                    tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
                }
                tmpProText.text += c;
                tmpProText.text += leadingChar;

                typedEvent.Invoke();

                yield return new WaitForSeconds(timeBtwChars);
            }
        }

        if (leadingChar != "")
        {
            tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
        }


        finishedEvent.Invoke();
    }
}
