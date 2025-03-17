using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    public GameObject[] birdObj;
    // Start is called before the first frame update
    private Bird bird;
    private Vector3 bird_position;
    void Start()
    {
        //birdObj = GameObject.FindGameObjectsWithTag("Bird");
        //bird = birdObj[0].GetComponent<Bird>();
        

    }

    // Update is called once per frame
    void Update()
    {
        birdObj = GameObject.FindGameObjectsWithTag("Bird");
        if (birdObj.Length > 0) bird = birdObj[0].GetComponent<Bird>();
        if (bird){
            
            
            //UnityEngine.Debug.Log("Bird: " + bird);
            LineRenderer lineRenderer = GetComponent<LineRenderer>(); // LineRenderer component
            bird_position = birdObj[0].transform.position;
            //UnityEngine.Debug.Log("Bird: " + bird_position);
            Vector3 direction = transform.position - bird_position; // Tính toán hướng bay ngược lại với hướng kéo
            float length = (float) Math.Sqrt(Math.Pow(direction.x, 2) + Math.Pow(direction.y, 2));
            if (length > bird.maxDistance) {
                direction.x = direction.x * bird.maxDistance / length; 
                direction.y = direction.y * bird.maxDistance / length;
                direction.z = 0; 
            }
            if (bird.isDragging){
                
                lineRenderer.enabled = true;
                int resolution = 30; // số điểm trên đường dẫn
            
                
                Vector3[] points = new Vector3[resolution];
                Vector3 startingPosition = transform.position; // Vị trí ban đầu của chim trước khi bắn (dùng xác định va chạm)
                Vector3 startPosition = startingPosition; // Vị trí ban đầu của chim trước khi bắn (dùng để tính vị trí các điểm)
                float timeStep = 0.1f; // khoảng cách giữa các điểm
                float mass = bird.GetComponent<Rigidbody2D>().mass;
                //UnityEngine.Debug.Log(mass);
                float holdTime = 1f;
                Vector3 initialVelocity = new Vector3((float) (direction.x * bird.launchForce / mass) * (float) holdTime, (float) (direction.y * bird.launchForce / mass) * (float) holdTime, 0); // velocity at the origin
                // UnityEngine.Debug.Log("Vận tốc đầu:" + initialVelocity);
                bool isCollision = false;
                for (int i = 0; i < resolution; i++)
                {
                    float time = timeStep * i;
                    Vector3 position = CalculatePosition(startPosition, initialVelocity, time);
                    points[i] = position;
                    //UnityEngine.Debug.Log("Start: " + startingPosition);
                    // Kiểm tra nếu có va chạm vật cản
                    if (i>0){
                        RaycastHit2D hit = Physics2D.Raycast(points[i-1], position - points[i-1], (position - points[i-1]).magnitude);
                        if (hit.collider != null && hit.collider.tag != "Map" && hit.collider.tag != "Bird" && hit.collider.tag != "Player" && hit.collider.tag != "Respawn")
                        {
                            points[i] = hit.point; // điểm va chạm
                            lineRenderer.positionCount = i+1;
                            //UnityEngine.Debug.Log("Va chạm với: "+ hit.collider.tag);
                            
                            isCollision = true;
                            break; // dừng lại khi có va chạm
                            
                        }
                    }
                    //startingPosition = position;
                }
                if (!isCollision) lineRenderer.positionCount = points.Length;
                
                // UnityEngine.Debug.Log(lineRenderer.positionCount);
                for (int i = 0; i < lineRenderer.positionCount; i++){
                    lineRenderer.SetPosition(i, points[i]);
                }
                for (int i = 0; i < lineRenderer.positionCount-1; i++){
                    //lineRenderer.SetPosition(i, points[i]);
                    UnityEngine.Debug.DrawRay(points[i], points[i+1] - points[i], Color.red);
                }
            }
            else {
                lineRenderer.enabled = false;
            }
        }
    }
    Vector3 CalculatePosition(Vector3 startPosition, Vector3 initialVelocity, float time)
    {
        float gravity = -9.81f; // gia tốc trọng trường
        //UnityEngine.Debug.Log("Điểm bắt đầu: "+ startPosition);
        Vector3 position = startPosition + initialVelocity * time + 0.5f * new Vector3(0, gravity, 0) * time * time;
        return position;
    }

}
