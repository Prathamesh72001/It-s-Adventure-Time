using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    void Start()
    {
        anim=GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
        Destroy(gameObject,2f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag.Equals("enemies")){
            
                    
                    if(col.gameObject.name=="Boss"){
                        Boss.instance.Hurt(10f);
                    }else{
                       Destroy(col.gameObject);
                    }

                    StartCoroutine(DestroyBullet());
          
        }
    }

    IEnumerator DestroyBullet(){
        rb.velocity=Vector3.zero;
        anim.SetBool("explode",true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
