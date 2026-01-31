using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBarUI : MonoBehaviour
{
    public float Mana, MaxMana, Width, Height;

    [SerializeField]
    private RectTransform manaBar;

    public void SetMaxMana(float maxMana)
    {
        MaxMana = maxMana;
    }

    public void SetMana(float health)
    {
        Mana = health;
        float newWidth = (Mana / MaxMana) * Width;

        manaBar.sizeDelta = new Vector2(newWidth, Height);
    }
}
