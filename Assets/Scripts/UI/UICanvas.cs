using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] RectTransform mainMenuTrans;
    [SerializeField] RectTransform gameOverTrans;
    [SerializeField] TMPro.TextMeshProUGUI playerWon;
    [SerializeField] float lerpSpeed;

    public bool isGameStarted = false;
    public bool isGameOver = false;
    private BoxCollider2D collider;
    public float mainDest = 0f;    //ydest for the main menu canvas to lerp to
    public float overDest = 0f;    //ydest for the game over canvas to lerp to
    public float screenH = 500f;

    // Start is called before the first frame update
    void Start()
    {
        //find screenH
        collider = GetComponent<BoxCollider2D>();
        screenH = collider.size.y * 1f;

        //push the game over screen up
        overDest = screenH * 1.5f;
        gameOverTrans.position = new Vector3(0f,screenH * 1.5f,0f);

        mainDest = screenH * 0.5f;
        mainMenuTrans.position = new Vector3(0f,screenH * 0.5f, 0f);

        
    }

    //call this to push the main menu away
    public void startGame()
    {
        mainDest = -screenH;
        isGameStarted = true;
    }

    public void endGame(int winningPlayerIndex)
    {
        isGameOver = false;

        //update pos dests
        overDest = 0.5f * screenH;

        //update text
        playerWon.text = "player " + winningPlayerIndex.ToString() + " won";
    }
    
    // Update is called once per frame
    void Update()
    {
        //input
        if(!isGameStarted && Input.GetKeyDown("space"))
            startGame();

        //lerp canvases to positions
        mainMenuTrans.position = new Vector3(0,Mathf.Lerp(mainMenuTrans.position.y,mainDest,lerpSpeed * Time.deltaTime),0f);
        gameOverTrans.position = new Vector3(0,Mathf.Lerp(gameOverTrans.position.y,overDest,lerpSpeed * Time.deltaTime),0f);
        //approach canvases to positions
        mainMenuTrans.position = new Vector3(0,Mathf.MoveTowards(mainMenuTrans.position.y,mainDest,lerpSpeed * Time.deltaTime * 10f),0f);
        gameOverTrans.position = new Vector3(0,Mathf.MoveTowards(gameOverTrans.position.y,overDest,lerpSpeed * Time.deltaTime * 10f),0f);
        
    }
}