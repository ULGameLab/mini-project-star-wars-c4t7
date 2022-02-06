using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float MaxHealth;
    public float CurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = Mathf.Max(Mathf.Min(CurrentHealth, MaxHealth), 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHealth <= 0)
        {
            //Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth;
    }

}
