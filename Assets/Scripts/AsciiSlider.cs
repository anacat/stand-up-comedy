using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class AsciiSlider : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> bars;

    public void SetValue(int value)
    {
        value = Mathf.Clamp(value, 0, 12);

        for (int i = 0; i < bars.Count; i++)
        {
            bars[i].SetActive(false);
        }
        for (int i = 0; i < value; i++)
        {
            bars[i].SetActive(true);
        }
    }
}
