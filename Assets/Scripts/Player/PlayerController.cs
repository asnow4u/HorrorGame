using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Camera mainCamera;

    public float walkSpeed;
    public float runSpeed;

    public TextMeshProUGUI promptText;

    private bool allowMovement;

    // Start is called before the first frame update
    private void Awake()
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

        ObjectRayCast();
    }

    private void ObjectRayCast()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            //Debug.Log(hit.transform.name);
            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.blue);

            DoorController[] controllers = hit.transform.GetComponentsInChildren<DoorController>();

            //Debug.Log("You have this many door controllers: " + controllers.Length);


            if (controllers.Length > 0) {
                promptText.SetText("Press 'E' to use.");
                promptText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    for (int i = 0; i < controllers.Length; i++)
                    {
                        controllers[i].Force(50, transform.position);
                    }
                }
            }
            else
            {
                promptText.gameObject.SetActive(false);
            }
        }
    }


    /// <summary>
    /// Enable or disable movement
    /// </summary>
    /// <param name="allowed"></param>
    public void ToggleMovement(bool allowed)
    {
        allowMovement = allowed;
        GetComponent<Rigidbody>().useGravity = allowed;
        GetComponent<Collider>().enabled = allowed;
    }


    /// <summary>
    /// Returns whether the givin position is within line of sight
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool InViewOfCamera(Vector3 pos)
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(pos);
        if (viewPortPos.z > 0 && (new Rect(0, 0, 1, 1)).Contains(viewPortPos))
        {
            return true; 
        }

        return false;
    }


    /// <summary>
    /// Called when the skeleton has captured the player
    /// </summary>
    public void PlayerCaught()
    {
        Debug.Log("YOU DIED!");
        SceneManager.LoadScene("Main");
    }
    




    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Skeleman")
        {
            PlayerCaught();
        }
    }
}
