using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        GameEvents.OnUpdateCountdownStatus += GameEvents_OnUpdateCountdownStatus;
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateCountdownStatus -= GameEvents_OnUpdateCountdownStatus;
    }

    private void GameEvents_OnUpdateCountdownStatus(string status)
    {
        _text.text = status;
    }
}
