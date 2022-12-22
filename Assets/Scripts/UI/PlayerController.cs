using UnityEngine;
using System.Collections.Generic;
using System;
using Logic;

namespace UI
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private SFXmanager SFXmanager;
        [SerializeField] private Animator flamethrowerAnimator;
        [SerializeField] private float stepSfxTimerMax = 0.5f;
        [SerializeField] private List<SpriteRenderer> hearts = new List<SpriteRenderer>();

        private Logic.Player lastPlayerState;
        private LevelLogicManager logicManager;
        private bool isInIFrame;
        private float stepSfxTimer = 0f;
        private bool isWalking = false;

        public void IFrame(bool isInIFrame)
        {
            this.isInIFrame = isInIFrame;
        }

        public void Start()
        {
            playerSprite = GetComponent<SpriteRenderer>();
            playerAnimator = GetComponent<Animator>();
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

            /// send data to the animation controller ///

            //direction
            if(currentPlayerState.orientation != previousPlayerState.orientation)
                playerAnimator.SetFloat("direction",(int)currentPlayerState.orientation);

            //walking or not
            if(manager.IsPlayerMoving(currentPlayerState) != manager.IsPlayerMoving(previousPlayerState))
                playerAnimator.SetBool("isWalking",manager.IsPlayerMoving(currentPlayerState));
            isWalking = manager.IsPlayerMoving(currentPlayerState);
            
            //flipx when facing left
            playerSprite.flipX = currentPlayerState.orientation == Direction.Left;

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