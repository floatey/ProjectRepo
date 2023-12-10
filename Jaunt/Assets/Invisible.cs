using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{

    private bool debounce = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!debounce)
        {
            StartCoroutine(GoInvisible());
        }

    }

    IEnumerator GoInvisible()
    {
        debounce = true;
        gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 1 seconds.
        yield return new WaitForSeconds(1);

        //After we have waited 1 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;

        yield return new WaitForSeconds(1);
        debounce = false;
    }
}
