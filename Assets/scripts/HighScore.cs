using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighScore : MonoBehaviour
{
    private Text highscore;

    private void OnEnable()
    {
        highscore = GetComponent<Text>();
        highscore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
}
