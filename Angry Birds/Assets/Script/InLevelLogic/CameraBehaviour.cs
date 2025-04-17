using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    
    public GameObject[] bird;
    public GameObject start;
    public float returnSpeed = 0.1f;
    // Start is called before the first frame update
    void Awake(){
        Time.timeScale = 1;
    }
    void Start()
    {
        transform.position = start.transform.position + new Vector3(0, 0, -0.6f);
    }

    // Update is called once per frame
    void Update()
    {   
        bird = GameObject.FindGameObjectsWithTag("Bird");
        if (bird.Length != 0) {
            if (bird[0].GetComponent<Bird>().isMouseUp){
                transform.position = bird[0].transform.position + new Vector3(0, 0, -1);
                // Debug.Log(transform.position);
            }
        }
        else OnBirdDestroyed();
    }
    public void OnBirdDestroyed()
    {
        // Khi con chim biến mất, bắt đầu Coroutine để chờ returnDelay giây rồi đưa camera về vị trí ban đầu
        StartCoroutine(ReturnToInitialPosition());
    }

    IEnumerator ReturnToInitialPosition()
    {
        // Đợi returnDelay giây
        //yield return new WaitForSeconds(3);
        while (Vector3.Distance(transform.position, start.transform.position + new Vector3(0, 0, -1)) > 0.01f)
        {
            // Di chuyển mượt mà về vị trí của ná
            transform.position = Vector3.Lerp(transform.position, start.transform.position + new Vector3(0, 0, -1), returnSpeed * Time.deltaTime);
            //Debug.Log(transform.position);
            // Đợi tới frame tiếp theo
            yield return null;
        }
    }
}
