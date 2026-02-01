using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{
    public GameObject Player;
    public GameObject RainDrops;
    public float Health, MaxHealth;
    //public GameObject letsrainText;
    public AudioSource rainSound;

    [SerializeField] private float dryDamagePerDay = 10f;
    [SerializeField] private HealthbarUI healthBar;
    //[SerializeField] private MeshRenderer meshRenderer;
    //[SerializeField] private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    [Header("Plant Meshes")]
    [SerializeField] private List<MeshRenderer> plantRenderers = new List<MeshRenderer>();
    [Header("Fire Meshes")]
    [SerializeField] private List<MeshRenderer> fireRenderers = new List<MeshRenderer>();
    [SerializeField] private Material healthyMaterial;
    [SerializeField] private Material damagedMaterial;
    [SerializeField] private Material deadMaterial;
    [SerializeField] private Material fireOnMaterial;



    private bool isDead;
    private bool isBurning;
    private float giveLife = 10f;
    private bool playerNearby;
    private float healthBeforeBurn;
    private bool isPreviewMode;
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
                Vector3 offset = new Vector3(0f, 0.7f, -0.2f); // nach oben
                RainDrops.transform.position = transform.position + offset;
                //RainDrops.transform.position = transform.position;
                RainDrops.SetActive(true);
                if (rainSound != null && !rainSound.isPlaying)
                    rainSound.Play();
                StartCoroutine(LetItRain());
            }
        }
        // Wenn schon gegossen → nix passiert, aber der Spieler verliert trotzdem Mana
    }

    IEnumerator LetItRain()
    {
        yield return new WaitForSeconds(3);
        RainDrops.SetActive(false);
        if (rainSound != null && rainSound.isPlaying)
            rainSound.Stop();

    }
    /*void ToggleBurning()
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
    }*/
    void ToggleBurning()
    {
        isBurning = !isBurning;

        if (isBurning)
        {
            healthBeforeBurn = Health;
            SetHealth(-50);
        }
        else
        {
            Health = healthBeforeBurn;
            healthBar.SetHealth(Health);
        }

        UpdateMaterial();
    }


    /*public void SetHealth(float healthChange)
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
    }*/

    public void SetHealth(float healthChange)
    {
        if (isDead && !isPreviewMode)
            return;

        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);
        UpdateMaterial();

        if (Health <= 0 && !isPreviewMode)
        {
            Die();
        }
    }


    /*void UpdateMaterial()
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
    }*/

    /*void UpdateMaterial()
    {
        if (!isPreviewMode && isDead)
            return;

        if (Health <= 0)
        {
            meshRenderer.material = deadMaterial;
        }
        else if (Health >= MaxHealth)
        {
            meshRenderer.material = healthyMaterial;
        }
        else
        {
            meshRenderer.material = damagedMaterial;
        }
    }*/

    /* void UpdateMaterial()
     {
         Material targetMaterial;

         if (isDead)
         {
             targetMaterial = deadMaterial;
         }
         else if (Health >= MaxHealth)
         {
             targetMaterial = healthyMaterial;
         }
         else
         {
             targetMaterial = damagedMaterial;
         }

         foreach (MeshRenderer renderer in meshRenderers)
         {
             if (renderer != null)
             {
                 renderer.material = targetMaterial;
             }
         }
     }*/

    void UpdateMaterial()
    {
        // 🌿 Pflanzen-Material
        Material plantMaterial;

        if (isDead)
        {
            plantMaterial = deadMaterial;
        }
        else if (Health >= MaxHealth)
        {
            plantMaterial = healthyMaterial;
        }
        else
        {
            plantMaterial = damagedMaterial;
        }

        foreach (MeshRenderer renderer in plantRenderers)
        {
            if (renderer != null)
                renderer.material = plantMaterial;
        }

        // 🔥 Feuer-Material
        foreach (MeshRenderer renderer in fireRenderers)
        {
            if (renderer == null)
                continue;

            if (isBurning)
            {
                renderer.enabled = true;
                renderer.material = fireOnMaterial;
            }
            else
            {
                renderer.enabled = false; // komplett unsichtbar
            }
        }
    }

    public void EnterFuturePreview()
    {
        isPreviewMode = true;
    }

    public void ExitFuturePreview()
    {
        isPreviewMode = false;

        // Zustand neu bewerten
        isDead = Health <= 0;
        UpdateMaterial();
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
            //letsrainText.SetActive(true);
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("out of trigger area");
        //letsrainText.SetActive(true);
        playerNearby = false;
    }

    void Die()
    {
        isDead = true;

        Health = 0;
        healthBar.SetHealth(0);

        UpdateMaterial();

        Debug.Log($"{gameObject.name} ist gestorben... du magst wohl deine Pflanzen nicht besonders... schäm dich");
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
