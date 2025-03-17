using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBird : MonoBehaviour
{
    
    private bool isCoroutineRunning = false;
    public GameObject normalBird;
    public GameObject initPos;
    private Rigidbody2D bird;
    private bool isReset = false;
    //private bool isCollided = false;

    public Vector2 velocityBeforeCollision;
    // Start is called before the first frame update
    void Start()
    {
        //bird = normalBird.GetComponent<Rigidbody2D>();
        //bird.mass = 3;
        
        transform.position = initPos.transform.position;
    }
    
    private void FixedUpdate()
    {
        // Cập nhật vận tốc ngay trước va chạm
        velocityBeforeCollision = GetComponent<Rigidbody2D>().velocity;
    }
    // Update is called once per frame
    void Update()
    {
        GameObject nextBird = GameObject.FindGameObjectsWithTag("BirdController")[0].GetComponent<ChooseBird>().nextBird;
        if (gameObject == nextBird && !isReset) {
            
            //Debug.Log("Bird : NULL!");
            StartCoroutine(GoToInitialPosition());
            isReset = true;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if (GetComponent<Bird>().isMouseUp ) {
            //isCollided = true;
            StartCoroutine(Disappear());
            if (collision.gameObject.GetComponent<EnemyHealth>()){
                float velocityAfterCollision = collision.gameObject.GetComponent<EnemyHealth>().velocityAfterCollision;
                Debug.Log("Velocity rate after collision: " + velocityAfterCollision);
                if (velocityAfterCollision > 0){
                    GetComponent<Rigidbody2D>().velocity = velocityBeforeCollision * new Vector2(velocityAfterCollision, velocityAfterCollision);
                }
            }
            //Debug.Log("Collision");
        }
        
    }
    IEnumerator GoToInitialPosition()
    {   
        
        yield return new WaitForSeconds(1);
        
        Bird bird = GetComponent<Bird>();
        
        //Debug.Log("Bird " + bird + "is the next bird");
        normalBird.tag = "Bird";
        bird.ResetBird();
        
            
        //Debug.Log(bird.initialPosition.ToString());
    }

    IEnumerator Disappear(){
        yield return new WaitForSeconds(3);
        GetComponent<Bird>().Disappear();
        //Debug.Log("Disappear");
    }
}
