using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public float speed, pos1, pos2;
    float dirX,dirY;
    public bool isDirX = true;
    bool movingRight=true, movingUp=true;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isDirX) {
            if (transform.position.x > pos2)
                movingRight = false;
            else if (transform.position.x < pos1)
                movingRight = true;

            if (movingRight)
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            else
                transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        }
        else
        {
            if (transform.position.y < pos2)
                movingUp = true;
            else if (transform.position.y > pos1)
                movingUp = false;

            if (movingUp)
                transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
            else
                transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
        }

       


    }
}
