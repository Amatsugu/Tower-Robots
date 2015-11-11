using UnityEngine;
using System.Collections;

public class DMGText : MonoBehaviour {
	// Use this for initialization
	private Color c;
	public void SetInfo(string Text, Color color) 
	{
		gameObject.GetComponent<TextMesh>().text = Text;
		gameObject.renderer.material.color = color;
		c = color;
		StatsManager.AddDamageDealt((int)float.Parse(Text));
	}
	void Update()
	{
		if(!PauseManager.inst.isPasued)
		{
			transform.Translate(Vector3.up * Time.deltaTime);
			gameObject.renderer.material.color = new Color(c.r, c.g, c.b, gameObject.renderer.material.color.a -.5f *Time.deltaTime );
		}
	}
}
