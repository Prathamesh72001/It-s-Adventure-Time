using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    public static ButtonsManager instance;
    [SerializeField] private AudioClip vfxMusic; 
    [SerializeField] private AudioClip coinMusic; 
    [SerializeField] private AudioClip chestMusic; 
    [SerializeField] private AudioClip gameOverMusic; 
    [SerializeField] private AudioClip gameCompleteMusic; 
    [SerializeField] private GameObject backGround; 
    [SerializeField] private GameObject settingsScreen; 
    [SerializeField] private GameObject exitScreen; 
    [SerializeField] private GameObject checkMusic; 
    [SerializeField] private GameObject uncheckMusic; 
    [SerializeField] private GameObject checkVFX; 
    [SerializeField] private GameObject uncheckVFX; 
    [SerializeField] private GameObject loadingScreen; 
    [SerializeField] private Button levelsButton; 
    [SerializeField] private Slider slider; 
    [SerializeField] private Text progressText; 
    private AudioSource vfxAudio; 
    private AudioSource bgAudio; 
    private Image checkMusicImage; 
    private Image uncheckMusicImage; 
    private Image checkVFXImage; 
    private Image uncheckVFXImage;
    public bool isMainMenu;

   
    protected void Start()
    {
        if (instance == null)
            instance = this;

        vfxAudio = GetComponent<AudioSource>();
        
        bgAudio = backGround.GetComponent<AudioSource>();
        checkMusicImage = checkMusic.GetComponent<Image>();
        uncheckMusicImage = uncheckMusic.GetComponent<Image>();
        checkVFXImage = checkVFX.GetComponent<Image>();
        uncheckVFXImage = uncheckVFX.GetComponent<Image>();

        int levelReached=PlayerPrefs.GetInt("levelReached",1);
        if(levelReached>1 && isMainMenu){
            levelsButton.interactable=true;
        }

        if (PlayerPrefs.GetInt("isMusicOn") == 1)
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
        
        if (PlayerPrefs.GetInt("isVFXOn") == 1)
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

    public void CoinCollected()
    {
        vfxAudio.clip = coinMusic;
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
        exitScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }
    
    public void Exit()
    {
        Time.timeScale = 0;
        PlayStop();
        exitScreen.SetActive(true);
        settingsScreen.SetActive(false);

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
        exitScreen.SetActive(false);
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
        PlayStop();
    }

    public void Credits()
    {
        PlayStop();
    }


    public void LevelButtons(int levelPos)
    {
        PlayStop();
        if(Time.timeScale == 0){
            Time.timeScale = 1;
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
