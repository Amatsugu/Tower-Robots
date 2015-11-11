using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Texture2D texture;
    static LoadingScreen instance;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;    
        gameObject.AddComponent<GUITexture>().enabled = false;
        guiTexture.texture = texture;
        transform.position = new Vector3(0.5f, 0.5f, 10000f);
        DontDestroyOnLoad(this); 
    }
	void Update()
	{
		if(Application.isLoadingLevel)
		{
			instance.guiTexture.enabled = true;
		}else
		{
			instance.guiTexture.enabled = false;
		}
	}
    public void Load(int index)
    {
        if (NoInstance()) return;
        instance.guiTexture.enabled = true;
        Application.LoadLevel(index);
		Debug.Log("Level Loaded: "  + index.ToString());
		DebugConsole.Log("Level Loaded: "  + index.ToString());
        instance.guiTexture.enabled = false;
    }

    /*public static void Load(string name)
    {
        if (NoInstance()) return;
        instance.guiTexture.enabled = true;
        Application.LoadLevel(name);
        instance.guiTexture.enabled = false;
    }*/

    static bool NoInstance()
    {
        if (!instance)
            Debug.LogError("Loading Screen is not existing in scene.");
        return instance;
    }
}