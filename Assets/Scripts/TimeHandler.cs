using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    [SerializeField]
    private Text timeText;
    bool isplaying = false;
    private float _gameDuration;

    private float timeRemaining;
    private GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
    void Update()
    {
        if(isplaying)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
                timeText.text = (int)timeRemaining + "s";
            }
            else
            {
                gameController.EndTime();
                isplaying = false;
            }
        }    
    }

    public void StartTime(float gameDuration)
    {
        this.timeRemaining = gameDuration;
        _gameDuration = gameDuration;
        this.isplaying = true;
    }

    public void StopTime()
    {
        this.isplaying = false;
        timeText.text = (int)_gameDuration + "s";
    }
}
