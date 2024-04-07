using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float initialHealth;
    private float currentHealth;

    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioSource explosionSound;


    void Awake()
    {
        currentHealth = initialHealth;
    }

    // Update is called once per frame
    public void ApplyDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            explosionSound.Play();
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
