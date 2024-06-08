using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Transform orientation = null;
    [SerializeField] private Transform lookAt = null;

    [SerializeField] private float sensibilityX = 1;
    [SerializeField] private float sensibilityY = 1;

    private float shakeMagnitude = 0f;
    private float shakeTime = 0f;
    private float shakeTimeMax = .1f;
    private float shakeLoss = 1f;
    private float skakeValue = 0f;
    private float skakeYMult = 0f;


    Vector3 rotation = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        float _mouseX = Input.GetAxisRaw("Mouse X") * sensibilityX * Time.unscaledDeltaTime * (Time.deltaTime <= 0 ? 0f : 1f);
        float _mouseY = Input.GetAxisRaw("Mouse Y") * sensibilityY * Time.unscaledDeltaTime * (Time.deltaTime <= 0 ? 0f : 1f);

        rotation.y += _mouseX;
        rotation.x -= _mouseY;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
  
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
        orientation.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        lookAt.transform.rotation = transform.rotation;

        transform.position = orientation.position;
        ShakeUpdate();
    }
   
    private void ShakeUpdate()
    {
        shakeMagnitude = Mathf.Lerp(shakeMagnitude, 0f, shakeLoss * Time.unscaledDeltaTime);

        shakeTime += Time.unscaledDeltaTime;
        if (shakeTime >= shakeTimeMax)
        {
            skakeValue = Random.Range(-1f, 1f);
            skakeYMult = Random.Range(0f, 1f)>0.5f ? -1f : 1f ;
            shakeTime = 0f;
        }

        transform.eulerAngles += new Vector3(shakeMagnitude * (1 - skakeValue), shakeMagnitude * skakeValue * skakeYMult, 0f);
    }

    public void Shake(float _magnitude = 0.1f, float _loss = 5f, float _time = 0.1f)
    {
        shakeMagnitude = _magnitude;
        shakeTimeMax = _time;
        shakeLoss = _loss;
    }

    public void SetSensibility(float value)
    {
        sensibilityX = value;
        sensibilityY = value;
    }
}
