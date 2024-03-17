using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public Transform pointA, pointB;

    public float speed;

    public bool directionFlag = true;

    void Start()
    {
        //transform.position = pointA.position;

        speed = Random.value;
    }

    void Update()
    {
        
        // Calcula la posición actual interpolando suavemente entre pointA y pointB
        float pingPong = Mathf.PingPong(Time.time * speed/2, 1f);
        
        transform.position = Vector3.Lerp(pointA.position, pointB.position, pingPong);


    }
}
