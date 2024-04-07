using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.OpenVR;

public class Hand : MonoBehaviour
{
    public Climber climber = null;
    [SerializeField] public InputActionReference leftTrigger;
    [SerializeField] public InputActionReference rightTrigger;
    //public OVRInput.Controller controller = OVRInput.Controller.None;

    public Vector3 Delta { private set; get; } = Vector3.zero;

    private Vector3 lastPosition = Vector3.zero;
    public GameObject griffesDroites;
    public GameObject griffesGauches;

    private GameObject currentPoint = null;
    public List<GameObject> contactPoints = new List<GameObject>();
    private MeshRenderer meshRenderer = null;

    [SerializeField] private AudioSource griffeOpeningSound = null;
    [SerializeField] private AudioSource griffeClosingSound = null;

    private void Awake()
    {
      meshRenderer = GetComponentInChildren<MeshRenderer>();       
    }
    private void RightActivate(InputAction.CallbackContext obj)
    {
        griffesDroites.SetActive(true);
        griffeOpeningSound.Play();
        GrabPoint();
        

        //Debug.Log("RIGHT CLICKK");
    }
    private void RightDeactivate()
    {
        griffesDroites.SetActive(false);
        griffeClosingSound.Play();
        ReleasePoint();
    }

    private void LeftActivate(InputAction.CallbackContext obj)
    {
        griffesGauches.SetActive(true);
        GrabPoint();
        griffeOpeningSound.Play();
    }

    private void LeftDeactivate()
    {
        griffesGauches.SetActive(false);
        griffeClosingSound.Play();
        ReleasePoint();
    }


    // Start is called before the first frame update
    private void Start()
    {
        if (leftTrigger != null)
            leftTrigger.action.performed += LeftActivate;
        if (rightTrigger != null)
            rightTrigger.action.performed += RightActivate;

        lastPosition = transform.position;
        griffesGauches.SetActive(false);
        griffesDroites.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (leftTrigger != null)
        {
            if (leftTrigger.action.WasReleasedThisFrame())
            {
                LeftDeactivate();
            }
        }
        if (rightTrigger != null)
        {
            if (rightTrigger.action.WasReleasedThisFrame())
            {
                RightDeactivate();
            }
        }
    }

    private void FixedUpdate()
    {
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        Delta = lastPosition - transform.position;
    }

    
    private void GrabPoint()
    {
        currentPoint = Utility.GetNearest(transform.position, contactPoints);

        if(currentPoint)
        {
            climber.SetHand(this);
            meshRenderer.enabled = false;
        }
    }

    public void ReleasePoint()
    {
        if(currentPoint)
        {
            climber.ClearHand();
            meshRenderer.enabled = true;
        }
        currentPoint = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        AddPoint(other.gameObject);
    }

    private void AddPoint(GameObject newObject)
    {
        if(newObject.CompareTag("Climbable"))
            contactPoints.Add(newObject);
    }

    private void OnTriggerExit(Collider other)
    {
        RemovePoint(other.gameObject);
    }

    private void RemovePoint(GameObject newObject)
    {
        if (newObject.CompareTag("Climbable"))
            contactPoints.Remove(newObject);
    }
}
