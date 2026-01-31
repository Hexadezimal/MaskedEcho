using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Health, MaxHealth;
    public float burnDamage = 5f;
    public float burnInterval = 1f;
    public float Mana, MaxMana;


    [SerializeField]
    private HealthbarUI healthBar;
    [SerializeField] 
    private ManaBarUI ManaBar;

    private bool isBurning;
    private Coroutine burnCoroutine;
    private bool plantNearby;

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
            if (plantNearby == true)
            {
                ReduceMana();
            }
        }
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
        if (other.CompareTag("Plant"))
        {
            plantNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("out of trigger area");
        plantNearby = false;
    }
}
