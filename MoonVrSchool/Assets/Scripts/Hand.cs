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

    private float griffesSpeed = 5f;
    private Vector3 positionFinaleGriffeLeft = Vector3.zero;
    private Vector3 positionFinaleGriffeRight = Vector3.zero;

    private bool griffesAnimation = false;
    private bool griffesDroitesActivate = false;
    private bool griffesGauchesActivate = false;

    private GameObject currentPoint = null;
    public List<GameObject> contactPoints = new List<GameObject>();
    private MeshRenderer meshRenderer = null;

    [SerializeField] private AudioSource griffeSoundCue = null;

    private void Awake()
    {
      meshRenderer = GetComponentInChildren<MeshRenderer>();       
    }
    private void RightActivate(InputAction.CallbackContext obj)
    {
        griffesAnimation = true;
        griffesDroitesActivate = true;
        griffeSoundCue.Play();

        if (!griffesDroites) griffesDroitesPosition = griffesDroites.transform.position;
        positionFinaleGriffeRight = griffesDroitesPosition + new Vector3(0f, 0f, 10.15f);
        GrabPoint();
        

        //Debug.Log("RIGHT CLICKK");
    }
    private void RightDeactivate()
    {
        griffesAnimation = true;
        griffesDroitesActivate = false;

        if (!griffesDroites) griffesDroitesPosition = griffesDroites.transform.position;
        positionFinaleGriffeRight = griffesDroitesPosition + new Vector3(0f, 0f, -10.15f);
        ReleasePoint();
    }

    private void LeftActivate(InputAction.CallbackContext obj)
    {
        griffesAnimation = true;
        GrabPoint();
        griffeSoundCue.Play();

        griffesGauchesActivate = true;
        if(!griffesGauches) griffesDroitesPosition = griffesDroites.transform.position;

        positionFinaleGriffeLeft = griffesGauchesPosition + new Vector3(0f, 0f, 10.15f);
        //Debug.Log("LEFT CLICKK");
    }

    private void LeftDeactivate()
    {
        griffesAnimation = true;
        griffesGauchesActivate = false;
        if (!griffesGauches) griffesDroitesPosition = griffesDroites.transform.position;
        positionFinaleGriffeLeft = griffesGauchesPosition + new Vector3(0f, 0f, -10.15f);
        ReleasePoint();
    }


    // Start is called before the first frame update
    private void Start()
    {
        //LeftDeactivate();
        //RightDeactivate();

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
        if(griffesDroites != null)
        {
            if (griffesDroitesActivate == true && griffesAnimation == true)
            {
                Debug.Log("griffe right on");

                griffesDroitesPosition = Vector3.MoveTowards(griffesDroitesPosition, positionFinaleGriffeRight, Time.deltaTime * griffesSpeed);
                if (griffesDroitesPosition == positionFinaleGriffeRight)
                    griffesAnimation = false;
            }

            if (griffesDroitesActivate == false && griffesAnimation == true)
            {
                Debug.Log("griffe right off");

                griffesDroitesPosition = Vector3.MoveTowards(griffesDroitesPosition, positionFinaleGriffeRight, Time.deltaTime * griffesSpeed);
                if (griffesDroitesPosition == positionFinaleGriffeRight)
                    griffesAnimation = false;
            }
        }
 
        if(griffesGauches)
        {
            if (griffesGauchesActivate == true && griffesAnimation == true)
            {
                Debug.Log("griffe left on");

                griffesGauchesPosition = Vector3.MoveTowards(griffesGauchesPosition, positionFinaleGriffeLeft, Time.deltaTime * griffesSpeed);
                if (griffesGauchesPosition == positionFinaleGriffeLeft)
                    griffesAnimation = false;
            }

            if (griffesGauchesActivate == false && griffesAnimation == true)
            {
                Debug.Log("griffe left off");

                griffesGauchesPosition = Vector3.MoveTowards(griffesGauchesPosition, positionFinaleGriffeLeft, Time.deltaTime * griffesSpeed);
                if (griffesGauchesPosition == positionFinaleGriffeLeft)
                    griffesAnimation = false;
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
