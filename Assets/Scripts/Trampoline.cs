using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float bounce = 30f;
    Animator anim;

   
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("player"))
        {
            anim.SetBool("is_bounce", true);
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
        }
    }
    
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("player"))
        {
            anim.SetBool("is_bounce", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
