using UnityEngine;
using System.Collections;

public class ChangeLog : MonoBehaviour {
	
	public GUISkin Skin;
	public GUISkin Skin2;
	public float width = 500f;
	public float height = 400f;
	
	public static string Build = "Build 32";
	private bool Active;
	private bool ChangeLogScreen;
	private Vector2 scroll = Vector2.zero;
	void OnMouseEnter()
	{
		//guiText.fontStyle = FontStyle.Bold;
		guiText.material.color = Color.gray;
		Active = true;
	}
	void OnMouseExit()
	{
		//guiText.fontStyle = FontStyle.Normal;
		guiText.material.color = Color.white;
		Active = false;
	}
	// Use this for initialization
	void Start () {
		guiText.text = Build;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Active)
		{
			if(Input.GetKeyUp(KeyCode.Mouse0))
			{
				if(ChangeLogScreen)
				{
					ChangeLogScreen = false;
				}else if(!ChangeLogScreen)
				{
					ChangeLogScreen = true;
				}
			}
		}
		if(Input.GetKeyUp(KeyCode.Escape) && ChangeLogScreen)
		{
			ChangeLogScreen = false;
		}
	}
	
	void OnGUI()
	{
		GUI.skin = Skin;
		if(ChangeLogScreen)
		{
			GUI.Window(0, new Rect((Screen.width/2)-(width/2), (Screen.height/2)-(height/2), width, height), ChangeLogWindow, "   Change Log");
		}
	}
	void ChangeLogWindow(int windowID)
	{
		string Add = " + Added ";
		string Adjust = " * Adjusted ";
		string Move = " * Moved ";
		string Change = " * Changed ";
		string Remove = " - Removed ";
		string Improve = " * Improved ";
		string Fix = " * Fixed ";
		GUILayout.BeginHorizontal();
		GUILayout.Space( 20.0F );
		GUILayout.BeginVertical();
		GUILayout.Space( 15.0F );
		scroll = GUILayout.BeginScrollView(scroll);
		Build = "Build 32";
		//BeginLog
		//Build 32
		GUILayout.Label("Build 32");
		GUILayout.Label( Add + "logs to the debug console, Shift + f1.");
		GUILayout.Label( Fix + "the stats not displaying after reloading the game(the stats were saved and loaded just not displayed).");
		//Build 31
		GUILayout.Space(10);
		GUILayout.Label("Build 31");
		GUILayout.Label( Improve + "the JC2 SAM turret's missile re-targeting.");
		//Build 30
		GUILayout.Space(10);
		GUILayout.Label("Build 30");
		GUILayout.Label( Add + "integration with kongregate API, some stats are now submitted to the leaderboard.");
		//Build 29
		GUILayout.Space(10);
		GUILayout.Label("Build 29");
		GUILayout.Label( Add + "zooming with scroll wheel and sensitivity controls for it.");
		GUILayout.Label( Add + "functionality to the controls screen.");
		GUILayout.Label( Add + "statistics tracking.");
		GUILayout.Label( Add + "Statistics screen.");
		GUILayout.Label( Adjust + "the balancing of the game, it should be slightly easier.");
		GUILayout.Label( Change + "the particle effect for the plasma cannon.");
		GUILayout.Label( Fix + "the game going an extra wave before ending.");
		GUILayout.Label( Fix + "certain GUI elements being offset when the resolution is changed, this is not perfect yet.");
		//Build 28
		GUILayout.Space(10);
		GUILayout.Label("Build 28");
		GUILayout.Label( Adjust + "the lighting.");
		GUILayout.Label( Adjust+"/ "+"changed come particle effects.");
		GUILayout.Label( Improve + "the JC2 SAM missile targeting.");
		GUILayout.Label( Improve + "preformace.");
		GUILayout.Label( Fix + "physics objects not pausing correctly.");
		GUILayout.Label( Fix + "some particle effects not pausing.");
		//Build 27
		GUILayout.Space(10);
		GUILayout.Label("Build 27");
		GUILayout.Label( Add + "a new tower that does area damage, the JC2 SAM turret.");
		GUILayout.Label( Fix + "another bug that alloed towers to be bought for free.");
		//Build 26
		GUILayout.Space(10);
		GUILayout.Label("Build 26");
		GUILayout.Label( Add + "damge delt indicator.");
		GUILayout.Label( Add + "an indicator sound for when you attempt to buy a tower with insuffecent credits.");
		GUILayout.Label( Change + "the font, again.");
		GUILayout.Label( Fix + "a bug that allowed towers to be bought for free.");
		//Build 25
		GUILayout.Space(10);
		GUILayout.Label("Build 25");
		GUILayout.Label( Add + "a preview of the tower in the buy menu.");
		GUILayout.Label( Add + "the ability to quick build towers, press a # key to choose a tower and 'B' to build it.");
		GUILayout.Label( Add + "the ability to build the last tower you built by pressing 'B'.");
		GUILayout.Label( Adjust + "the countdown to be less obtructive.");
		GUILayout.Label( Change + "the tower menu to not close after you buy a tower, to help ease of use.");
		GUILayout.Label( Fix + "the build menu not closing after closing the tower menu.");
		GUILayout.Label( Fix + "graphical bugs with the tutorial.");
		//Build 24
		GUILayout.Space(10);
		GUILayout.Label("Build 24");
		GUILayout.Label(Add + "a new system for building and selecting towers, this looks better and will make it easier to add more towers.");
		GUILayout.Label(Add + "updated icons for towers, they are not final.");
		GUILayout.Label(Change + "various minor things.");
		//Build 23
		GUILayout.Space(10);
		GUILayout.Label("Build 23");
		GUILayout.Label( Add + "a count down to when the game starts and when the next wave starts.");
		GUILayout.Label( Adjust + "the positionaing and scale of some items on the HUD.");
		GUILayout.Label( Fix + "the wave delay not working correctly in some cases.");
		GUILayout.Label( Fix + "a few errors with timeing.");
		GUILayout.Label( Improve + "the tutorials and made them more visable.");
		//Build 22
		GUILayout.Space(10);
		GUILayout.Label("Build 22");
		GUILayout.Label( Add + "a delay before enemies start spawning.");
		GUILayout.Label( Add + "a delay between waves.");
		GUILayout.Label( Add + "a health bar above towers, they overlap when zoomed out, that will be fixed later.");
		GUILayout.Label( Add + "a muzzle effect for the plasma Cannon.");
		GUILayout.Label( Fix + "font mismatch.");
		//Build 21
		GUILayout.Space(10);
		GUILayout.Label("Build 21");
		GUILayout.Label( Change + "the font to one that supports more character.");
		GUILayout.Label( Fix + "when you upgrade the firerate of a tower to it's max level causing a crash.");
		GUILayout.Label( Fix + "display error with the upgrade menu, it has been reorganized to help ease of use.");
		GUILayout.Label( Fix + "some upgrades not being availabe on the plasma cannon.");
		//Build 20
		GUILayout.Space(10);
		GUILayout.Label("Build 20");
		GUILayout.Label( Fix + "not being able to pause after the tutorial.");
		GUILayout.Label( Fix + "crash cause when the auto upgrade repair is purchased.");
		GUILayout.Label( Fix + "the upgrade window not reopening after clicking repair.");
		GUILayout.Label( Fix + "the correct current aromor of a tower not being shown in the upgrade menu.");
		//Build 19
		GUILayout.Space(10);
		GUILayout.Label("Build 19");
		GUILayout.Label( Add + "tower upgrades screen to towers.");
		GUILayout.Label( Add + "a tutorial on first time launch.");
		GUILayout.Label( Add + "getting started tutorial, that also explains the Upgrade System.");
		GUILayout.Label( Add + "crititcal hits, auto repair, and looting upgrades.");
		//GUILayout.Label( Add + "improves tower selection screen.");
		GUILayout.Label( Add + "indicator on whether you have enough credits to repair something.");
		GUILayout.Label( Remove + "the duplicate tower icon for now.");
		//Build 18
		GUILayout.Space(10);
		GUILayout.Label("Build 18");
		GUILayout.Label( Add + "a difficulty selection screen.");
		GUILayout.Label( Add + "statitics tracking in prep for integration with konregate api.");
		GUILayout.Label( Add + "all controls to the controls screen, cant  be changed yet.");
		GUILayout.Label( Change + "the spawn rate of enemies.");
		GUILayout.Label( Fix + "the menus not working correctly in webplayer.");
		GUILayout.Label( Fix + "being able to pause when you win or lose.");
		//Build 17
		GUILayout.Space(10);
		GUILayout.Label("Build 17");
		GUILayout.Label( Add + "muzzle flash to the Gatling Gun.");
		GUILayout.Label( Change + "the price of the Plasma Cannon to Aid Balance.");
		GUILayout.Label( Fix + "Gatling guns dropping infite ragdolls when they die.");
		GUILayout.Label( Fix + "Plasma Cannon's ragdolls not despawning.");
		GUILayout.Label( Fix + "Collision detection for the Plasma Cannon and the Gatling Gun.");
		//Build 16
		GUILayout.Space(10);
		GUILayout.Label("Build 16");
		GUILayout.Label( Add + "a link to the forums in the updater.");
		GUILayout.Label( Add + "a gatling gun model.");
		GUILayout.Label( Add + "a nicer placeholder PlasmaCannon.");
		GUILayout.Label( Fix + "camera accelleration at higher timescales.");
		GUILayout.Label( Fix + "pause menu being affected by time scale.");
		//Build 15
		GUILayout.Space(10);
		GUILayout.Label("Build 15");
		GUILayout.Label( Fix + "Updater not being able to connect to the server.");
		//Build 14.6
		GUILayout.Space(10);
		GUILayout.Label("Build 14.6");
		GUILayout.Label( Add + "a error window that shows if there was an error when checking for updates.");
		GUILayout.Label( Fix + "some formatting errors.");
		//Build 14.5
		GUILayout.Space(10);
		GUILayout.Label("Build 14.5");
		GUILayout.Label( Fix + "the version checker not running at startup.");
		//Build 14
		GUILayout.Space(10);
		GUILayout.Label("Build 14");
		GUILayout.Label( Add + "a semi-functional controls screen. DO NOT USE!");
		GUILayout.Label( Add + "randomly changing sky for the main menu.");
		GUILayout.Label( Add + "a version checker to check for updates.");
		GUILayout.Label( Change + "the directional light's direction.");
		GUILayout.Label( Change + "the increment of of time scale to .5.");
		GUILayout.Label( Change + "the font to a font that supports lowercase characters.");
		GUILayout.Label( Improve + "preformance.");
		//GUILayout.Label( Add + "a slow rotational animation to the main menu.");
		//Build 13
		GUILayout.Space(10);
		GUILayout.Label("Build 13");
		GUILayout.Label( Add + "a way to repair towers.");
		GUILayout.Label( Add + "a color animation to the health bar that plays when health is critical, <50%.");
		GUILayout.Label( Add + "time compression up to 3x.");
		GUILayout.Label( Add + "a temperary low res lightmap.");
		GUILayout.Label( Adjust + "the loot droped by enimes");
		GUILayout.Label( Change + "the start credits to 2000.");
		GUILayout.Label( Change + "the enemy spawning system to spawn the enimes in waves with each wave being stronger than the last.");
		GUILayout.Label( Change + "the way the sell price is determined to be based on how much health is remaining.");
		GUILayout.Label( Change + "the positioning of the info window to never be offscreen.");
		GUILayout.Label( Fix + "all issues with pausing and time.");
		GUILayout.Label( Fix + "the info window for towers not being in the correct position.");
		GUILayout.Label( Improve + "tower Ai, and creep Ai to only shoot at target it can see.");
		GUILayout.Label( " * Minor cleanup, and bug fixes.");
		//Build 12
		GUILayout.Space(10);
		GUILayout.Label("Build 12");
		GUILayout.Label( Add + "a way to sell a tower youve built.");
		GUILayout.Label( Add + "indicator that shows how many enimies have been spawned of the total.");
		GUILayout.Label( Adjust + "the cost and stats of the various level of towers.");
		GUILayout.Label( Adjust + "the ammount of credits gained from killing creep.");
		GUILayout.Label( Adjust + "the UI color is little");
		GUILayout.Label( Change + "the way the stats of incomming creeps are buffed, it is now based on the percent out of the total spawned. The more the spawns the more powerfull it will be.");
		GUILayout.Label( Fix + "minor bugs.");
		//Build 11
		GUILayout.Space(10);
		GUILayout.Label("Build 11");
		GUILayout.Label( Add + "abillity to determine is a location is valid to build on.");
		GUILayout.Label( Add + "ability to build tower and creeps on the battlefeild.");
		GUILayout.Label( Add + "credits system.");
		GUILayout.Label( Add + "a way to gain and spend credits.");
		GUILayout.Label( Add + "visable faction colors, temperary.");
		GUILayout.Label( Add + "a overall health system.");
		GUILayout.Label( Add + "a way to determine win or lose.");
		GUILayout.Label( Add + "new effects.");
		GUILayout.Label( Add + "waves of incomming creeps with each wave being stronger than the last.");
		GUILayout.Label( Add + "new system for buying towers.");
		GUILayout.Label( Add + "victory screen.");
		GUILayout.Label( Add + "defeat screen.");
		GUILayout.Label( Add + "action!");
		GUILayout.Label( Remove + "all prebuild towers");
		GUILayout.Label( Improve + "AI of creeps.");
		GUILayout.Label( Improve + "AI of towers.");
		GUILayout.Label( Fix + "object building gui.");
		
		//Build 10
		GUILayout.Space(10);
		GUILayout.Label("Build 10");
		GUILayout.Label( Add + "numbers to the 'Build Menu' icons");
		GUILayout.Label( Move + "the tooltip up by 1 pixel.");
		GUILayout.Label( Change + "the size of the changelog text area.");
		GUILayout.Label( Improve + "organization of changelog.");
		GUILayout.Label( Fix + "bugs.");
		//Build 9
		GUILayout.Space(10);
		GUILayout.Label("Build 9");
		GUILayout.Label( Add + "ability to initiate Object spawning.");
		GUILayout.Label( Add + "audio Options.");
		GUILayout.Label( Add + "health and Armor Calculations.");
		GUILayout.Label( Add + "death Effect.");
		GUILayout.Label( Add + "BGM to the level");
		GUILayout.Label( Adjust + "mouse controlled pan, not perfected yet.");
		GUILayout.Label( Change + "damagle dealth by AI's.");
		GUILayout.Label( Change + "the color of tiles that are valid to build on.");		GUILayout.Label( Change + "all panning to use the x and y sensitivies set in gameplay options, takes an average of the two sensitivies for panning.");
		GUILayout.Label( Improve + "readibilty of the changelog");
		GUILayout.Label( " * More Cleaned up");
		//Build 8
		GUILayout.Space(10);
		GUILayout.Label("Build 8");
		GUILayout.Label( Add + "changelog");
		GUILayout.Label( Add + "'Done' button to all options windows.");
		GUILayout.Label( Add + "new GUI.");
		GUILayout.Label( Add + "panning when holding right mouse button.");
		GUILayout.Label( Fix + "bugs");
		GUILayout.Label( " * Cleaned up Code");
		//EndLog
		GUILayout.EndScrollView();
		GUILayout.Space( 45.0f);
		GUILayout.EndVertical();
		GUILayout.Space( 25.0f);
		GUILayout.EndHorizontal();
		GUI.skin = Skin2;
		if(GUI.Button(new Rect((width/2) - 47.5f, height - 50, 95, 20), "Close"))
		{
			ChangeLogScreen = false;
		}
	}
}
