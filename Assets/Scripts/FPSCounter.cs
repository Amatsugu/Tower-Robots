using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {
    public float updateInterval = 0.5F;
	public bool Active = false;
    private double lastInterval;
    private int frames = 0;
    private float fps;
	void Awake()
	{
		int FPS = PlayerPrefs.GetInt("FPSCounter", 0);
		if(FPS == 0)
		{
			Active = false;
		}else if(FPS == 1)
		{
			Active = true;
		}
	}
    void Start() {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }
    void Update() {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval) {
            fps = frames / (timeNow - (float)lastInterval);
            frames = 0;
            lastInterval = timeNow;
        }
		if(Input.GetKeyUp(KeyCode.F3))
		{
			int x;
			if(Active)
			{
				Active = false;
				x = 0;
				PlayerPrefs.SetInt("FPSCounter", x);
			}else if(!Active)
			{
				Active = true;
				x = 1;
				PlayerPrefs.SetInt("FPSCounter", x);
			}
		}
		if(Active)
		{
			gameObject.guiText.text = "" + fps.ToString("f2")+" FPS";
		}else if(!Active)
		{
			gameObject.guiText.text = "";
		}
    }
}