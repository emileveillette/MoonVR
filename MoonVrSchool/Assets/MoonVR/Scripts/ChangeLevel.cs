using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField]  private CharacterController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other == player)
        {
            SceneManager.LoadScene("MoonMenu", LoadSceneMode.Single);
        }

    }
}
