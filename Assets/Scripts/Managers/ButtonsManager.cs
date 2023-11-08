using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class ButtonsManager : MonoBehaviour
{
    public static ButtonsManager instance;
    [SerializeField] private AudioClip vfxMusic; 
    [SerializeField] private AudioClip coinMusic; 
    [SerializeField] private AudioClip chestMusic; 
    [SerializeField] private AudioClip gameOverMusic; 
    [SerializeField] private AudioClip gameCompleteMusic; 
    [SerializeField] private AudioClip bulletMusic;
    [SerializeField] private AudioClip hurtMusic;
    [SerializeField] private AudioClip bossMusic; 
    [SerializeField] private AudioClip backGroundMusic; 
    [SerializeField] private GameObject backGround; 
    [SerializeField] private GameObject settingsScreen; 
    [SerializeField] private GameObject exitScreen;
    [SerializeField] private GameObject creditsScreen; 
    [SerializeField] private GameObject controlsScreen; 
    [SerializeField] private GameObject newGameScreen; 
    [SerializeField] private GameObject checkMusic; 
    [SerializeField] private GameObject uncheckMusic; 
    [SerializeField] private GameObject checkVFX; 
    [SerializeField] private GameObject uncheckVFX; 
    [SerializeField] private GameObject loadingScreen; 
    public GameObject checkPointArea; 
    [SerializeField] private Button levelsButton; 
    [SerializeField] private Slider slider; 
    [SerializeField] private Text progressText; 
    [SerializeField] private Scrollbar creditsScrollBar; 
    [SerializeField] private Scrollbar controlsScrollBar; 
    private AudioSource vfxAudio; 
    private AudioSource bgAudio; 
    private Image checkMusicImage; 
    private Image uncheckMusicImage; 
    private Image checkVFXImage; 
    private Image uncheckVFXImage;
    public bool isMainMenu;
    [SerializeField] Rigidbody2D bullet;
    [SerializeField] Transform barrel;
    public float bulletSpeed=500f;
    float fireRate=1f;
    float nextFire=0f;
    public int currentLevel=0;
    bool isNewGame=false;

   
    protected void Start()
    {
        if (instance == null)
            instance = this;

        StartCoroutine(CheckPermissions());

        vfxAudio = GetComponent<AudioSource>();        
        bgAudio = backGround.GetComponent<AudioSource>();
        checkMusicImage = checkMusic.GetComponent<Image>();
        uncheckMusicImage = uncheckMusic.GetComponent<Image>();
        checkVFXImage = checkVFX.GetComponent<Image>();
        uncheckVFXImage = uncheckVFX.GetComponent<Image>();
        
        int levelReached=PlayerPrefs.GetInt("levelReached",1);
        Debug.Log(levelReached);
        
        if(levelReached>1 && isMainMenu){
            levelsButton.interactable=true;
        }

        if (PlayerPrefs.GetInt("isMusicOn",1) == 1)
        {
            bgAudio.volume = 1;
            checkMusicImage.color = new Color32(255, 255, 255, 255);
            uncheckMusicImage.color = new Color32(40, 40, 40, 255);
        }
        else
        {
            bgAudio.volume = 0;
            checkMusicImage.color = new Color32(40, 40, 40, 255);
            uncheckMusicImage.color = new Color32(255, 255, 255, 255);
        }
        
        if (PlayerPrefs.GetInt("isVFXOn",1) == 1)
        {
            vfxAudio.volume = 1;
            checkVFXImage.color = new Color32(255, 255, 255, 255);
            uncheckVFXImage.color = new Color32(40, 40, 40, 255);
        }
        else
        {
            vfxAudio.volume = 0;
            checkVFXImage.color = new Color32(40, 40, 40, 255);
            uncheckVFXImage.color = new Color32(255, 255, 255, 255);
        }

        PlayMusic();

    }

    IEnumerator CheckPermissions(){
        if(!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite)){
            Debug.Log("Permission asked");
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        yield return null;
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isMainMenu)
                    Exit();
                else
                    LevelButtons(0);
            }
        }
    }

    public void PlayStop()
    {
      vfxAudio.clip = vfxMusic;
      vfxAudio.Play();
    }

    public void SetCheckpoint(float checkPoint_x,float checkPoint_y){
        if(PlayerPrefs.GetFloat("Checkpoint_X_Level", 0f)==0f && PlayerPrefs.GetFloat("Checkpoint_Y_Level", 0f)==0f){
            checkPointArea.SetActive(true);
            PlayerPrefs.SetFloat("Checkpoint_X_Level", checkPoint_x);
            PlayerPrefs.SetFloat("Checkpoint_Y_Level", checkPoint_y);
            PlayerPrefs.SetFloat("Checkpoint_Health", PlayerManager.instance.health);
            PlayerPrefs.SetInt("Checkpoint_Score", PlayerManager.instance.score);
            PlayerPrefs.SetInt("Checkpoint_Diamond", PlayerManager.instance.diamond);
            PlayerPrefs.SetInt("Checkpoint_Key", PlayerManager.instance.key);
            PlayerPrefs.SetInt("Checkpoint_bullet", PlayerManager.instance.bullet);
            }
    }

    public void ForgetCheckPoint(){
        PlayerPrefs.SetFloat("Checkpoint_X_Level", 0f);
        PlayerPrefs.SetFloat("Checkpoint_Y_Level", 0f);
    }

    public void FireBullet(){
        if(PlayerManager.instance.bullet!=0 && Time.time>nextFire){
            vfxAudio.clip = bulletMusic;
      vfxAudio.Play();
            nextFire=Time.time*fireRate;
        var spawnedBullet= Instantiate(bullet, barrel.position, barrel.rotation);
        if(Player.instance.facingRight)
            spawnedBullet.AddForce(barrel.right*bulletSpeed);
        else
            spawnedBullet.AddForce(barrel.right*-bulletSpeed);
        PlayerManager.instance.DecreaseBullet(1);
        }
    }

    public void StartBossBattle(){
        bgAudio.clip=bossMusic;
        bgAudio.Play();
    }

    public void EndBossBattle(){
        bgAudio.clip=backGroundMusic;
        bgAudio.Play();
    }

    public void CoinCollected()
    {
        vfxAudio.clip = coinMusic;
        vfxAudio.Play();
    }

    public void Hurt()
    {
        vfxAudio.clip = hurtMusic;
        vfxAudio.Play();
    }

    public void GameOver(){
        vfxAudio.clip=gameOverMusic;
        vfxAudio.Play();
    }

    public void GameComplete(){
        vfxAudio.clip=gameCompleteMusic;
        vfxAudio.Play();
    }

     public void ChestOpened()
    {
        vfxAudio.clip = chestMusic;
        vfxAudio.Play();
    }

    public void PauseMusic(){
        bgAudio.Pause();
    }

    public void PlayMusic(){
        bgAudio.Play();
    }

    public void NewGame()
    {
        PlayStop();
        PlayerPrefs.SetInt("levelReached",1);
        SaveSystem.DeleteSavedData();
        LevelButtons(1);
    }
    
    public void Continue()
    {
        PlayStop();
        LevelButtons(2);

    }

    public void Setting()
    {
        Time.timeScale = 0;
        PlayStop();
        creditsScrollBar.value=1;
        controlsScrollBar.value=1;
        exitScreen.SetActive(false);
        creditsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }
    
    public void Exit()
    {
        Time.timeScale = 0;
        PlayStop();
        exitScreen.SetActive(true);
        settingsScreen.SetActive(false);

    }

    public void StartNewGame(){
        int level=PlayerPrefs.GetInt("levelReached",1);
        if(level>1){
        isNewGame=true;
        Time.timeScale = 0;
        PlayStop();
        newGameScreen.SetActive(true);
        }else{
            NewGame();
        }
    }

    public void Yes()
    {
        PlayStop();
        Application.Quit();
    }
    
    public void No()
    {
        
        PlayStop();
        if(Time.timeScale == 0){
            Time.timeScale = 1;
        }
        if(isNewGame){
            newGameScreen.SetActive(false);
            isNewGame=false;
        }else{
                    exitScreen.SetActive(false);

        }
    }

    public void Close()
    {
        PlayStop();
        if(Time.timeScale == 0){
            Time.timeScale = 1;
        }
        settingsScreen.SetActive(false);
    }

    public void CheckMusic()
    {
        PlayStop();
        PlayerPrefs.SetInt("isMusicOn", 1);
        bgAudio.volume = 1;
        checkMusicImage.color = new Color32(255, 255, 255, 255);
        uncheckMusicImage.color = new Color32(40, 40, 40, 255);
    }

    public void UncheckMusic()
    {
        PlayStop();
        PlayerPrefs.SetInt("isMusicOn", 0);
        bgAudio.volume = 0;
        checkMusicImage.color = new Color32(40, 40, 40, 255);
        uncheckMusicImage.color = new Color32(255, 255, 255, 255);
    }
    
    public void CheckVFX()
    {
        PlayStop();
        PlayerPrefs.SetInt("isVFXOn", 1);
        vfxAudio.volume = 1;
        checkVFXImage.color = new Color32(255, 255, 255, 255);
        uncheckVFXImage.color = new Color32(40, 40, 40, 255);
    }

    public void UncheckVFX()
    {
        
        PlayStop();
        PlayerPrefs.SetInt("isVFXOn", 0);
        vfxAudio.volume = 0;
        checkVFXImage.color = new Color32(40, 40, 40, 255);
        uncheckVFXImage.color = new Color32(255, 255, 255, 255);
    }

    public void Controls()
    {
        Time.timeScale = 0;
        PlayStop();
        controlsScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }

    public void Credits()
    {
       Time.timeScale = 0;
        PlayStop();
        creditsScreen.SetActive(true);
        settingsScreen.SetActive(false);

    }

    public void GameCompleteCode(int stars){
        LevelData l=SaveSystem.Load(currentLevel);
        if(l!=null){
            if(stars>l.stars){
              SaveSystem.Save(new LevelData(currentLevel,stars));
            }
        }else{
            SaveSystem.Save(new LevelData(currentLevel,stars));
        }

        int levelReached=PlayerPrefs.GetInt("levelReached",1);
        Debug.Log("levelReached"+levelReached);
        int newReached=currentLevel+1;
        if(newReached>levelReached){
            PlayerPrefs.SetInt("levelReached",newReached);
            SaveSystem.Save(new LevelData(newReached));
        }
    }


    public void LevelButtons(int levelPos)
    {
        PlayStop();
        if(Time.timeScale == 0){
            Time.timeScale = 1;
        }
        if(currentLevel==0 || levelPos!=currentLevel+2){
          ForgetCheckPoint();
        }
        StartCoroutine(LoadAsynchronously(levelPos));
    }

    IEnumerator LoadAsynchronously(int levelPos)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelPos);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            int p = (int) progress * 100;
            progressText.text = p + " %";
            yield return null;
        }
    }

}
