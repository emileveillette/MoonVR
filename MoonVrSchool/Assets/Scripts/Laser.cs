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
    [SerializeField] private GameObject beamLeft;
    GameObject leftHit;
    GameObject rightHit;
    RaycastHit leftRaycastHit;
    RaycastHit rightRaycastHit;
    [SerializeField] private float damage;
    [SerializeField] private InputActionReference leftTrigger;
    [SerializeField] private InputActionReference rightTrigger;

    [SerializeField] private GameObject explosion;

    private bool leftIsActive;
    private bool rightIsActive;

    private void Start()
    {

        LeftDeactivate();
        RightDeactivate();

        leftTrigger.action.performed += LeftActivate;
        rightTrigger.action.performed += RightActivate;
    }

    private void RightActivate(InputAction.CallbackContext obj)
    {
        beamRight.GetComponent<XRInteractorLineVisual>().enabled = true;
        rightIsActive = true;
    }

    private void LeftActivate(InputAction.CallbackContext obj)
    {
        beamLeft.GetComponent<XRInteractorLineVisual>().enabled = true;
        leftIsActive = true;
    }

    private void LeftDeactivate()
    {
        beamLeft.GetComponent<XRInteractorLineVisual>().enabled = false;
        leftIsActive = false;
    }
    private void RightDeactivate()
    {
        beamRight.GetComponent<XRInteractorLineVisual>().enabled = false;
        rightIsActive = false;
    }

    private void Update()
    {
        if (leftTrigger.action.WasReleasedThisFrame())
        {
            LeftDeactivate();
        }
        else if (rightTrigger.action.WasReleasedThisFrame())
        {
            RightDeactivate();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Damage
        if (leftIsActive)
        {
            Physics.Raycast(beamLeft.transform.position, beamLeft.transform.forward, out leftRaycastHit, 200);
            GameObject leftHitObject = leftRaycastHit.collider.gameObject;
            if (leftHitObject)
            {
                if (leftIsActive && leftRaycastHit.collider.TryGetComponent(out Damageable damageable))
                {
                    damageable.ApplyDamage(damage * Time.fixedDeltaTime);
                }
            }
        }

        if (rightIsActive)
        {
            Physics.Raycast(beamRight.transform.position, beamRight.transform.forward, out rightRaycastHit, 200);
            GameObject rightHitObject = rightRaycastHit.collider.gameObject;
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
