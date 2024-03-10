using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.OpenVR;

public class Hand : MonoBehaviour
{
    public Climber climber = null;
    public OVRInput.Controller controller = OVRInput.Controller.None;

    public Vector3 Delta { private set; get; } = Vector3.zero;

    private Vector3 lastPosition = Vector3.zero;

    private GameObject currentPoint = null;
    public List<GameObject> contactPoints = new List<GameObject>();
    private MeshRenderer meshRenderer = null;

    private void Awake()
    {
      meshRenderer = GetComponentInChildren<MeshRenderer>();       
    }
    // Start is called before the first frame update
    private void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller))
            GrabPoint();
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, controller))
            ReleasePoint();
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
