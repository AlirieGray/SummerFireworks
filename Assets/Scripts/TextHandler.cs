using UnityEngine;
using System.Collections.Generic;

public class TextHandler : MonoBehaviour
{
    private DisplayText[] texts;
    void Start()
    {
        texts = gameObject.GetComponentsInChildren<DisplayText>();
    }

    public void ResetAll()
    {
        foreach (var text in texts)
        {
            text.Reset();
        }
    }
}
