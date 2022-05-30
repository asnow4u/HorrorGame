using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Transform player;

    public static PlayerController instance;

    private Camera mainCamera;



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

        mainCamera = player.GetComponentInChildren<Camera>();
    }


    #region Getter / Setter

    public Camera MainCamera
    {
        get { return mainCamera; }
    }


    #endregion


    public bool InViewOfCamera(Vector3 pos)
    {
        Vector3 viewPortPos = MainCamera.WorldToViewportPoint(pos);
        if (viewPortPos.z > 0 && (new Rect(0, 0, 1, 1)).Contains(viewPortPos))
        {
            return true; 
        }

        return false;
    }

}
