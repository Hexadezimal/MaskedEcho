using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Health, MaxHealth;
    public float burnDamage = 5f;
    public float burnInterval = 1f;
    public float Mana, MaxMana;
    public ScreenFader screenFader;

    [SerializeField] private HealthbarUI healthBar;
    [SerializeField] private ManaBarUI ManaBar;
    [SerializeField] private float waterManaCost = 20f;

    private bool isBurning;
    private Coroutine burnCoroutine;
    //private bool plantNearby;
    private Plants nearbyPlant;


    void Start()
    {
        isBurning = false;
        healthBar.SetMaxHealth(MaxHealth);
        healthBar.SetHealth(Health);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleBurning();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
                //ReduceMana();
                TryWaterPlant();
        }
    }
    void TryWaterPlant()
    {

        if (nearbyPlant == null)
            return;

        if (Mana < waterManaCost)
        {
            Debug.Log("Nicht genug Mana");
            return;
        }

        // Ziehe Mana **immer ab**, egal ob die Pflanze heilt oder nicht
        SetMana(-waterManaCost);

        // Pflanze heilt nur, wenn noch nicht gegossen
        nearbyPlant.Water();

    }


    public void ReduceMana()
    {
        SetMana(-20);
    }
    void ToggleBurning()
    {
        isBurning = !isBurning;

        if (isBurning)
        {
            burnCoroutine = StartCoroutine(BurnDamageOverTime());
        }
        else
        {
            if (burnCoroutine != null)
            {
                StopCoroutine(burnCoroutine);
                burnCoroutine = null;
            }
        }
    }

    IEnumerator BurnDamageOverTime()
    {
        while (isBurning)
        {
            SetHealth(-burnDamage);
            yield return new WaitForSeconds(burnInterval);
        }
    }

    public void SetHealth(float healthChange)
    {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);
    }

    public void SetMana(float manaChange)
    {
        Mana += manaChange;
        Mana = Mathf.Clamp(Mana, 0, MaxMana);

        ManaBar.SetMana(Mana);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("was triggered by: "+other.name);
        if (other.CompareTag("Plant"))
        {
            //plantNearby = true;
            Plants plant = other.GetComponent<Plants>();
            if (plant != null)
            {
                nearbyPlant = plant;
            }
        }
        else if (other.CompareTag("Shrine"))
        {

            screenFader.FadeOutAndIn();
            StartCoroutine(Sleep()); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("out of trigger area");
        //plantNearby = false;
        Plants plant = other.GetComponent<Plants>();
        if (plant == nearbyPlant)
        {
            nearbyPlant = null;
        }
    }

    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(1f);
        DayManager.Instance.NextDay();
        SetMana(MaxMana);
    }
}
