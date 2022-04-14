using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    //Skeleton
    [SerializeField] private GameObject skeletonPrefab;
    private GameObject skeleton;

    //Animator
    private Animator animator;

    //Particle Systems
    private ParticleSystem hauntParticle;

    //Lights
    private Light headLight;

    //Debug
    public bool haunt;
    public bool walk;


    // Start is called before the first frame update
    void Start()
    {
        skeleton = Instantiate(skeletonPrefab, transform);
        hauntParticle = GetComponentInChildren<ParticleSystem>();
        headLight = GetComponentInChildren<Light>();
        skeleton.TryGetComponent<Animator>(out animator);
    }

    // Update is called once per frame
    void Update()
    {
        if (haunt)
        {
            haunt = false;
            transform.position = new Vector3(0, 0, 0);
            StartCoroutine(Haunt(transform));
        }

        if (walk)
        {
            transform.Translate(new Vector3(0, 0, 0.01f));
        }
    }



    private IEnumerator Haunt(Transform spawnTransform) 
    {
        SkinnedMeshRenderer mesh = skeleton.GetComponentInChildren<SkinnedMeshRenderer>();
        mesh.enabled = false;
        
        skeleton.transform.position = spawnTransform.position;
        skeleton.transform.rotation = spawnTransform.rotation;

        var shape = hauntParticle.shape;
        shape.skinnedMeshRenderer = mesh;
        hauntParticle.Play();

        //Time before skeleton actally appears
        yield return new WaitForSeconds(2f);

        mesh.enabled = true;

        animator.SetTrigger("resurection");

        //TODO: Wait till after resurrection is finished
        yield return new WaitForSeconds(5f);
        

        hauntParticle.Stop();
        hauntParticle.Clear();

        //TODO: better walking
        walk = true;
        animator.SetBool("walk_forward", true);
    }
}
