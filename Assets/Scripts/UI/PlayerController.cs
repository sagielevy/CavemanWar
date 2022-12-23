using UnityEngine;
using System.Collections.Generic;
using System;
using Logic;
using DG.Tweening;

namespace UI
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        //[SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private SFXmanager SFXmanager;
        [SerializeField] private Animator flamethrowerAnimator;
        [SerializeField] private float stepSfxTimerMax = 0.5f;
        [SerializeField] private float walkingSpeed = 0.5f;
        
        private SpriteRenderer[] hearts;

        private Logic.Player lastPlayerState;
        private LevelLogicManager logicManager;
        private bool isInIFrame;
        private float stepSfxTimer = 0f;
        private bool isWalking = false;

        public void IFrame(bool isInIFrame)
        {
            this.isInIFrame = isInIFrame;
        }

        public void Setup(SpriteRenderer[] hearts)
        {
            this.hearts = hearts;
        }

        public void Start()
        {
            SFXmanager = GameObject.FindWithTag("SFX").GetComponent<SFXmanager>();
            //playerSprite = GetComponent<SpriteRenderer>();
            playerAnimator = GetComponent<Animator>();

            playerAnimator.SetBool("IsWalking",false);
            playerAnimator.SetFloat("direction",0);
        }

        public void Update()
        {
            if(isWalking)
            {
                stepSfxTimer -= Time.deltaTime;
                
                //play sfx
                if(stepSfxTimer <= 0f)
                {
                    stepSfxTimer = stepSfxTimerMax;
                    SFXmanager.playStep();
                }
            }
        }

        public void UpdatePlayer(Logic.Player previousPlayerState,
            Logic.Player currentPlayerState, LevelSettings levelSettings, LevelLogicManager manager)
        {
            logicManager = manager;
            Debug.Log(isWalking);
            
            //direction
            if(currentPlayerState.orientation != previousPlayerState.orientation)
                playerAnimator.SetFloat("direction",(int)currentPlayerState.orientation);

            //walking or not
            if(manager.IsPlayerMoving(currentPlayerState) != manager.IsPlayerMoving(previousPlayerState))
                playerAnimator.SetBool("IsWalking",true);//manager.IsPlayerMoving(currentPlayerState));
            isWalking = manager.IsPlayerMoving(currentPlayerState);
            
            //trigger dotween animation
            if(currentPlayerState.position != previousPlayerState.position)
            {
                Vector2Int dest = currentPlayerState.position;
                transform.DOMove(new Vector3(dest.x, dest.y, 0f),walkingSpeed);
            }

            //flipx when facing left
            //playerSprite.flipX = currentPlayerState.orientation == Direction.Left;

            //hurt
            if(currentPlayerState.HP < previousPlayerState.HP)
            {
                if(currentPlayerState.HP > 0)
                {
                    playerAnimator.SetTrigger("hurt");
                    SFXmanager.playHurt();
                }
                else
                {
                    playerAnimator.SetTrigger("die");
                    SFXmanager.playDie();
                }

                //update hearts
                for(var i=levelSettings.InitialHP; i > currentPlayerState.HP; i--)
                {
                    hearts[i].enabled = false;
                } 
            }

            //shoot
            if(currentPlayerState.TimeSinceLastAttack < previousPlayerState.TimeSinceLastAttack)
            {
                SFXmanager.playShoot();
                flamethrowerAnimator.SetTrigger("Burn");
            }

        }
    }
}