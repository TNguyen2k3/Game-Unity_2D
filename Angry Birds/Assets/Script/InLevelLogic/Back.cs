using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
public class Back : MonoBehaviour
{
    private bool isClicked = false;
    // Start is called before the first frame update
    private bool isCollided = false;
    private bool xVelocityIsNotNegative = false; ///
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (GetComponent<Bird>().isMouseUp && Input.GetMouseButtonDown(0) && !isClicked && !isCollided && !EventSystem.current.IsPointerOverGameObject())
        {
            isClicked = true;
            if (rb.velocity.x >= 0) xVelocityIsNotNegative = true;
            else xVelocityIsNotNegative = false;
        }

        if (isClicked && !isCollided){
            float rotationStep;
           
            if (xVelocityIsNotNegative) {
                rb.velocity = new Vector2(rb.velocity.x - 40 * Time.deltaTime, rb.velocity.y); 
                rotationStep = 360 * Time.deltaTime;
            }
            else {
                rb.velocity = new Vector2(rb.velocity.x + 40 * Time.deltaTime, rb.velocity.y); 
                rotationStep = -360 * Time.deltaTime;
            }
            transform.Rotate(0, 0, rotationStep);
        }
        //Debug.Log(Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D collision){
        if (GetComponent<Bird>().isMouseUp ) {
            isCollided = true;
            //Debug.Log("Collision");
        }
    }
}
