using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform orientation = null;
    [SerializeField] private Transform lookAt = null;


    [SerializeField] private float sensibilityX = 1;
    [SerializeField] private float sensibilityY = 1;

    Vector3 rotation = Vector3.zero;

   

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        

        float _mouseX = Input.GetAxisRaw("Mouse X") * sensibilityX * Time.deltaTime;
        float _mouseY = Input.GetAxisRaw("Mouse Y") * sensibilityY * Time.deltaTime;

        rotation.y += _mouseX;
        rotation.x -= _mouseY;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
        orientation.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        lookAt.transform.rotation = transform.rotation;

        transform.position = orientation.position;
    }
}
