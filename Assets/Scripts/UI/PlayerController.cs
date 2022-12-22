using UnityEngine;
using System.Collections;
using System;
using Logic;

namespace UI
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private SpriteRenderer playerSpriteRenderer;
        [SerializeField] private Animator flamethrowerAnimator;
        [SerializeField] private float IframesFlickerLength = 0.2f;

        private Logic.Player lastPlayerState;
        private LevelLogicManager logicManager;
        private bool isInIFrame;
        private float IframesFlickerTimer = 0f;

        public void IFrame(bool isInIFrame)
        {
            this.isInIFrame = isInIFrame;
        }

        public void Update()
        {
            
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
            
            
            //hurt
            if(currentPlayerState.HP < previousPlayerState.HP)
            {
                if(currentPlayerState.HP > 0)
                    playerAnimator.SetTrigger("hurt");
                else
                    playerAnimator.SetTrigger("die");
            }

            if(logicManager.IsPlayerInvincible(currentPlayerState))
            {
                Color col = playerSpriteRenderer.color;
                //playerSpriteRenderer.color = new col
            }

        }
    }
}