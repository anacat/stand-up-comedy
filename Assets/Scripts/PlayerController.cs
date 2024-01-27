using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Vector2 calibrationBounds;

    [Header("Chair")] public Vector2 sittingBounds;
    private BodySourceView _bodyView;

    private void Awake()
    {
        _bodyView = GetComponent<BodySourceView>();
    }

    private void Start()
    {
        StartCoroutine(WaitForCalibration());
    }

    public IEnumerator WaitForCalibration()
    {
        text.text = "Waiting for P1";

        yield return new WaitUntil(() => _bodyView.FoundPlayer());

        text.text = "get into position";

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

        text.text = "Perfect";
        
        yield return new WaitForSeconds(1f);

        StartCoroutine(SitOnChair());
    }

    public IEnumerator SitOnChair()
    {
        text.text = "Please sit";
        
        yield return new WaitForSeconds(1f);

        float distance = 0f;
        while (distance < sittingBounds.x || distance < sittingBounds.y)
        {
            Vector3 ancaPos = _bodyView.GetPlayerPosition();

            if (ancaPos != Vector3.zero)
            {
                distance = Camera.main.transform.position.y - ancaPos.y;
                text.text = $"{distance:0.0}";
            }

            yield return null;
        }

        text.text = "Perfect";

        yield return new WaitForSeconds(1f);

        StartCoroutine(NowGetUp());
    }

    public IEnumerator NowGetUp()
    {
        text.text = "Please get up";
        
        yield return new WaitForSeconds(1f);

        float distance = 0f;
        
        while (distance > 40f || distance == 0)
        {
            Vector3 ancaPos = _bodyView.GetPlayerPosition();

            if (ancaPos != Vector3.zero)
            {
                distance = Camera.main.transform.position.y - ancaPos.y;
                text.text = $"{distance:0.0}";
            }

            yield return null;
        }

        text.text = "Perfect";

        yield return new WaitForSeconds(1f);

        text.text = "Thank you for your time";
    }
}