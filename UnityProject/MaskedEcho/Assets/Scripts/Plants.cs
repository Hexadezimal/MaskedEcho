using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{
    public GameObject Player;
    public GameObject RainDrops;
    public float Health, MaxHealth;

    [SerializeField] private float dryDamagePerDay = 10f;
    [SerializeField] private HealthbarUI healthBar;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material healthyMaterial;
    [SerializeField] private Material damagedMaterial;
    [SerializeField] private Material deadMaterial;

    private bool isDead;
    private bool isBurning;
    private float giveLife = 10f;
    private bool playerNearby;
    private float healthBeforeBurn;

    private int lastWateredDay = -1;



    void Start()
    {
        RainDrops.SetActive(false);
        healthBar.SetMaxHealth(MaxHealth);
        healthBar.SetHealth(Health);
        UpdateMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleBurning();
        }
    }

   /* public bool TryWater()
    {
        if (lastWateredDay == DayManager.Instance.CurrentDay)
            return false;

        lastWateredDay = DayManager.Instance.CurrentDay;
        SetHealth(giveLife);
        return true;
    }*/
    public bool CanBeWatered()
    {
        // Gibt nur true zurück, wenn die Pflanze noch nicht gegossen wurde heute
        return lastWateredDay != DayManager.Instance.CurrentDay;
    }

    public void Water()
    {
        if (isDead)
            return;

        if (CanBeWatered())
        {
            lastWateredDay = DayManager.Instance.CurrentDay;
            SetHealth(giveLife);

            if (RainDrops != null)
            {
                RainDrops.transform.position = transform.position;
                RainDrops.SetActive(true);
                StartCoroutine(LetItRain());
            }
        }
        // Wenn schon gegossen → nix passiert, aber der Spieler verliert trotzdem Mana
    }

    IEnumerator LetItRain()
    {
        yield return new WaitForSeconds(3);
        RainDrops.SetActive(false);

    }
    void ToggleBurning()
    {
        isBurning = !isBurning;

        if (isBurning)
        {
            healthBeforeBurn = Health;   // save current health
            SetHealth(-50);
        }
        else
        {
            Health = healthBeforeBurn;   // restore
            healthBar.SetHealth(Health);
            UpdateMaterial();
        }
    }

    public void SetHealth(float healthChange)
    {
        if (isDead)
            return;

        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);
        UpdateMaterial();

        if (Health <= 0)
        {
            Die();
        }
    }

    void UpdateMaterial()
    {
        if (isDead)
            return;

        if (Health >= MaxHealth)
        {
            meshRenderer.material = healthyMaterial;
        }
        else
        {
            meshRenderer.material = damagedMaterial;
        }
    }
    public void GetWatered()
    {
        //SetHealth(giveLife);
        if (lastWateredDay == DayManager.Instance.CurrentDay)
        {
            Debug.Log("Plant already watered today");
            return;
        }

        // Accept water
        lastWateredDay = DayManager.Instance.CurrentDay;
        SetHealth(giveLife);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Was triggered by " + other.name);
        if (other.name == "Player")
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("out of trigger area");
        playerNearby = false;
    }

    void Die()
    {
        isDead = true;

        Health = 0;
        healthBar.SetHealth(0);

        meshRenderer.material = deadMaterial;

        Debug.Log($"{gameObject.name} ist gestorben... du magst wohl deine Pflanzen nicht besonders...schäm dich");
    }

    public void OnNewDay(int currentDay)
    {
        if (lastWateredDay == -1)
            return; // noch nie gegossen → kein Schaden

        int daysWithoutWater = currentDay - lastWateredDay - 1;

        if (daysWithoutWater >= 2)
        {
            float damage = dryDamagePerDay * (daysWithoutWater - 1);
            SetHealth(-damage);
        }
    }
}
