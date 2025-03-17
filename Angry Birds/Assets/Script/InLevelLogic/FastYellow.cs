using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class FastYellow : MonoBehaviour
{
    // Start is called before the first frame update
    private Bird fastYellow;
    bool isClicked = false;
    bool isCollided = false;
    void Start()
    {
        fastYellow = GetComponent<Bird>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fastYellow.isMouseUp){
            if (Input.GetMouseButtonDown(0) && !isClicked && !isCollided && !EventSystem.current.IsPointerOverGameObject()) // 0 là chuột trái
                {
                    //Debug.Log("Screen clicked!");
                    gameObject.GetComponent<Rigidbody2D>().velocity *= new Vector2(3f, 10f);
                    isClicked = true;
                    // Thực hiện các thao tác khi nhấn vào màn hình
                }
        }
    }
    void OnCollisionEnter2D(Collision2D collision){
        if (GetComponent<Bird>().isMouseUp) isCollided = true;
    }
}
