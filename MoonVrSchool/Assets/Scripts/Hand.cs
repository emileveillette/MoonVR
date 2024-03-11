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
    private Vector3 griffesDroitesPosition = Vector3.zero;
    private Vector3 griffesGauchesPosition = Vector3.zero;
    private bool griffesDroitesActivate = false;
    private bool griffesGauchesActivate = false;
    private float griffesSpeed = 0.2f;
    private Vector3 positionFinaleGriffe = Vector3.zero;

    private bool griffesAnimation = false;
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
        LeftDeactivate();
        RightDeactivate();

        if (leftTrigger != null)
            leftTrigger.action.performed += LeftActivate;
        if (rightTrigger != null)
            rightTrigger.action.performed += RightActivate;

        lastPosition = transform.position;
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
        if (griffesDroitesActivate == true && griffesAnimation == true)
        {
            griffesDroitesPosition = griffesDroites.transform.position;
            positionFinaleGriffe += new Vector3(0f, 0f, -0.15f);
            griffesDroitesPosition = Vector3.Lerp(griffesDroitesPosition, positionFinaleGriffe, Time.deltaTime * griffesSpeed);
            if (griffesDroitesPosition == positionFinaleGriffe)
                griffesAnimation = false;
        }
        if (griffesGauchesActivate == true && griffesAnimation == true)
        {
            griffesGauchesPosition = griffesGauches.transform.position;
            positionFinaleGriffe += new Vector3(0f, 0f, -0.15f);
            griffesGauchesPosition = Vector3.Lerp(griffesGauchesPosition, positionFinaleGriffe, Time.deltaTime * griffesSpeed);
            if (griffesGauchesPosition == positionFinaleGriffe)
                griffesAnimation = false;
        }

        if (griffesDroitesActivate == false && griffesAnimation == true)
        {
            griffesDroitesPosition = griffesDroites.transform.position;
            positionFinaleGriffe += new Vector3(0f, 0f, 0.15f);
            griffesDroitesPosition = Vector3.Lerp(griffesDroitesPosition, positionFinaleGriffe, Time.deltaTime * griffesSpeed);
            if (griffesDroitesPosition == positionFinaleGriffe)
                griffesAnimation = false;
        }
        if (griffesGauchesActivate == false && griffesAnimation == true)
        {
            griffesGauchesPosition = griffesGauches.transform.position;
            positionFinaleGriffe += new Vector3(0f, 0f, 0.15f);
            griffesGauchesPosition = Vector3.Lerp(griffesGauchesPosition, positionFinaleGriffe, Time.deltaTime * griffesSpeed);
            if (griffesGauchesPosition == positionFinaleGriffe)
                griffesAnimation = false;
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

    private void RightActivate(InputAction.CallbackContext obj)
    {
        griffesAnimation = true;
        GrabPoint();
        griffesDroitesActivate = true;
        
        //Debug.Log("RIGHT CLICKK");
    }

    private void LeftActivate(InputAction.CallbackContext obj)
    {
        griffesAnimation = true;
        GrabPoint();
        griffesGauchesActivate = true;
        //Debug.Log("LEFT CLICKK");
    }

    private void LeftDeactivate()
    {
        griffesAnimation = true;
        griffesGauchesActivate = false;
        ReleasePoint();
    }
    private void RightDeactivate()
    {
        griffesAnimation = true;
        griffesGauchesActivate = false;
        ReleasePoint();
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
