using UnityEngine;
using System.Collections;
[AddComponentMenu("Camera/CameraControls")]
public class CameraControls : MonoBehaviour 
{
	//vars
	public float CameraSpeed = 300;
	//public GameObject UP, DOWN, LEFT, RIGHT;
	public GameObject particle;
	public Transform PauseMenu;
	public Transform PrePause;
	public GameObject PauseCam;
	public SmoothCam s;
	private UP up;
	private DOWN down;
	private LEFT left;
	private RIGHT right;
	private float CamSpeed;
	public float XMax, XMin, YMax, YMin;
	public float ZMax, ZMin;
	public static CameraControls inst;
	//public float X, Y;
	//private
	private float XSen, YSen, YSenI, Sen;
	//KeyBindings
	private int ZoomOut;
	private int ZoomIn;
	private float ZSen;
	// Use this for initialization
	void Awake () 
	{
		s = PauseCam.GetComponent<SmoothCam>();
		//up = UP.GetComponent<UP>();
		//down = DOWN.GetComponent<DOWN>();
		//left = LEFT.GetComponent<LEFT>();
		//right = RIGHT.GetComponent<RIGHT>();
		XSen = PlayerPrefs.GetFloat("XSen", 8);
		YSen = PlayerPrefs.GetFloat("YSen", 8);
		ZSen = PlayerPrefs.GetFloat("ZSen", 15) * 10;
		ZoomIn = PlayerPrefs.GetInt("ZoomIN", (int)KeyCode.Q);
		ZoomOut = PlayerPrefs.GetInt("ZoomOUT", (int)KeyCode.E);
		CamSpeed = ((XSen * YSen)/2)*CameraSpeed;
		DebugConsole.Log("CamSpeed: " + CamSpeed);
		inst = this;
	}
	public void UpdatePrefs()
	{
		XSen = PlayerPrefs.GetFloat("XSen", 8);
		YSen = PlayerPrefs.GetFloat("YSen", 8);
		ZSen = PlayerPrefs.GetFloat("ZSen", 15) * 10;
		CamSpeed = ((XSen * YSen)/2)*CameraSpeed;
		ZoomIn = PlayerPrefs.GetInt("ZoomIN", (int)KeyCode.Q);
		ZoomOut = PlayerPrefs.GetInt("ZoomOUT", (int)KeyCode.E);
	}
	public void UpdatePref(float timeScale)
	{
		if(timeScale > 1)
		{
			CamSpeed = ((XSen * YSen)/2)*CameraSpeed;
			CamSpeed /= timeScale-1;
			XSen = PlayerPrefs.GetFloat("XSen", 8)/ (timeScale-1);
			YSen = PlayerPrefs.GetFloat("YSen", 8)/ (timeScale-1);
			
		}else if(timeScale <= 0)
		{
			CamSpeed = ((XSen * YSen)/2)*CameraSpeed;
			CamSpeed /= timeScale;
			XSen = PlayerPrefs.GetFloat("XSen", 8)/ (timeScale);
			YSen = PlayerPrefs.GetFloat("YSen", 8)/ (timeScale);
			
		}
	}
	// Update is called once per frame
	void Update () 
	{
		if(!PauseManager.inst.isPasued)
		{
			Vector3 XMaxpos = new Vector3(XMax , transform.position.y, transform.position.z);
			Vector3 XMinpos = new Vector3(XMin , transform.position.y, transform.position.z);
			Vector3 YMaxpos = new Vector3(transform.position.x , transform.position.y , YMax);
			Vector3 YMinpos = new Vector3(transform.position.x , transform.position.y , YMin);
			Vector3 ZMaxPos = new Vector3(transform.position.x, ZMax, transform.position.z);
			Vector3 ZMinPos = new Vector3(transform.position.x, ZMin, transform.position.z);
			//Limit Pos
			if(transform.position.x > XMax)
				transform.position = XMaxpos;// * Time.deltaTime);
			if(transform.position.x < XMin)
				transform.position = XMinpos;// * Time.deltaTime);
			if(transform.position.z > YMax)
				transform.position = YMaxpos;// * Time.deltaTime);
			if(transform.position.z < YMin)
				transform.position = YMinpos;// * Time.deltaTime);
			if(transform.position.y > ZMax)
				transform.position = ZMaxPos;
			if(transform.position.y < ZMin)
				transform.position = ZMinPos;
			//Jump
			if(Input.GetKey((KeyCode)PlayerPrefs.GetInt("Forward")) && transform.position.x < XMax)
				transform.position = new Vector3(transform.position.x + ((CamSpeed/Time.timeScale) * Time.deltaTime), transform.position.y, transform.position.z);
			if(Input.GetKey((KeyCode)PlayerPrefs.GetInt("Backward")) && transform.position.x > XMin)
				transform.position = new Vector3(transform.position.x - ((CamSpeed/Time.timeScale) * Time.deltaTime), transform.position.y, transform.position.z);
			if(Input.GetKey((KeyCode)PlayerPrefs.GetInt("Left")) && transform.position.z < YMax)
				transform.position = new Vector3(transform.position.x , transform.position.y , transform.position.z + ((CamSpeed/Time.timeScale) * Time.deltaTime));
			if(Input.GetKey((KeyCode)PlayerPrefs.GetInt("Right")) && transform.position.z > YMin)
				transform.position = new Vector3(transform.position.x , transform.position.y , transform.position.z - ((CamSpeed/Time.timeScale) * Time.deltaTime));
			//ZoomOut
			if(Input.GetKey((KeyCode)ZoomOut) && transform.position.y < ZMax)
				transform.position = new Vector3(transform.position.x, transform.position.y + ((CamSpeed/Time.timeScale) * Time.deltaTime), transform.position.z);
			//ZoomIn
			if(Input.GetKey((KeyCode)ZoomIn) && transform.position.y > ZMin)
				transform.position = new Vector3(transform.position.x, transform.position.y - ((CamSpeed/Time.timeScale) * Time.deltaTime), transform.position.z);
			//Up/Down
			transform.position = new Vector3(transform.position.x, transform.position.y + ((Input.GetAxis("MouseWheel")*-1) * (ZSen/Time.timeScale)) * Time.smoothDeltaTime, transform.position.z);
			if(Input.GetKey((KeyCode)PlayerPrefs.GetInt("Pan")))
			{
				Screen.lockCursor = true;
				float x = ((Input.GetAxis("Mouse X")* -1) * XSen)/2;
				float y = (Input.GetAxis("Mouse Y") * YSen)/2;
				bool canMove;
				if(transform.position.x + y < XMax && transform.position.x - y > XMin || transform.position.z + x < YMax && transform.position.z - x > YMin)
				{
					canMove = true;
				}else
				{
					canMove = false;
				}
				if(canMove)
					transform.position = new Vector3(transform.position.x + (y/Time.timeScale), transform.position.y, transform.position.z + (x/Time.timeScale));
			}
			else
			{
				Screen.lockCursor = false;
			}
		}
	}
}
