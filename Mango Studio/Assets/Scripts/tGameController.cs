using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Tutorial{
public class tGameController : MonoBehaviour
{

    public int boardWidth, boardHeight; // board size init is in Unity editor
    private GameObject playerFolder;// folders for object organization
	public List<GameObject> bulletsFolder = new List<GameObject>();


    private List<tPlayer> players; // list of all placed players
//	******** FELIPE LOOK HERE FOR current player ********
	public tPlayer currentplayer;

//	******** FELIPE LOOK HERE FOR Shadow CHARACTER List ********
	public List<tPlayer> shadowPlayers = new List<tPlayer>();
    // Beat tracking
    private float clock;
    private float startTime;
    private float BEAT = .5f;
    private int numBeats = 0;
    int playerbeaten = 0;
    int playernum = 0;
	private int playertype = 0;
	public Text HealthText;
	private List<Vector3> shadow;
	private int shadowiterator;
	private Boolean startitr;
	public tBoss THEBOSS;
	public Boolean gameover = false;
	public Boolean gamewon = false;
	//public int score = 36000;

	//A list for the environment variables
	public List<tEnvVar> envVariables = new List<tEnvVar>();
	private int maxEnv = 4;


	//******Handling player lives, as well as the order and iteration of player********
	private int playerLives = 3;
	private int[] playerOrder;// = new int[playerLives];
	public int playerOrderIndex = 0;
	private int firingDirection = 0;

	//******Boss Lives********
	private int bossTotalLives = 1;
	public int bossCurrentLife = 1;

	//Iteration Transition Variables
	private bool inTransition = false;
	private bool guiTransition = false;


	//Textures for GUI
	public Texture forSq;
	public Texture forC;
	public Texture forT;
	public Texture bossText;
	public Texture bossHpText;
	public Texture lastLifeText;
	public Texture nextBossText;
	public Texture nextUpText;
	public Texture scoreText;
	public Texture cooldownText;
	public Texture specialText;
	public Texture abilityOnText;
	public Texture gameoverText;
	public Texture gamewonText;
	public Texture startText;
	public Texture quitText;
	public Texture nextbossText;
	public Texture titleText;
	public Texture bar1;
	public Texture bar2;
	public Texture bar3;
	public Texture bar4;
	public Texture bar5;
	public Texture bar6;
	public Texture bar7;
	public Texture bar8;
	public Texture bar9;
	public Texture bar10;
    public Texture tutorialText;
	public Texture diamondins;
	public Texture circleins;
	public Texture squareins;
	public Texture squareab;
	public Texture circleab;
	public Texture diamondab;
	public Texture skipToGame;




	// These are the readonly CD Functions
	public readonly float coolDownCircle = 0.4f;
	public readonly float coolDownTriangle = 0.6f;
	public readonly float coolDownSquare = 1.3f;

	//define character speed for every iteration blowup and slowdown
	public float charSpeed;
	public float bossSpeed;
	public bool inSlowDown = false;

    // Level number

    public int level = 0;


    //button locations
    float trayx = 0;
    float traywidth = 0;
    float trayspace = 0;

    // Sound stuff
    public AudioSource music;
    public AudioSource sfx;

    // Music clips
	public AudioClip bgm;
    private AudioClip menumusic;

    // Sound effect clips
	public AudioClip bossDead;
	public AudioClip playerHit;
	public AudioClip playerHitX;
	public AudioClip bossHit;
	public AudioClip bossHitX;
    public AudioClip abilityon;
    public AudioClip abilityoff;
	public AudioClip shootClip;

    // Use this for initialization
    void Start(){
		//score = 36000;
		Camera.main.backgroundColor = Color.black;
		music.GetComponent<AudioSource> ().clip = bgm;
		music.Play ();
	
		this.charSpeed = 2.9f;
		this.bossSpeed = 1.5f;
		this.bossCurrentLife = 1;
		this.bossTotalLives = 0;
		//currentboss = 1;
		// Set up the player order
		playerOrder = new int[playerLives];
		this.createPlayerOrderList ();


//		foreach (int x in playerOrder) {
//			print ("playerorder : " + x);
//		}

		//Randomise the player order




		//set up folder for enemies
		playerFolder = new GameObject ();
		playerFolder.name = "Players";
		players = new List<tPlayer> ();

		//set up a bullets folder to destroy all bullets during iterations





/*		addPlayer(playerOrder[playerOrderIndex], 1, -4, -4);
		playerOrderIndex++;

		currentplayer = players [0];

		print ("firstplayertype: " + currentplayer.playerType);
		if (currentplayer.playerType == 0) {
			//square
			currentplayer.setCD (this.coolDownSquare);
		} else if (currentplayer.playerType == 1) {
			//circle
			currentplayer.setCD (this.coolDownCircle);

		} else if (currentplayer.playerType == 2) {
			//triangle
			currentplayer.setCD (this.coolDownTriangle);
		}
		//setHealthText ();
		clock = 0f;
		shadow = new List<Vector3> ();
		shadowiterator = 0;
		startitr = false;
		this.createInitialEnv ();

		GameObject bossObject = new GameObject();
		Boss boss = bossObject.AddComponent<Boss>();
		boss.init (this, bossCurrentLife);
		THEBOSS = boss;
		StartCoroutine (iterationSlowdown (3));*/


		// setting up music
        SoundSetUp();

	}

	public void destroyForNextIteration(){
		foreach (tPlayer x in players) {
			x.transform.position = new Vector3(-100f, -100f, 0f);
		}
		foreach (tPlayer x in shadowPlayers) {
			x.transform.position = new Vector3(-100f, -100f, 0f);
		}
		foreach (tEnvVar x in envVariables) {
			x.model.transform.position = new Vector3(-100f, -100f, 0f);
			x.transform.position = new Vector3(-100f, -100f, 0f);
		}

	}

	//create next boss
	public void createNextBoss(){
		this.bossCurrentLife++;


		GameObject bossObject = new GameObject();
		tBoss boss = bossObject.AddComponent<tBoss>();
		boss.init (this, bossCurrentLife);
		//Destroy (THEBOSS.gameObject);
		THEBOSS = boss;
		playerOrderIndex = 0;
		foreach (tPlayer x in players) {
			Destroy (x.gameObject);
		}
		foreach (tPlayer x in shadowPlayers) {
			Destroy (x.gameObject);
		}
		foreach (tEnvVar x in envVariables) {
			Destroy (x.model.gameObject);
			Destroy (x.gameObject);
		}
		this.players.Clear ();
		this.shadowPlayers.Clear ();
		this.envVariables.Clear ();
		players = new List<tPlayer> ();
		shadowPlayers = new List<tPlayer> ();
		envVariables = new List<tEnvVar> ();
		//createInitialEnv ();
		playerOrderIndex = 0;
		addPlayer(playerOrder[playerOrderIndex], 1, -4, -4);
		playerOrderIndex++;
		currentplayer = players [0];
		if (currentplayer.playerType == 0) {
			//square
			currentplayer.setCD (this.coolDownSquare);
		} else if (currentplayer.playerType == 1) {
			//circle
			currentplayer.setCD (this.coolDownCircle);

		} else if (currentplayer.playerType == 2) {
			//triangle
			currentplayer.setCD (this.coolDownTriangle);
		}
	
	}


	public void createInitialEnv(){
		for (int i = 0; i < this.maxEnv; i++) {
			GameObject envObject = new GameObject();
			tEnvVar newenv = envObject.AddComponent<tEnvVar>();
			newenv.init (this);
			this.envVariables.Add (newenv);
		}
	
	}



	public void spawnNewEnv(){
		GameObject envObject = new GameObject();
		tEnvVar newenv = envObject.AddComponent<tEnvVar>();
		newenv.init (this);
		this.envVariables.Add (newenv);
	}

	IEnumerator startTransition (){
		this.inTransition = true;
		this.guiTransition = true;
//		if (this.THEBOSS.gameObject != null) {
//			Destroy (THEBOSS.gameObject);
//		}
//		bulletsFolder.Clear ();
//		this.createPlayerOrderList ();
//		this.destroyForNextIteration ();
		while (this.guiTransition) {
			yield return new WaitForSeconds (0.01f);
		}
		this.createNextBoss ();
		StartCoroutine (iterationSlowdown (3));
		this.inTransition = false;
	}

	IEnumerator startTransitionAni (){


		this.THEBOSS.initDead ();
		if (this.THEBOSS.gameObject != null) {
			Destroy (THEBOSS.gameObject);
		}
		bulletsFolder.Clear ();
		this.createPlayerOrderList ();
		this.destroyForNextIteration ();
		yield return new WaitForSeconds (5.03f);

		StartCoroutine (this.startTransition ());
	}





        
    // Update is called once per frame
    void Update()
    {
		if (this.gamewon == false && THEBOSS.bossHealth <= 0) {
			//this.gamewon = true;				
//			The commented bit below is for multiple levels and bosses - still working through the code for this ******* MOEEZ
			if (this.bossCurrentLife > this.bossTotalLives) {
				if (this.playerOrderIndex > 2) {
					this.gamewon = true;
				}
			} else {
				//got here
//				print("Defeated one boss");


				if (!this.inTransition) {
					foreach (GameObject x in this.bulletsFolder) {
						Destroy (x);
					}


//					bulletsFolder.Clear ();
//					this.createPlayerOrderList ();
					StartCoroutine (startTransitionAni ());
//					this.createNextBoss ();
//					StartCoroutine (iterationSlowdown (3));			
				}



				//************************************************
			}
		}
		clock += Time.deltaTime;
		//THEBOSS.updatePositions (currentplayer.transform.position.x, currentplayer.transform.position.y);
		currentplayer.model.shadowDirection.Add (currentplayer.direction);
		currentplayer.model.firingDirection.Add (firingDirection);
		Vector3 playerPosScreen = Camera.main.WorldToScreenPoint(currentplayer.transform.position);
		float speed = this.charSpeed;

		if (Input.GetKey (KeyCode.RightArrow)  && playerPosScreen.x < Screen.width -22 && currentplayer.getHealth() >0) {
			if (currentplayer.playerType != 2 || !currentplayer.usingability) {
				currentplayer.direction = 3;
				currentplayer.transform.eulerAngles = new Vector3 (0, 0, 3 * 90);
				if (Input.GetKey (KeyCode.UpArrow) && playerPosScreen.y < Screen.height -22) {
					currentplayer.transform.eulerAngles = new Vector3 (0, 0, 3 * 90 + 45);
					speed = this.charSpeed * (1/2);
					currentplayer.direction = 7;
				}
				if (Input.GetKey (KeyCode.DownArrow) && playerPosScreen.y > 22) {
					currentplayer.transform.eulerAngles = new Vector3 (0, 0, 3 * 90 - 45);
					speed = this.charSpeed * (1/2);
					currentplayer.direction = 6;
				}
				currentplayer.transform.Translate (Vector3.up * this.charSpeed * Time.deltaTime);
			}
		} 

		if (Input.GetKey (KeyCode.UpArrow) && playerPosScreen.y < Screen.height -22 && currentplayer.getHealth() >0 ) {
			if (currentplayer.playerType != 2 || !currentplayer.usingability) {
				currentplayer.direction = 0;
				currentplayer.transform.eulerAngles = new Vector3 (0, 0, 0 * 90);
				if (Input.GetKey (KeyCode.RightArrow) && playerPosScreen.x < Screen.width -22) {
					currentplayer.transform.eulerAngles = new Vector3 (0, 0, 0 * 90 - 45);
					speed = this.charSpeed * (1/2);
					currentplayer.direction = 7;
				}
				if (Input.GetKey (KeyCode.LeftArrow) && playerPosScreen.x > 22) {
					currentplayer.transform.eulerAngles = new Vector3 (0, 0, 0 * 90 + 45);
					speed = this.charSpeed * (1/2);
					currentplayer.direction = 4;
				}
				currentplayer.transform.Translate (Vector3.up * speed * Time.deltaTime);
			}
		}

		if (Input.GetKey (KeyCode.LeftArrow) && playerPosScreen.x > 22  && currentplayer.getHealth() >0){
			if (currentplayer.playerType != 2 || !currentplayer.usingability) {
				currentplayer.direction = 1;
				currentplayer.transform.eulerAngles = new Vector3 (0, 0, 1 * 90);
				if (Input.GetKey (KeyCode.UpArrow) && playerPosScreen.y < Screen.height -22) {
					currentplayer.transform.eulerAngles = new Vector3 (0, 0, 1 * 90 - 45);
					speed = this.charSpeed * (1/2);
					currentplayer.direction = 4;
				}
				if (Input.GetKey (KeyCode.DownArrow) && playerPosScreen.y > 22) {
					currentplayer.transform.eulerAngles = new Vector3 (0, 0, 1 * 90 + 45);
					speed = this.charSpeed * (1/2);
					currentplayer.direction = 5;
				}
				currentplayer.transform.Translate (Vector3.up * this.charSpeed * Time.deltaTime);
			}
		}

		if (Input.GetKey (KeyCode.DownArrow) && playerPosScreen.y > 22 && currentplayer.getHealth() >0) {
			if (currentplayer.playerType != 2 || !currentplayer.usingability) {
				currentplayer.direction = 2;
				currentplayer.transform.eulerAngles = new Vector3 (0, 0, 2 * 90);
				if (Input.GetKey (KeyCode.LeftArrow) && playerPosScreen.x > 22) {
					currentplayer.transform.eulerAngles = new Vector3 (0, 0, 2 * 90 - 45);
					speed = this.charSpeed * (1/2);
					currentplayer.direction = 5;

				}
				if (Input.GetKey (KeyCode.RightArrow) && playerPosScreen.x < Screen.width -22) {
					currentplayer.transform.eulerAngles = new Vector3 (0, 0, 2 * 90 + 45);
					speed = this.charSpeed * (1/2);
					currentplayer.direction = 6;
				}
				currentplayer.transform.Translate (Vector3.up * speed * Time.deltaTime);
			}
		}

		if (Input.GetKey (KeyCode.D)  && playerPosScreen.x < Screen.width -22 && currentplayer.getHealth() >0) {
			currentplayer.shoot (-90);
			firingDirection = -90;
		} 

		if (Input.GetKey (KeyCode.W) && playerPosScreen.y < Screen.height -22 && currentplayer.getHealth() >0) {
			currentplayer.shoot (0);
			firingDirection = 0;
		}

		if (Input.GetKey (KeyCode.A) && playerPosScreen.x > 22 && currentplayer.getHealth() >0){
			currentplayer.shoot (90);
			firingDirection = 90;
		}

		if (Input.GetKey (KeyCode.S) && playerPosScreen.y > 22 && currentplayer.getHealth() >0) {
			currentplayer.shoot (180);
			firingDirection = 180;
		}


		/*if (Input.GetKeyDown (KeyCode.Space)) {
			//The next line is just for testing texture 
			//this.THEBOSS.dealDamage (5);
			currentplayer.shoot();
		}*/
		if (Input.GetKeyDown (KeyCode.Space)) {
			currentplayer.useAbility ();

		}



    }


	public void whenPlayerDies(){
		//setHealthText ();
		if (currentplayer.getHealth () <= 0) {
			//recenter the boss
			Vector3 p = THEBOSS.transform.position;
			float q = p.z;
			THEBOSS.transform.position = new Vector3 (0, 0, q);
			THEBOSS.giveFullHealth ();
			//Start a corouting to slow down the time:

			foreach (GameObject x in this.bulletsFolder) {
				Destroy (x);
			}

			bulletsFolder.Clear ();
			//currentplayer.destroy();

			//this.THEBOSS.model.transform.position.y = 0;
			players.Remove(currentplayer);
			shadowPlayers.Add (currentplayer);
			//startitr = true;

			if ( shadowPlayers.Count <= this.playerLives) {
				//playertype++;
			} else if (shadowPlayers.Count >= this.playerLives) {
				this.gameOver();

			}


			addPlayer (playerOrder[playerOrderIndex], 1, -4, -4);
			playerOrderIndex++;
			currentplayer = players [0];
			StartCoroutine(iterationSlowdown(3) );

			if (currentplayer.playerType == 0) {
				//square
				currentplayer.setCD (this.coolDownSquare);
			} else if (currentplayer.playerType == 1) {
				//circle
				currentplayer.setCD (this.coolDownCircle);

			} else if (currentplayer.playerType == 2) {
				//triangle
				currentplayer.setCD (this.coolDownTriangle);
			}

		}
	}



	IEnumerator iterationSlowdown (int sec){
		this.inSlowDown = true;
		float pcharSpeed = this.charSpeed;
		float pbossSpeed = this.bossSpeed;
		this.charSpeed = 0.3f;
		this.bossSpeed = 0.3f;
		THEBOSS.setSpeeds();
		yield return new WaitForSeconds (sec);

		this.charSpeed = pcharSpeed;
		this.bossSpeed = pbossSpeed;
		THEBOSS.setSpeeds();
		this.inSlowDown = false;
	}






	public void addPlayer(int playerTypee, int initHealth, int x, int y)
	{
		System.Random rng = new System.Random ();
		float posx = rng.Next (-8, 8) * 1.0f;
		//float posy = rng.Next (-4, 4) * 1.0f;
		//print("sc vector:" + Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)));
		//print("screex" + Screen.width +", " + Screen.height + ", " + posx) ;
		GameObject playerObject = new GameObject();
		tPlayer player = playerObject.AddComponent<tPlayer>();
		player.transform.parent = playerFolder.transform;
		player.transform.position = new Vector3(posx, y, 0);
		player.init(playerTypee, this);
		players.Add(player);
		playernum++;
		player.name = "Player " + players.Count;
	}

	void setHealthText (){
		HealthText.text = "Health: "+currentplayer.getHealth();
	}

	public void gameOver(){
		foreach (tPlayer x in shadowPlayers) {
			Destroy (x.gameObject);
		}
		foreach (tPlayer x in this.players) {
			Destroy (x.gameObject);
		}
		Destroy (THEBOSS.gameObject);
		this.gameover = true;

	}

    private void SoundSetUp()
    {
        // music
/*        idle = Resources.Load<AudioClip>("Music/title song");
        gametrack = Resources.Load<AudioClip>("Music/Main song loop");
        winmusic = Resources.Load<AudioClip>("Music/You Win Song");*/

        // sfx
        bossDead = Resources.Load<AudioClip>("Music/explosion");
        playerHit = Resources.Load<AudioClip>("Music/shoot");
        playerHitX = Resources.Load<AudioClip>("Music/shootX");  //Special bullet when ability is on
  		bossHit = Resources.Load<AudioClip>("Music/bosshit");
        bossHitX = Resources.Load<AudioClip>("Music/bosshitX");
        abilityon = Resources.Load<AudioClip>("Music/abilityon");
        abilityoff = Resources.Load<AudioClip>("Music/abilityoff");    }
        // Music section
    public void PlayEffect(AudioClip clip)
    {
        sfx.clip = clip;
        sfx.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        this.music.loop = true;
        this.music.clip = clip;
        this.music.Play();
    }


	public void createPlayerOrderList() {

		// We use this to setup the list because it allows for pseudorandom ordering - which is that we wanted. 
		// It still maintains the same number of each kind of character
//		for (int i = 0; i < playerLives; i++) {
//			for (int j = 0; j < playerLives / 3; j++) {
//				this.playerOrder [i] = this.playertype;
//				if (j < playerLives / 3 - 1) {
//					i++;
//				}
//			}
//			this.playertype++;
//		}

		this.playerOrder [0] = 2;
		this.playerOrder [1] = 1;
		this.playerOrder [2] = 0;

		this.playertype = 0;
	//	this.shuffleYates (playerOrder);
//		for (int i = 0; i < playerLives; i++) {
//			this.playerOrder [i] = i % 3;
//		}
	}


	public void shuffleYates(int[] array)
	{
		int n = array.Length;
		for (int i = 0; i < n; i++)
		{
			System.Random rng = new System.Random ();
			// NextDouble returns a random number between 0 and 1.
			// ... It is equivalent to Math.random() in Java.
			int r = i + (int)(rng.NextDouble() * (n - i));
			int t = array[r];
			array[r] = array[i];
			array[i] = t;
		}
	}


	void OnGUI(){

			if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 105, 100, 60), skipToGame) ){
				SceneManager.LoadScene ("scene");
			}

			if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 165, 100, 60), quitText) ){
				Application.Quit();
			}
			
//		GUI.Box (new Rect (970, 28, 200, 33), nextUpText);
				    if (level == 0){ 
//            GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("label"));
//            myStyle.fontSize = 40;
			GUI.skin.box.alignment = TextAnchor.MiddleLeft;
			GUI.skin.box.fontSize = 25;


            GUI.Box (new Rect (Screen.width / 2 - 250, Screen.height / 2 - 270, 500, 400), titleText);
            GUI.color = Color.white;
            GUI.skin.box.fontSize = 12;
            GUI.skin.box.alignment = TextAnchor.MiddleCenter;

//            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2 + 50, 140, 60),tutorialText))
//            {
//                //level = 10;
//				SceneManager.LoadScene("main");
//				// Here, we can just load a differnet scene
//            }

   if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 2 + 130, 300, 100),startText)|| Input.GetKeyDown(KeyCode.Return))
            {
                level = 1;

        addPlayer(playerOrder[playerOrderIndex], 1, -4, -4);
		playerOrderIndex++;

		currentplayer = players [0];

		print ("firstplayertype: " + currentplayer.playerType);
		if (currentplayer.playerType == 0) {
			//square
			currentplayer.setCD (this.coolDownSquare);
		} else if (currentplayer.playerType == 1) {
			//circle
			currentplayer.setCD (this.coolDownCircle);

		} else if (currentplayer.playerType == 2) {
			//triangle
			currentplayer.setCD (this.coolDownTriangle);
		}
		//setHealthText ();
		clock = 0f;
		shadow = new List<Vector3> ();
		shadowiterator = 0;
		startitr = false;
		this.createInitialEnv ();

		GameObject bossObject = new GameObject();
		tBoss boss = bossObject.AddComponent<tBoss>();
		boss.init (this, bossCurrentLife);
		THEBOSS = boss;
		StartCoroutine (iterationSlowdown (3));



            }
//            if (GUI.Button(new Rect(25, Screen.height - 85, 110, 60), quitText)){
//                Application.Quit();
//            }

				if (GUI.Button(new Rect(25, Screen.height - 1555, 110, 60), skipToGame) ){
					SceneManager.LoadScene ("scene");
				}
        	} 

		if (level == 10){ //level selction

            for (int i = 1; i < 7; i++) {
                    GUIStyle BStyle = new GUIStyle(GUI.skin.GetStyle("Button"));
                    BStyle.fontSize = 25;
                    if (GUI.Button(new Rect(Screen.width / 8 + (i - 1) * Screen.width / 8, Screen.height / 2-Screen.width / 8/2 + Screen.width / 8 / 4/2, Screen.width / 8 - Screen.width / 8 / 4, Screen.width / 8 - Screen.width / 8 / 4), i.ToString(), BStyle))
                    {
//                        resetLevel();
                        level = i;
//                        makeLevel();
                    }
                    if (GUI.Button(new Rect(100, Screen.height-100, 200,80), "Back To Menu"))
                    {
//                        resetLevel();
                        level = 0;
//                        makeLevel();
                    }

        }
    }


        	if (level == 1){
		if (this.gameover) {


			GUI.skin.box.alignment = TextAnchor.MiddleLeft;
			GUI.skin.box.fontSize = 25;


					if (GUI.Button (new Rect (Screen.width / 2 - 300, Screen.height / 2 - 50, 600, 100), gameoverText)) {
						SceneManager.LoadScene("tutorial");	
					}
			GUI.color = Color.white;
			GUI.skin.box.fontSize = 12;
			GUI.skin.box.alignment = TextAnchor.MiddleCenter;
		
		}

		if(this.gamewon){
			

			foreach (tPlayer x in this.players) {
				Destroy (x.gameObject);
			}
			this.players.Clear ();

			foreach (tPlayer x in this.shadowPlayers) {
				Destroy (x.gameObject);
			}

			this.shadowPlayers.Clear ();

					//Destroy (this.THEBOSS.model1.gameObject);
					//Destroy (this.THEBOSS.gameObject);
					THEBOSS.transform.position = new Vector3(-200, -200, 0);

			GUI.skin.box.alignment = TextAnchor.MiddleLeft;
			GUI.skin.box.fontSize = 25;


					if (GUI.Button (new Rect (Screen.width / 2 - 300, Screen.height / 2 - 100, 600, 200), gamewonText)) {
						SceneManager.LoadScene("scene");	
					}
			
			GUI.color = Color.white;
			GUI.skin.box.fontSize = 12;
			GUI.skin.box.alignment = TextAnchor.MiddleCenter;



		}

		if (this.guiTransition) {
			if (GUI.Button (new Rect (Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 200), nextbossText)) {
				this.guiTransition = false;
			}
		
		}

// ****** TO reset the game - we will need to use a scene manager - Luxing take a look at this!**********
//		if(GUI.Button(new Rect((Screen.width- 100, Screen.height - 200, 500, 20), "Reset")) {
//			Application.
//		}
			/*GUI.Box(new Rect (1100, 28, 200, 33), scoreText);
			int s1 = score / 10000;
			int s2 = (score-10000*s1 )/ 1000;
			int s3 = (score-10000*s1-1000*s2 )/ 100;
			int s4 = (score-10000*s1-1000*s2 -100*s3) / 10;
			int s5 = score-10000*s1-1000*s2 -100*s3-10*s4;
			if (score >= 10000) {
				GUI.Box (new Rect (1050, 58, 40, 40), Resources.Load<Texture> ("Textures/number" + s1));
			}
			if (score >= 1000) {
				GUI.Box (new Rect (1080, 58, 40, 40), Resources.Load<Texture> ("Textures/number" + s2));
			}
			if (score >= 100) {
				GUI.Box (new Rect (1110, 58, 40, 40), Resources.Load<Texture> ("Textures/number" + s3));
			}
			if (score >= 10) {
				GUI.Box (new Rect (1140, 58, 40, 40), Resources.Load<Texture> ("Textures/number" + s4));
			}
			GUI.Box (new Rect (1170, 58, 40, 40), Resources.Load<Texture>("Textures/number"+s5));*/

			if (!this.gamewon && !this.gameover) {
				if (currentplayer.playerType == 0) {
					GUI.Box (new Rect (50, Screen.height-120, 300, 50), squareins);
					GUI.Box (new Rect (50, Screen.height-170, 300, 50), squareab);
					// ******JUN LI ******* Look here for tutorial instructions
						// The next one would be for the instruction for this tutorial
					// Use the square as a shield to protect others
					// Use shadows to kill the boss within 30 secs
					
					//GUI.Box (new Rect (50, Screen.height-170, 300, 50), squareab);
				} else if (currentplayer.playerType == 1) {
					GUI.Box (new Rect (50, Screen.height-120, 300, 50), circleins);
					GUI.Box (new Rect (50, Screen.height-170, 300, 50), circleab);
						// The next one would be for the instruction for this tutorial
						// Use special ability to kill the boss within 30 secs

						//GUI.Box (new Rect (50, Screen.height-170, 300, 50), squareab);
				} else {
					GUI.Box (new Rect (50, Screen.height-120, 300, 50), diamondins);
					GUI.Box (new Rect (50, Screen.height-170, 300, 50), diamondab);
						// The next one would be for the instruction for this tutorial
						// Use rapid fire ability to kill the boss within 30 secs

						//GUI.Box (new Rect (50, Screen.height-170, 300, 50), squareab);
				}

			}

			if (this.currentplayer.model.healthval > 3) {			
				GUI.color = Color.green;
			} else {
				GUI.color = Color.red;
			}
			GUI.skin.box.alignment = TextAnchor.MiddleLeft;
			GUI.skin.box.fontSize = 22;
			 String ss = "";

			for (int i = 0; i < this.currentplayer.model.healthval; i++) {

				ss += "I";

			}
				

			GUI.color = Color.white;
			GUI.skin.box.fontSize = 12;
			GUI.skin.box.alignment = TextAnchor.MiddleCenter;
		

		if (this.playerOrderIndex < playerLives) {
			GUI.skin.box.alignment = TextAnchor.LowerCenter;
			GUI.skin.box.fontSize = 22;
			GUI.Box (new Rect (900, 25, 110, 34), nextUpText);

			int nextType = playerOrder [playerOrderIndex];
			if (nextType == 0) {
				GUI.Box (new Rect (925, 60, 50, 50), this.forSq);
			} else if (nextType == 1) {
				GUI.Box (new Rect (925, 60, 50, 50), this.forC);
			} else if (nextType == 2) {
				GUI.Box (new Rect (925, 60, 50, 50), this.forT);
			}
		}else {
				//GUI.color = Color.red;
				GUI.skin.box.fontSize = 22;
			GUI.Box(new Rect (900, 19, 100, 40), lastLifeText);


		
		}
		GUI.skin.box.fontSize = 12;

}

	}

	public tPlayer GetTarget(){
		return currentplayer;
	}

	public float GetTargetX(){
		return currentplayer.getX();
	}

	public float GetTargetY(){
		return currentplayer.getY();
	}

	public Texture getCooldownText(){
		return cooldownText;
	}

	public Texture getSpecialText(){
		return specialText;
	}

	public Texture getAbilityOnText(){
		return abilityOnText;
	}



	}}