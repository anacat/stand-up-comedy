using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScaleFromAudio : MonoBehaviour
{
    public float maxValue;
    public LoudnessDetector detector;

    public float loudnessSensibility = 10;
    public float threshhold = 0.1f;

    public AsciiSlider slider;

    private float score;
    private bool scoring;
    private float finalScore;

    private void Start()
    {
        scoring = false;
        finalScore = 0;
    }

    void FixedUpdate()
    {
        float loudness = detector.GetLoudnessFronMicro() * loudnessSensibility;

        if (loudness < threshhold)
            loudness = 0;

        //transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);

        float value = loudness * maxValue;

        slider.SetValue((int)value + 1);

        if(scoring)
            score += value;
    }


    public void StartScoring()
    {
        score = 0;
        scoring = true;
    }

    public float StopRecording()
    {
        scoring = false;
        finalScore += score;
        return score;
    }

    public float GetFinalScore()
    {
        return finalScore;
    }
}
