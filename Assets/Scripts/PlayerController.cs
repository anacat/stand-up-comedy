using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float calibrationDistance;
    
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
        yield return new WaitUntil(() => _bodyView.FoundPlayer());
        
        Debug.Log("there they be");

        float distance = 0f;

        while (distance < calibrationDistance)
        {
            Vector3 bodyPos =  _bodyView.GetPlayerPosition();
            distance = Vector3.Distance(bodyPos, Camera.main.transform.position);

            Debug.Log(distance);
            
            yield return null;
        }
        
        Debug.Log("perfect");
    }
}
