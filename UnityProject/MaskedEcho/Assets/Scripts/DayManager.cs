using TMPro;
using UnityEngine;

/*public class DayManager : MonoBehaviour
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
}*/
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    public TMP_Text daysPassedText;
    public TMP_Text maxDaysText;

    public int limitDays = 7;
    public int CurrentDay { get; private set; } = 1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void NextDay()
    {
        CurrentDay++;
        UpdateUI();

        if (CurrentDay > limitDays)
        {
            CheckWinCondition();
        }
    }

    void UpdateUI()
    {
        daysPassedText.text = CurrentDay.ToString();
        maxDaysText.text = limitDays.ToString();
    }

    void CheckWinCondition()
    {
        Plants[] allPlants = FindObjectsOfType<Plants>();

        foreach (Plants plant in allPlants)
        {
            float healthPercent = plant.Health / plant.MaxHealth;

            if (healthPercent < 0.7f)
            {
                LoseGame();
                return;
            }
        }

        WinGame();
    }

    void WinGame()
    {
        Debug.Log("GEWONNEN!--> Szenenwechsel");
        //SceneManager.LoadScene("WinScene");
    }

    void LoseGame()
    {
        Debug.Log("VERLOREN!--> Szenenwechsel");
        //SceneManager.LoadScene("LoseScene");
    }

}