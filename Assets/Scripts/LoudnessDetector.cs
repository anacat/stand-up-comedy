using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudnessDetector : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microClip;

    // Start is called before the first frame update
    void Start()
    {
        MicrophoneToAudioClip();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MicrophoneToAudioClip()
    {
        string microName = Microphone.devices[0];
        microClip = Microphone.Start(microName, true, 20,AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFronMicro()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if(startPosition < 0)
        {
            return 0;
        }

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        //compute loudness
        float totalLoudness = 0;

        for(int i = 0; i < sampleWindow; i++) 
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
}
