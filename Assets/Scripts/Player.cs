using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
        public static Player instance;
    Rigidbody2D rb;
    Animator anim;
    float dirX;
    public float moveSpeed=10f;
    bool facingRight = true;
    Vector3 localScale;
    public JoyStick joyStick;
    public Vector2 MovementVector{get;private set;}
    public event Action<Vector2> OnMovement;
    public GameObject GameOverPanel;
    public GameObject GameCompletePanel;
    public GameObject HudPanel;
    public bool isHurt=false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
        joyStick.onMove+=Move;
    }

    void Move(Vector2 input){
        MovementVector=input;
        OnMovement?.Invoke(MovementVector);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isHurt){
        dirX = MovementVector.x*moveSpeed;
        }else{
            dirX=0;
        }
        //dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (CrossPlatformInputManager.GetButtonDown("Jump") && rb.velocity.y == 0 && !isHurt)
            rb.AddForce(Vector2.up * 900f);

        if (Mathf.Abs(dirX) > 0 && rb.velocity.y == 0)
            anim.SetBool("is_running", true);
        else
            anim.SetBool("is_running", false);

        if (rb.velocity.y == 0)
        {
            anim.SetBool("is_jumping", false);
            anim.SetBool("is_falling", false);

        }

        if (rb.velocity.y > 0)
        {
            anim.SetBool("is_jumping", true);
            anim.SetBool("is_falling", false);
        }

        if (rb.velocity.y < 0)
        {
            anim.SetBool("is_jumping", false);
            anim.SetBool("is_falling", true);

        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    void LateUpdate()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("coin"))
        {
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag.Equals("gameover_trigger"))
        {
            GameOverCode();
            
        }

        if (col.gameObject.tag.Equals("gamecomplete_trigger"))
        {
            Time.timeScale = 0;
                        ButtonsManager.instance.PauseMusic();

                        ButtonsManager.instance.GameComplete();
                                    HudPanel.SetActive(false);

            GameCompletePanel.SetActive(true);
        }

        if(col.gameObject.tag.Equals("enemies")){
            
                                StartCoroutine(Hurt(1f));
               
        }

    }

    public void GameOverCode(){
        Time.timeScale = 0;
                        ButtonsManager.instance.PauseMusic();

            ButtonsManager.instance.GameOver();
            HudPanel.SetActive(false);
            GameOverPanel.SetActive(true);
    }

    IEnumerator Hurt(float health){
        isHurt=true;
                            gameObject.layer=LayerMask.NameToLayer("Coins");
        anim.SetBool("is_hurt", true);
        PlayerManager.instance.DecreaseHealth(health);
                yield return new WaitForSeconds(2f);
                anim.SetBool("is_hurt",false);
                gameObject.layer=LayerMask.NameToLayer("Player");
                isHurt=false;
 
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("floating_platform"))
            this.transform.parent = col.transform;

        if(col.gameObject.tag.Equals("enemies")){
            foreach(ContactPoint2D point in col.contacts){
                if(point.normal.y>=0.9f){
                    rb.AddForce(Vector2.up * 350f);
            Destroy(col.gameObject);
                }else{
                                StartCoroutine(Hurt(0.5f));
                }
            }
        }
        
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("floating_platform"))
            this.transform.parent = null;

    }
}
