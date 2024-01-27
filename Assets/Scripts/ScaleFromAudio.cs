using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromAudio : MonoBehaviour
{
    public float maxValue;
    public LoudnessDetector detector;

    public float loudnessSensibility = 10;
    public float threshhold = 0.1f;

    public AsciiSlider slider;

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFronMicro() * loudnessSensibility;

        if (loudness < threshhold)
            loudness = 0;

        //transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);

        float value = loudness * maxValue;

        slider.SetValue((int)value + 1);
    }
}
