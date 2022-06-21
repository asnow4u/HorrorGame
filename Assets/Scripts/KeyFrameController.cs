using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFrameController : MonoBehaviour
{

    private class KeyFrameData
    {
        public float totalDuration;
        public float curDuration;
        public float totalFrames;
        public float curFrame;

        public KeyFrameData(float totalTime, int numFrames)
        {
            totalDuration = totalTime;
            totalFrames = numFrames;
            curDuration = 0;
            curFrame = 0;
        }
    }

    private KeyFrameData data;


    public float PercentageFinished
    {
        get { return data.curDuration / data.totalDuration; }
    }



    public IEnumerator KeyFrameMovement(Transform target, List<Transform> frames, List<float> durations)
    {
        float timeElapsed;
        Vector3 startPos;
        Quaternion startRot;

        //Update
        float totalTime = 0;
        foreach (float time in durations)
        {
            totalTime += time;
        }
        data = new KeyFrameData(totalTime, frames.Count);

        //KeyFrames
        for (int i=0; i<frames.Count; i++)
        {
            timeElapsed = 0;
            startPos = target.position;
            startRot = target.rotation;

            while (timeElapsed < durations[i])
            {
                target.position = Vector3.Lerp(startPos, frames[i].position, timeElapsed / durations[i]);
                target.rotation = Quaternion.Lerp(startRot, frames[i].rotation, timeElapsed / durations[i]);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            target.position = frames[i].position;
            target.rotation = frames[i].rotation;

            data.curDuration += durations[i];
            data.curFrame = i;
        }
    }


    //Debug
    public List<Transform> testTarget = new List<Transform>();
    public List<float> testDuration = new List<float>();
    public bool testRun;

    private void Update()
    {
        if (testRun)
        {
            testRun = false;
            StartCoroutine(KeyFrameMovement(transform, testTarget, testDuration));
        }
    }
}
