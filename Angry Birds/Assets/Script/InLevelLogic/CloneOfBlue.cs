using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneOfBlue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position);
        if (transform.position.y <= -10) {
            //Debug.Log("Out of map bounds");
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Bird") {
            //Debug.Log("Coll");
            StartCoroutine(disappear(gameObject));
        }
    }
    IEnumerator disappear(GameObject gameObject){
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        // Add your code to handle the disappearance of the clone here
    }
}
