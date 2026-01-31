using TMPro;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;
    public TMP_Text dayspassed;
    public TMP_Text maxDays;
    public int limitDays;

    public int CurrentDay { get; private set; } = 1;

    private void Awake()
    {
        maxDays.text = " / "+limitDays.ToString();
        dayspassed.text = "1";
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void NextDay()
    {
        CurrentDay++;
        Debug.Log("New Day: " + CurrentDay);
        dayspassed.text = CurrentDay.ToString();
    }
}