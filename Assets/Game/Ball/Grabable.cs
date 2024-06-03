using UnityEngine;

public class Grabable : MonoBehaviour
{
    [SerializeField] private string defaultSortingLayer = "Default";
    [SerializeField] private string grabedSortingLayer = "Weapon";
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject mesh;
    [SerializeField] private LayerMask whatIsGround;

    private Transform holdTransform = null;
    [HideInInspector] public int rebond = 0;
    [HideInInspector] public Rigidbody rb = null;
    [SerializeField] private Collider ballCollider = null;
    [SerializeField] private Collider ballTrigger = null;
    private MeshRenderer meshRenderer = null;
    private Transform defaultParent = null;
    private Vector3 defaultPosition;

    bool isGrabed = false;
    bool hasBeenGrounded = true;
   
    void Start()
    {
        defaultPosition = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        defaultParent = transform.parent;
    }

    public void Update()
    {
        hasBeenGrounded = (hasBeenGrounded || (rb.velocity.y <= 0 && Physics.SphereCast(transform.position, 0.45f, Vector3.down, out RaycastHit _rayCast, 0.1f, whatIsGround)));
        animator.SetBool("Balled", !isGrabed);
       
        if (isGrabed)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new(0f, 270f, 0f)),10f * Time.deltaTime);
        }
    }

    public void Take(Transform _holder)
    {
        ballCollider.enabled = false;
        ballTrigger.enabled = false;

        rb.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer(grabedSortingLayer);
        mesh.layer = gameObject.layer;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        holdTransform = _holder;
        transform.SetParent(holdTransform, true);
        transform.localPosition = Vector3.zero;
        isGrabed = true;
    }

    public void Trow(Vector3 _velocity)
    {
        rebond = 0;
        hasBeenGrounded = false;
        ballCollider.enabled = true;
        ballTrigger.enabled = true;

        rb.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer(defaultSortingLayer);
        mesh.layer = gameObject.layer;

        transform.SetParent(defaultParent);
        rb.AddForce(_velocity,ForceMode.VelocityChange);
        isGrabed = false;
    }

    public void Death()
    {
        transform.position = defaultPosition;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!hasBeenGrounded)
        {
            rebond++;
            Meter.Instance.AddNewMeterText("Rebond x"+ rebond.ToString(), 5 * rebond);
        }
    }
}
