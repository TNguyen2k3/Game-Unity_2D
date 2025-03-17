using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pink : MonoBehaviour
{
    // Start is called before the first frame update
    bool isActive = false;
    public float radius = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Bird>().isMouseUp && !isActive){
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()){
                isActive = true;
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, radius);
                foreach (Collider2D collider in collider2Ds){
                    if (collider.gameObject) {
                        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                        rb.gravityScale = -0.3f;
                    }  
                }
            }
        }
        if (isActive){
            if (gameObject){
                StartCoroutine(disappearAfterEffect(gameObject));
            }
        }
    }
    IEnumerator disappearAfterEffect(GameObject pink){
        yield return new WaitForSeconds(2f);
        Destroy(pink);
    }
    void OnCollisionEnter2D(Collision2D collision){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders){
            if (collider.gameObject.tag != "Ready" && GetComponent<Bird>().isMouseUp){
                if (collider.gameObject) {
                    Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                    StartCoroutine(waitEffect(rb));
                }  
            }
        }
    }
    IEnumerator waitEffect(Rigidbody2D rb){
        yield return new WaitForSeconds(2f);
        if (rb) rb.gravityScale = -0.5f;
    }
    
}
