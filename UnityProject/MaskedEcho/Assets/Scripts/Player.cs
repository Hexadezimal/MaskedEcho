using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float Health, MaxHealth;
    public float burnDamage = 5f;
    public float burnInterval = 1f;
    public float Mana, MaxMana;
    public ScreenFader screenFader;
    public GameObject playerMask;
    public GameObject normalMask;
    public GameObject noManaText;

    [SerializeField] private HealthbarUI healthBar;
    [SerializeField] private ManaBarUI ManaBar;
    [SerializeField] private float waterManaCost = 20f;
    [SerializeField] private MeshRenderer meshRendererBG;
    [SerializeField] private MeshRenderer meshRendererFloor;
    [SerializeField] private MeshRenderer meshRendererBGSideWall_Left;
    [SerializeField] private MeshRenderer meshRendererBGSideWall_Right;
    [SerializeField] private Material BurningForestBGMaterial;
    [SerializeField] private Material NormalForestBGMaterial;
    [SerializeField] private Material BurningForestFloorMaterial;
    [SerializeField] private Material NormalForestFloorMaterial;

    private bool isBurning;
    private Coroutine burnCoroutine;
    //private bool plantNearby;
    private Plants nearbyPlant;
    private bool isDead = false;



    void Start()
    {
        isBurning = false;
        healthBar.SetMaxHealth(MaxHealth);
        healthBar.SetHealth(Health);
        meshRendererBG.material = NormalForestBGMaterial;
        meshRendererFloor.material = NormalForestFloorMaterial;
        meshRendererBGSideWall_Left.material = NormalForestBGMaterial;
        meshRendererBGSideWall_Right.material = NormalForestBGMaterial;
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
            noManaText.SetActive(true);
            StartCoroutine(NoManaGoSleep());
            Debug.Log("Nicht genug Mana");
            return;
        }

        // Ziehe Mana **immer ab**, egal ob die Pflanze heilt oder nicht
        SetMana(-waterManaCost);

        // Pflanze heilt nur, wenn noch nicht gegossen
        nearbyPlant.Water();

    }

    IEnumerator NoManaGoSleep()
    {
        yield return new WaitForSeconds(3);
        noManaText.SetActive(false);
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
            playerMask.SetActive(true);
            normalMask.SetActive(false);
            burnCoroutine = StartCoroutine(BurnDamageOverTime());
            UpdateMaterial();
        }
        else
        {
            if (burnCoroutine != null)
            {
                playerMask.SetActive(false);
                normalMask.SetActive(true);
                UpdateMaterial();
                StopCoroutine(burnCoroutine);
                burnCoroutine = null;
            }
        }
    }

    void UpdateMaterial()
    {
        if (isBurning)
        {
            meshRendererBG.material = BurningForestBGMaterial;
            meshRendererBGSideWall_Left.material = BurningForestBGMaterial;
            meshRendererBGSideWall_Right.material = BurningForestBGMaterial;
            meshRendererFloor.material = BurningForestFloorMaterial;
        }
        else
        {
            meshRendererBG.material = NormalForestBGMaterial;
            meshRendererBGSideWall_Left.material = NormalForestBGMaterial;
            meshRendererBGSideWall_Right.material = NormalForestBGMaterial;
            meshRendererFloor.material = NormalForestFloorMaterial;
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
        /*Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);*/

        if (isDead)
            return;

        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);

        if (Health <= 0)
        {
            Die();
        }
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
        if (Health < 100)
        {
            SetHealth(20);
        }
    }

    void Die()
    {
        isDead = true;

        // Brennen stoppen
        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
            burnCoroutine = null;
        }

        Debug.Log("Game Over --> Szenenwechsel");

        // Szene wechseln
        //SceneManager.LoadScene("GameOver");
    }
}
