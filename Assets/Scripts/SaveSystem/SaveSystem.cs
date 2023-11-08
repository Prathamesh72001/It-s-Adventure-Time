using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    static readonly string SAVE_FOLDER=Application.persistentDataPath+"/Saves/";

    public static void Init()
    {
        Debug.Log("path "+SAVE_FOLDER);
        if(!Directory.Exists(SAVE_FOLDER)){
        Directory.CreateDirectory(SAVE_FOLDER);
        Debug.Log("Created");
        }
    }


    public static void Save(LevelData levelData){
        Init();
        string data=JsonUtility.ToJson(levelData);
        Debug.Log(data);

        // if(!File.Exists(SAVE_FOLDER+"/save"+levelData.level+".txt")){
        File.WriteAllText(SAVE_FOLDER+"/save"+levelData.level+".txt",data);
        Debug.Log("Saved");
        // }else{
        //     Debug.Log("Not Saved");
        // }
    }

    public static LevelData Load(int level){        
        if(File.Exists(SAVE_FOLDER+"/save"+level+".txt")){
            string saveString=File.ReadAllText(SAVE_FOLDER+"/save"+level+".txt");
            Debug.Log(saveString);
            LevelData l=JsonUtility.FromJson<LevelData>(saveString);
            return l;
            
        }else{
            return null;
        }
    }

    public static void DeleteSavedData(){
        if(Directory.Exists(SAVE_FOLDER)){
            Directory.Delete(SAVE_FOLDER,true);
            Save(new LevelData(1));
        }
    }
    
}
