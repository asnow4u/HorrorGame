using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{

    /* Requirements
     * 
     * Primarly incharge of starting the game and controlling enviroment
     * 
     * Also contains ref to other scripts in the skeleton
     */

    public static SkeletonController instance;
    public static SkeletonMovement movement;
    public static SkeletonBehavior behaviour;

    //Skeleton
    [SerializeField] private GameObject skeletonPrefab;
    private GameObject skeleton;

    //Animator
    private Animator animator;

    //Particle Systems
    private ParticleSystem hauntParticle;

    //Lights
    private Light headLight;


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

    // Start is called before the first frame update
    void Start()
    {
        //    skeleton = Instantiate(skeletonPrefab, transform);
        //    hauntParticle = GetComponentInChildren<ParticleSystem>();
        //    headLight = GetComponentInChildren<Light>();
        //    skeleton.TryGetComponent<Animator>(out animator);
        //    skeleton.TryGetComponent<SkeletonBehavior>(out behaviour);
        //    skeleton.TryGetComponent<SkeletonMovement>(out movement);
    }


// Update is called once per frame
void Update()
    {

    }

}
