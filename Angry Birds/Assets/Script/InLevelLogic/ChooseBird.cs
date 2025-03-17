using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseBird : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextBird;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] birdAlive = GameObject.FindGameObjectsWithTag("Bird");
        if (birdAlive.Length == 0 ){
            GameObject[] candidates = GameObject.FindGameObjectsWithTag("Ready");
            if (candidates.Length == 0) return;
            nextBird = getNextBird(candidates);
        }
        // else {
        //     for (int i = 0; i < birdAlive.Length; i++) {
        //         Debug.Log("Bird " + i + " : " + birdAlive[i]);
        //     }
        // }
    }

    GameObject getNextBird(GameObject[] candidates){
            int minPriority = candidates[0].GetComponent<Bird>().priority;
            GameObject nextBird = candidates[0];
            for (int i = 0; i < candidates.Length; i++) {
                //Debug.Log("Bird" + i +"priority " + candidates[i].GetComponent<Bird>().priority);
                if (candidates[i].GetComponent<Bird>().priority < minPriority) {
                    
                    minPriority = candidates[i].GetComponent<Bird>().priority;
                    nextBird = candidates[i];
                }
            }
            return nextBird;
    }
}
