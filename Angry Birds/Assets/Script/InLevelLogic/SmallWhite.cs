using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallWhite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -10) {
            
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with " + collision.gameObject.tag);  // For debugging purposes, you can print the tag of the colliding object here
        if (collision.gameObject.tag != "Untagged") StartCoroutine(disappear());
    }
    IEnumerator disappear(){
        yield return new WaitForSeconds(3);
        Debug.Log("Disappear with " + gameObject.tag);
        Destroy(gameObject);
       
    }
}
