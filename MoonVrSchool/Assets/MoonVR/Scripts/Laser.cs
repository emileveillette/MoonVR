using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Laser : MonoBehaviour
{
    [SerializeField] private GameObject beamRight;

    RaycastHit rightRaycastHit;
    [SerializeField] private float damage;
    [SerializeField] private InputActionReference rightTrigger;

    [SerializeField] private ParticleSystem rightMuzzleParticle;
    [SerializeField] private ParticleSystem rightHitParticle;

    [SerializeField] private AudioSource laser;

    private bool rightIsActive;

    private void Start()
    {

        RightDeactivate();
        //rightTrigger.action.performed += RightActivate;
    }

    public void RightActivate()
    {
        beamRight.GetComponent<XRInteractorLineVisual>().enabled = true;
        rightIsActive = true;
        rightMuzzleParticle.Play();
        rightHitParticle.Play();
        laser.Play();
    }
    public void RightDeactivate()
    {
        beamRight.GetComponent<XRInteractorLineVisual>().enabled = false;
        rightIsActive = false;
        rightMuzzleParticle.Stop();
        rightHitParticle.Stop();
    }

    private void Update()
    {
        /*if (leftTrigger.action.WasReleasedThisFrame())
        {
            LeftDeactivate();
        }
        else if (rightTrigger.action.WasReleasedThisFrame())
        {
            RightDeactivate();
        }*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (rightIsActive)
        {
            
            Physics.Raycast(beamRight.transform.position, beamRight.transform.forward, out rightRaycastHit, 200);
            GameObject rightHitObject = rightRaycastHit.collider.gameObject;
            rightHitParticle.transform.position = rightRaycastHit.transform.position;
            if (rightHitObject)
            {
                if (rightIsActive && rightRaycastHit.collider.TryGetComponent(out Damageable damageable))
                {
                    damageable.ApplyDamage(damage * Time.fixedDeltaTime);
                }
            }
        }

        
    }
}
