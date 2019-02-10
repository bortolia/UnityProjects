using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Set in Inspector")]

    //Prefab for instantiating apples
    public GameObject applePrefab;

    //Speed at which the AppleTree moves
    public float speed = 1f;

    //Distance where AppleTree turns around
    public float leftAndRightEdge = 10f;

    //Chance that AppleTree will change direction
    public float chanceToChangeDirections = 0.1f;

    // Rate at which apples will be instantiated
    public float secondsBetweenAppleDrops = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //Dropping apples every second
        Invoke("DropApple", 2f);
    }

    void DropApple()
    {
        GameObject apple = Instantiate<GameObject>(applePrefab);
        apple.transform.position = transform.position;
        Invoke("DropApple", secondsBetweenAppleDrops);
    }

    // Update is called once per frame
    void Update()
    {
        // Basic Movement
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        // Changing direction
        if(pos.x < -leftAndRightEdge) {
            speed = Mathf.Abs(speed);   // Moves right
        }
        else if(pos.x > leftAndRightEdge) {
            speed = -Mathf.Abs(speed);  // Moves left
        }
    }

    void FixedUpdate()
    {
        // Changing directions randomly is now time-based because of FixedUpdate()
        if (Random.value < chanceToChangeDirections) {
            speed *= -1;    //Changes direction
        }
    }
}
