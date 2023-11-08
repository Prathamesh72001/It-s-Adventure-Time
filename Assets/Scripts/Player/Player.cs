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
    public bool facingRight = true;
    Vector3 localScale;
    public JoyStick joyStick;
    public Vector2 MovementVector{get;private set;}
    public event Action<Vector2> OnMovement;
    public GameObject GameOverPanel;
    public GameObject GameCompletePanel;
    public GameObject HudPanel;
    public GameObject BossBattleCamera;
    public GameObject BossBattleStartArea;
    public GameObject BossBattleEndArea;
    public GameObject BossHealthBar;
    public bool isHurt=false;
    bool doubleJump;
    

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
        joyStick.onMove+=Move;
        GetCheckpoint();
    }

    public void GetCheckpoint(){
        if(PlayerPrefs.GetFloat("Checkpoint_X_Level", 0f)!=0f && PlayerPrefs.GetFloat("Checkpoint_Y_Level", 0f)!=0f){
            ButtonsManager.instance.checkPointArea.SetActive(true);
            
            //get
            float checkPoint_x=PlayerPrefs.GetFloat("Checkpoint_X_Level", 0f);
            float checkPoint_y=PlayerPrefs.GetFloat("Checkpoint_Y_Level", 0f);
            float health=PlayerPrefs.GetFloat("Checkpoint_Health", 0f);
            int score=PlayerPrefs.GetInt("Checkpoint_Score", 0);
            int diamond=PlayerPrefs.GetInt("Checkpoint_Diamond", 0);
            int key=PlayerPrefs.GetInt("Checkpoint_Key", 0);
            int bullet=PlayerPrefs.GetInt("Checkpoint_Bullet", 0);

            //set
            transform.position=new Vector3(checkPoint_x,checkPoint_y,0f);
            PlayerManager.instance.IncreaseHealth(health-5);
            PlayerManager.instance.ChangeScore(score);
            PlayerManager.instance.ChangeDiamond(diamond);
            PlayerManager.instance.ChangeKey(key);
            PlayerManager.instance.IncreaseBullet(bullet);
        }
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

        if (!CrossPlatformInputManager.GetButtonDown("Jump") && rb.velocity.y == 0){
            doubleJump=false;
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump") && !isHurt){
            if(rb.velocity.y == 0 || doubleJump){
                if(doubleJump){
                  rb.AddForce(Vector2.up * 600);
                }else{
                  rb.AddForce(Vector2.up * 900);  
                }
            doubleJump=!doubleJump;
            }
        }
            

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

        if (col.gameObject.tag.Equals("birds_moving"))
        {
            foreach(GameObject birds in GameObject.FindGameObjectsWithTag("enemies")){
                if(birds.name=="Bat"){
                    birds.GetComponent<PointToPointMovement>().shouldMove=true;
                }
            }
        }

        if (col.gameObject.tag.Equals("gamecomplete_trigger"))
        {
            PlayerManager.instance.GameCompleteCode();
        }


        if(col.gameObject.tag.Equals("bossbattlestart_trigger") ){
            
           BossHealthBar.SetActive(true);
           BossBattleCamera.SetActive(true);
           BossBattleEndArea.SetActive(true);    
           BossBattleStartArea.SetActive(true); 
           ButtonsManager.instance.StartBossBattle();
           isHurt=true;
           FindObjectOfType<DialogueTrigger>().StartDialogues();   
           Destroy(col.gameObject);
        }

        if(col.gameObject.tag.Equals("checkpoint") ){
            ButtonsManager.instance.SetCheckpoint(transform.position.x,transform.position.y);
        }

    }

    public void GameOverCode(){
        Time.timeScale = 0;
        ButtonsManager.instance.PauseMusic();
        ButtonsManager.instance.GameOver();
        HudPanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }


    IEnumerator Hurt(float health,float timeToStun,bool isBoss=false){
        if(isBoss){
            ButtonsManager.instance.Hurt();
            FindObjectOfType<Boss>().GetComponent<Animator>().SetBool("is_attacking",true);
        isHurt=true;
        anim.SetBool("is_hurt", true);
        if(facingRight){
                            rb.AddForce(Vector2.left * 1000f);
                         }else{
                             rb.AddForce(Vector2.right * 1000f);
                         }
        
        gameObject.layer=LayerMask.NameToLayer("Coins");
        PlayerManager.instance.DecreaseHealth(health);
        yield return new WaitForSeconds(timeToStun);
                anim.SetBool("is_hurt",false);
                isHurt=false;
        yield return new WaitForSeconds(1f);
        gameObject.layer=LayerMask.NameToLayer("Player");
        FindObjectOfType<Boss>().GetComponent<Animator>().SetBool("is_attacking",false);
        }else{
            ButtonsManager.instance.Hurt();
            isHurt=true;
        gameObject.layer=LayerMask.NameToLayer("Coins");
        anim.SetBool("is_hurt", true);
        PlayerManager.instance.DecreaseHealth(health);
                yield return new WaitForSeconds(timeToStun);
                anim.SetBool("is_hurt",false);
                gameObject.layer=LayerMask.NameToLayer("Player");
                isHurt=false;
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("floating_platform"))
            this.transform.parent = col.transform;

        if(col.gameObject.tag.Equals("enemies")){
            foreach(ContactPoint2D point in col.contacts){
                if(point.normal.y>=0.9f){
                    
                    if(col.gameObject.name=="Boss"){
                        rb.AddForce(Vector2.up * 700f);
                        Boss.instance.Hurt(2.5f);
                    }else{
                        rb.AddForce(Vector2.up * 350f);
                       Destroy(col.gameObject);
                    }
                }else{
                    if(col.gameObject.name=="Boss"){
                         StartCoroutine(Hurt(0.5f,0.2f,true));
                         
                    }else{
                       StartCoroutine(Hurt(0.5f,2f));
                    }
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
