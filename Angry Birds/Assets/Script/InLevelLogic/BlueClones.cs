using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BlueClones : MonoBehaviour
{
    // Start is called before the first frame update
    Bird bird;
    public GameObject blueClones;
    GameObject[] blueClone = new GameObject[2];
    bool isClicked = false;
    bool isCollided = false;
    void Start()
    {
        bird = GetComponent<Bird>();
    }

    // Update is called once per frame
    void Update()
    {   //if (!gameObject) return;
        
        if (bird.isMouseUp && Input.GetMouseButtonDown(0) && !isClicked && !isCollided && !EventSystem.current.IsPointerOverGameObject()){
            
            isClicked = true;
            float velocity = -0.3f;
            float y = 0.3f;           
            
            if (blueClones != null){
                for (int i = 0; i < 2; i++)
                {
                    blueClone[i] = Instantiate(blueClones, transform.position + new Vector3(0, y * (2 * i - 1), 0), Quaternion.identity);
                    blueClone[i].GetComponent<SpriteRenderer>().enabled = true;  
                    blueClone[i].GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity + new Vector2(0, velocity - 2 * i * velocity);
                    blueClone[i].transform.localScale = new Vector3(0.5969f, 0.5969f, 0.5969f);
                    blueClone[i].GetComponent<CircleCollider2D>().enabled = true;
                    blueClone[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    // Debug.Log(blueClone[i].transform.position);
                }
            }
            //else {Debug.Log("blueClone sprite is null");}
            //Destroy(gameObject);
             // Xóa chim 
        }
        // for (int i = 0; i < blueClone.Length; i++){
        //     if (blueClone[i]){
        //         if (blueClone[i].transform.position.y <= 10) Destroy(blueClone[i]);
        //     }
        // }
        if (transform.position.y <= -10) Destroy(gameObject);
    }
    
    // IEnumerator disappear(GameObject[] blueClone){
    //     yield return new WaitForSeconds(3);
    //     for (int i = 0; i < blueClone.Length; i++)
    //     {
    //         Destroy(blueClone[i]);
    //     }
    //     Destroy(gameObject);
    //     isClicked = false;
    // }
    IEnumerator disappear(GameObject gameObject){
        yield return new WaitForSeconds(3);
        
        Destroy(gameObject);
        
        isClicked = false;
    }
    void OnCollisionEnter2D(){
        if (GetComponent<Bird>().isMouseUp) isCollided = true;
    }
}
