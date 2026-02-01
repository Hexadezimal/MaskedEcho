using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject winImage;
    [SerializeField] private GameObject loseImage;

    void Start()
    {
        winImage.SetActive(false);
        loseImage.SetActive(false);

        if (DayManager.Instance == null)
            return;

        if (DayManager.Instance.Result == GameResult.Win)
        {
            winImage.SetActive(true);
        }
        else if (DayManager.Instance.Result == GameResult.Lose)
        {
            loseImage.SetActive(true);
        }
    }
}

