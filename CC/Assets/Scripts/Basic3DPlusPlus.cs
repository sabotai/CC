using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basic3DPlusPlus : MonoBehaviour {

	public GameObject mover;
	public int movementAmt = 1;
	public Vector3 startingPosition;
	public GameObject winSpot;
	public GameObject[] enemies;
	public GameObject bg;
	public float enemySpeed = 0.1f; //must be a multiple of 1.0
	public float enemyDifficulty = 0.01f; //must add up to above to always be a multiple of 1.0
	public float hitThresh = 0.2f;
	public AudioClip impactClip;
	public AudioClip moveClip;
	public AudioClip lvlUpClip;
	public AudioClip outOfBoundsClip;
	public AudioClip resetSound;
    AudioSource audioSource;
    bool moveInProgress = false;
    [SerializeField] Vector3 moveDestPos;
    float movePct = 0f;
    float lerpSpeed = 0.2f;
    bool disabled = false;
    int movesAllowed;
    int movesElapsed;
    int currentLevel;
    public Text playerScore;
    public Text playerLevel;
    public GameObject hiddenResetInstructions;
 
	// Use this for initialization
	void Start () {
		//assign the initial starting position to wherever mover is when the game starts
		

        audioSource = mover.GetComponent<AudioSource>();
        moveDestPos = startingPosition;
	    movesAllowed = 100;
	    movesElapsed = 0;
	    currentLevel = 1;
        audioSource.PlayOneShot(resetSound, 0.7F);
	}

	// Update is called once per frame
	void Update () {
		int score = movesAllowed - movesElapsed;
		playerScore.text = score + " ";

		if (movesElapsed > movesAllowed){
			hiddenResetInstructions.SetActive(true);
			playerLevel.text = "";
			StartCoroutine(ScreenShake.Shake(0.5f, 0.5f));
					moveSound(impactClip);
					moveSound(impactClip);
					moveSound(impactClip);
					if (Input.GetKeyDown("space")){
						moveDestPos = startingPosition;

						hiddenResetInstructions.SetActive(false);
						Start();
					}
		} else {
			playerLevel.text = currentLevel + " ";

		}
		if (!disabled && Input.anyKey){
				movesElapsed++;
				if (Input.GetKeyDown(KeyCode.A)) {
				//	Debug.Log ("left arrow pressed down");
					moveDestPos += new Vector3(-movementAmt,0,0);
					moveSound(moveClip);
				} 
				if (Input.GetKeyDown(KeyCode.D)) {
					//Debug.Log ("right arrow pressed down");
					moveDestPos += new Vector3(movementAmt,0,0);
					moveSound(moveClip);
				} 
				if (Input.GetKeyDown(KeyCode.W)) {
					//Debug.Log ("up arrow pressed down");
					moveDestPos += new Vector3(0,0,movementAmt);
					moveSound(moveClip);
				} 
				if (Input.GetKeyDown(KeyCode.S)) {
					//Debug.Log ("down arrow pressed down");
					moveDestPos += new Vector3(0,0,-movementAmt);
					moveSound(moveClip);
				}
		}
		radiate(winSpot);
		checkMover();
		doEnemies();
		moveMover(lerpSpeed);
	}
	void doEnemies(){

		for (int i = 0; i < enemies.Length; i++) {
			if (Vector3.Distance(mover.transform.position, enemies[i].transform.position) < hitThresh) { //is mover in same position as enemy?
				//if so, reset the movers position
				if (!disabled){
					moveSound(impactClip);
					lerpSpeed = 0.3f;
					movesElapsed += currentLevel;
					StartCoroutine(ScreenShake.Shake(0.25f, 0.5f));
					resetPos();
				}
			}

			if (enemies[i].transform.position.x > -2) {
				enemies[i].transform.position += new Vector3 (-enemySpeed, 0, 0);
			} else {
				enemies[i].transform.position = new Vector3 (3, enemies[i].transform.position.y, enemies[i].transform.position.z);
			}
		}
	}

	void checkMover(){


		//check if mover's transform.position.* is beyond each threshold
		if (mover.transform.position.z < 0 ||  //is it behind the grid?
			mover.transform.position.z > 6 || //is it too far off the grid?
			mover.transform.position.x < -2 || //is it too far left of the grid?
			mover.transform.position.x > 3) { //is it too far right of the grid?

       			audioSource.PlayOneShot(outOfBoundsClip, 0.7F);
       			lerpSpeed = 0.01f;

				StartCoroutine(ScreenShake.Shake(0.4f, 0.02f));
				resetPos(); //if any of those are true... reset it's position to the starting position

		}

		//check if mover's transform.position has the same...
		if (mover.transform.position == 
			new Vector3 (winSpot.transform.position.x, //...x as winSpot
				mover.transform.position.y, //...y as itself, because the winSpot is below it and we don't care about the y
				winSpot.transform.position.z)){ //z as winSpot
			//if so...
			//Debug.Log ("win?"); //give us a console message

			//mover.GetComponent<MeshRenderer> ().material.color = Color.red; //access the color through 
			//mover.transform.localScale *= 1.01f;
			if (!disabled)		newLevel ();
		}

	}

	void resetPos(){
		moveDestPos = startingPosition;
		//lerpSpeed = 0.01f;
		//mover.transform.position = startingPosition;

	}

	void moveMover(float moveSpd){
		float thresh = 0.1f;

		float dist = Vector3.Distance(moveDestPos, mover.transform.position);
		if ((movePct > 1f) || (dist <= thresh)){
			//moveDestPos = new Vector3((int)moveDestPos.x, (int)moveDestPos.y,(int)moveDestPos.z);
			mover.transform.position = moveDestPos;
			//moveInProgress = false;
			//Debug.Log("approximating finished move at " + movePct + "% && dist " + dist);
			movePct = 0f;
			lerpSpeed = 0.2f;
			disabled = false;
		} else {
			disabled = true;
			//moveInProgress = true;
			mover.transform.position = Vector3.Lerp(mover.transform.position, moveDestPos, movePct);
			movePct += moveSpd;
			//Debug.Log("attempting move at " + movePct+ "% && dist " + dist);
		}
	}

	void moveSound(AudioClip aud){

        audioSource.PlayOneShot(aud, 0.7F);

	}

	void newLevel(){
		currentLevel++;
		Debug.Log("Entering Level " + currentLevel);
		movesElapsed -= currentLevel * 5;
        audioSource.PlayOneShot(lvlUpClip, 0.7F);
		lerpSpeed = 0.01f;
		resetPos(); 
		mover.GetComponent<MeshRenderer> ().material.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f), 1F);
		bg.GetComponent<MeshRenderer> ().material.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f), 1F);
		enemySpeed += enemyDifficulty;

		for (int i = 0; i < enemies.Length; i++) {
			enemies [i].transform.position = new Vector3 (Random.Range (-2, 3), enemies [i].transform.position.y, Random.Range (1, 6));
		}
	}

	void radiate(GameObject radObj){
		Color radColor = radObj.GetComponent<MeshRenderer> ().material.color;
		float rSpeed = 0.3f;
		float gSpeed = 0.9f;
		float bSpeed = 0.2f;
		radColor.r = (Mathf.Sin(rSpeed * Time.time) + 1f) / 2f;
		radColor.g = (Mathf.Sin(gSpeed * Time.time) + 1f) / 2f;
		radColor.b = (Mathf.Sin(bSpeed * Time.time) + 1f) / 2f;
		radObj.GetComponent<MeshRenderer> ().material.color = radColor;
	}
}
