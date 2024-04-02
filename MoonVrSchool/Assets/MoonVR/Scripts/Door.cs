using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    public List<GameObject> crytals = new List<GameObject>();
    private bool doorOpened;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < crytals.Count; ++i) 
        {
            if (crytals[i] == null)
                crytals.RemoveAt(i);
        }
        if (!doorOpened && crytals.Count == 0)
        {
            OpenDoor();
            doorOpened = true;
        }
    } 

    private void OpenDoor()
    {
        animator.SetTrigger("OpenDoor");
        Debug.Log("Door Opened");
    }
}
