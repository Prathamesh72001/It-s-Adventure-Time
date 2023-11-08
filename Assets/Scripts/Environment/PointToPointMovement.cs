using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPointMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float startingPos = 0f;
    [SerializeField] float endingPos = 1f;
    [SerializeField] float scale = 1f;
    [SerializeField] bool directionX = true;
    [SerializeField] bool isDestroyable = false;
    public bool shouldMove = true;
    Rigidbody2D rigidbody2D;
    BoxCollider2D boxCollider2D;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(shouldMove){
        Move();
        

        if (directionX)
        {

            if (isFacingRight())
                rigidbody2D.velocity = new Vector2(moveSpeed, 0f);
            else
                rigidbody2D.velocity = new Vector2(-moveSpeed, 0f);
        }
        else
        {
            if (isFacingUp())
                rigidbody2D.velocity = new Vector2(0f, -moveSpeed);
            else
                rigidbody2D.velocity = new Vector2(0f, moveSpeed);
        }
        }

    }

    private bool isFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    private bool isFacingUp()
    {
        return transform.localScale.y > Mathf.Epsilon;

    }

    private void Move()
    {
        if (directionX)
        {
            if (transform.localPosition.x < startingPos){
                if(isDestroyable){
Destroy(gameObject, 0.1f);
                }
                transform.localScale = new Vector2(scale, transform.localScale.y);
                
            }
            else if(transform.localPosition.x > endingPos){
                
                transform.localScale = new Vector2(-scale, transform.localScale.y);
                
            }

        }
        else
        {
            if (transform.localPosition.y > startingPos){
                if(isDestroyable){
Destroy(gameObject, 0.1f);
                }
                transform.localScale = new Vector2(scale,scale);
                
            }
            else if ( transform.localPosition.y < endingPos){
                
                transform.localScale = new Vector2(-scale, -scale);
                
            }

        }

    }

    // private void OnTrtiggerEnter2D(Collider2D collision)
    // {
    //     transform.localScale = new Vector2(-(Mathf.Sign(rigidbody2D.velocity.x)), transform.localScale.y);
    // }
}
