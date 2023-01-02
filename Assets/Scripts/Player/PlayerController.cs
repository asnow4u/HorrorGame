using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Camera mainCamera;

    public float walkSpeed;
    public float runSpeed;
    private bool allowMovement;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();

        allowMovement = true;
    }


    #region Getter / Setter

    public Camera MainCamera
    {
        get { return mainCamera; }
    }


    public void ToggleMovement(bool allowed)
    {
        allowMovement = allowed;
        GetComponent<Rigidbody>().useGravity = allowed;
        GetComponent<Collider>().enabled = allowed;
    }


    public bool InViewOfCamera(Vector3 pos)
    {
        Vector3 viewPortPos = MainCamera.WorldToViewportPoint(pos);
        if (viewPortPos.z > 0 && (new Rect(0, 0, 1, 1)).Contains(viewPortPos))
        {
            return true; 
        }

        return false;
    }

    #endregion


    private void Update()
    {
        if (!allowMovement) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 dir = transform.right * x + transform.forward * z;


        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.Translate(dir * runSpeed * Time.deltaTime, Space.World);
        }

        else
        {
            transform.Translate(dir * walkSpeed * Time.deltaTime, Space.World);
        }
    }
}
