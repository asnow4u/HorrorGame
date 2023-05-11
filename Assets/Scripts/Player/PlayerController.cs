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

    public Items heldItem;

    private bool allowMovement;

    [Header("Hiding Spot")]
    public HidingSpot curHidingSpot;
    public bool hidingAnimationPlaying; //TODO: get from actual animation controller


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
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f, ~LayerMask.NameToLayer("Interactable")))
        {
            Debug.Log(hit.transform.name);

            switch (hit.transform.tag)
            {
                case "Door":
                    InteractableDoorPrompt(hit.transform);                   
                    break;
                
                case "Item":
                    InteractableItemPrompt(hit.transform);
                    break;

                case "EscapeDoor":
                    InteractableEscapeDoorPrompt(hit.transform);
                    break;
            }
        }

        else
        {
            promptText.gameObject.SetActive(false);
        }
    }


    private void InteractableDoorPrompt(Transform hitTransform)
    {
        DoorController[] controllers = hitTransform.GetComponentsInParent<DoorController>();
        Debug.Log("You have this many door controllers: " + controllers.Length);

        if (controllers.Length > 0)
        {
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

    private void InteractableItemPrompt(Transform hitTransform)
    {
        promptText.SetText("Press 'E' to pick up.");
        promptText.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem != null)
            {
                Vector3 dropItemPos = new Vector3(transform.position.x, heldItem.itemHeight, transform.position.z);
                
                heldItem.transform.position = dropItemPos;
                Debug.Log("item height is:" + heldItem.itemHeight);
                
                heldItem.ShowItem();

                heldItem = hitTransform.GetComponent<Items>();

                heldItem.HideItem();
            }
            else
            {
                heldItem = hitTransform.GetComponent<Items>();

                heldItem.HideItem();
            }

            //promptText.gameObject.SetActive(false);
        }
    }

    private void InteractableEscapeDoorPrompt(Transform hitTransform)
    {
        EscapeDoorController[] controllers = hitTransform.GetComponentsInParent<EscapeDoorController>();
        Debug.Log("This is the escape door.");

        if (controllers.Length > 0)
        {
            
            
            if (heldItem != null && heldItem.TryGetComponent<Key>(out Key key))
            {
                promptText.SetText("Press 'E' to use.");
                promptText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    for (int i = 0; i < controllers.Length; i++)
                    {
                        controllers[i].IsTheOne(key);
                        heldItem.DestroyItem();
                        //controllers[i].Force(50, transform.position);
                    }
                }
            }
            else
            {
                //TODO: Determine how we want to prompt player that they need a key not some other item to interact with the door.
                promptText.SetText("Press 'E' to use.");
                promptText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    
                }
            }
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }

    public void ToggleMovement(bool allowed)
    {
        allowMovement = allowed;
        GetComponent<Rigidbody>().useGravity = allowed;
        GetComponent<Collider>().enabled = allowed;
    }


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
