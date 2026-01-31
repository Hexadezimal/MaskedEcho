using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Health, MaxHealth;
    public float burnDamage = 5f;
    public float burnInterval = 1f;

    [SerializeField]
    private HealthbarUI healthBar;

    private bool isBurning;
    private Coroutine burnCoroutine;

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
}
