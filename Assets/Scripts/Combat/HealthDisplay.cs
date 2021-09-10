using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField]
    private Health health = null;
    [SerializeField]
    private GameObject healthBarParent = null;
    [SerializeField]
    private Image healthBarImage = null;

    private void Awake()
    {
        health.ClientOnHealthUpdated += HandleHealthUpdated;
        healthBarParent.SetActive(false);
    }

    private void OnDestroy()
    {
        health.ClientOnHealthUpdated -= HandleHealthUpdated;
    }

    private void OnMouseEnter()
    {
        healthBarParent.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (healthBarImage.fillAmount < 1) healthBarParent.SetActive(true);
        else healthBarParent.SetActive(false);
    }

    private void HandleHealthUpdated(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float) currentHealth / maxHealth;

        if (currentHealth == maxHealth) healthBarParent.SetActive(false);
        else healthBarParent.SetActive(true);
    }
}