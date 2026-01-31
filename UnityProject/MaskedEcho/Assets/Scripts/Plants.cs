using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{
    public GameObject Player;
    public float Health, MaxHealth;


    [SerializeField] private HealthbarUI healthBar;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material healthyMaterial;
    [SerializeField] private Material damagedMaterial;



    private bool isBurning;
    private float giveLife = 5f;
    private bool playerNearby;
    private float healthBeforeBurn;


    void Start()
    {
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(playerNearby == true)
            {
                GetWatered();
            }

        }
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
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);
        Debug.Log(Health);
        UpdateMaterial();

    }

    void UpdateMaterial()
    {
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
}
