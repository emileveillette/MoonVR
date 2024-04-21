using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float initialHealth;
    private float currentHealth;

    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private MeshRenderer meshRenderer;


    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        currentHealth = initialHealth;
    }

    // Update is called once per frame
    public void ApplyDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        meshRenderer.material.color = Color.Lerp(meshRenderer.material.color,Color.black, currentHealth / 100);
        if(currentHealth <= 0)
        {
            explosionSound.Play();
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
