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
    public Vector3 defaultPosition;

    public bool isGrabed = false;
    bool hasBeenGrounded = true;
    bool isEnding = false;

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
            if (isEnding)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new(0f, 0f, 0f)), 10f * Time.deltaTime);
            }
            else 
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new(0f, 270f, 0f)), 10f * Time.deltaTime);
            }
           
        }
    }

    public void Take(Transform _holder, bool _ending = false)
    {
        if (isGrabed) return;

        ballCollider.enabled = false;
        ballTrigger.enabled = false;

        rb.isKinematic = true;

        if (!_ending) gameObject.layer = LayerMask.NameToLayer(grabedSortingLayer);
        if (!_ending) mesh.layer = gameObject.layer;
        if (!_ending) SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxGrab);
        
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        holdTransform = _holder;
        transform.SetParent(holdTransform, true);
        transform.localPosition = Vector3.zero;
        isGrabed = true;
        isEnding = _ending;
    }

    public void Trow(Vector3 _velocity)
    {
        SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxTrow);

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
        Respawn();
    }

    public void Respawn()
    {
        transform.position = defaultPosition;
        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!hasBeenGrounded)
        {
            rebond++;
            Meter.Instance.AddNewMeterText("Rebond x"+ rebond.ToString(), 5 * rebond);
            SFXManager.Instance.SfxPlay(SFXManager.Instance.sfxBong);
        }
    }
}
