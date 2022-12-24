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
        [SerializeField] private SFXmanager SFXmanager;
        [SerializeField] private Animator flamethrowerAnimator;
        [SerializeField] private float stepSfxTimerMax = 0.5f;
        [SerializeField] private SpriteRenderer[] ammoSlots;
        [SerializeField] private Animator pickupBar;
        [SerializeField] private Animator topAnimator;
        private Heart[] hearts;
        
        [Header("Body parts")]
        [SerializeField] private GameObject bodySide;
        [SerializeField] private GameObject bodyFront;
        [SerializeField] private GameObject bodyBack;
        [SerializeField] private Transform sideTrans;
        [SerializeField] private Transform flameTrans;
        [SerializeField] private Animator frontAnimator;
        [SerializeField] private Animator backAnimator;
        [SerializeField] private Animator sideAnimator;

        private Logic.Player lastPlayerState;
        private LevelLogicManager logicManager;
        private float stepSfxTimer = 0f;
        private bool isWalking = false;
        private float flamethrowerLocalDistance;

        public void Setup(Heart[] hearts, Direction initialDirection)
        {
            this.hearts = hearts;

            foreach (var heart in hearts)
            {
                heart.ResetHeart();
            }

            updateDirection(initialDirection);
            UpdateAmmo(0);
        }

        private void Awake()
        {
            SFXmanager = GameObject.FindWithTag("SFX").GetComponent<SFXmanager>();
            //playerSprite = GetComponent<SpriteRenderer>();
            playerAnimator = GetComponent<Animator>();


            flamethrowerLocalDistance = flameTrans.localPosition.magnitude;
        }

        private void Update()
        {
            if (isWalking)
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

        private void UpdateAmmo(int ammoCount)
        {
            for(var i=0; i < ammoSlots.Length; i++)
            {
                ammoSlots[i].enabled = i < ammoCount;
            }
        }
        private void updateDirection(Direction dir)
        {
            //reset rotation
            sideTrans.localScale = new Vector3(1,1,1);

            switch(dir)
            {
                case Direction.Up:
                    bodyBack.SetActive(true);
                    bodyFront.SetActive(false);
                    bodySide.SetActive(false);

                    flamethrowerAnimator.SetBool("IsHorizontal",false);
                    flameTrans.localPosition = Vector3.up * flamethrowerLocalDistance;
                    flameTrans.localRotation = Quaternion.identity;
                break;
                //////////
                case Direction.Down:
                    bodyBack.SetActive(false);
                    bodyFront.SetActive(true);
                    bodySide.SetActive(false);

                    flamethrowerAnimator.SetBool("IsHorizontal",false);
                    flameTrans.localPosition = Quaternion.Euler(0, 0, 180) * Vector3.up * flamethrowerLocalDistance;
                    flameTrans.localRotation = Quaternion.Euler(0, 0, 180);
                break;
                /////////
                case Direction.Right:
                    bodyBack.SetActive(false);
                    bodyFront.SetActive(false);
                    bodySide.SetActive(true);

                    flamethrowerAnimator.SetBool("IsHorizontal",true);
                    flameTrans.localPosition = Vector3.right * flamethrowerLocalDistance;
                    flameTrans.localRotation = Quaternion.identity;
                break;
                /////////
                case Direction.Left:
                    bodyBack.SetActive(false);
                    bodyFront.SetActive(false);
                    bodySide.SetActive(true);

                    flamethrowerAnimator.SetBool("IsHorizontal",true); 
                    
                    flameTrans.localPosition = Quaternion.Euler(0, 0, 180) * Vector3.right * flamethrowerLocalDistance;
                    flameTrans.localRotation = Quaternion.Euler(0, 0, 180);
                    sideTrans.localScale = new Vector3(-1,1,1);
                break;
            }
            
        }


        public void UpdatePlayer(Logic.Player previousPlayerState,
            Logic.Player currentPlayerState, LevelSettings levelSettings, Logic.Grid grid,
             LevelLogicManager manager)
        {
            logicManager = manager;
            
            //direction
            if(currentPlayerState.orientation != previousPlayerState.orientation)
            {
                updateDirection(currentPlayerState.orientation);
            }

            //weed
            if(manager.WeedPickupProgression(currentPlayerState, grid) > 0 &&
               manager.WeedPickupProgression(previousPlayerState, grid) < float.Epsilon)
            {
                pickupBar.SetTrigger("Start");
            }

            //walking or not
            if (manager.IsPlayerMoving(currentPlayerState) != manager.IsPlayerMoving(previousPlayerState))
            {
                var val = manager.IsPlayerMoving(currentPlayerState);
                frontAnimator.SetBool("IsWalking", val);
                backAnimator.SetBool("IsWalking", val);
                sideAnimator.SetBool("IsWalking", val);
                
                if(manager.IsPlayerMoving(currentPlayerState))
                {
                    pickupBar.SetTrigger("Stop");
                }
            }
            
            //trigger dotween animation
            if(currentPlayerState.position != previousPlayerState.position)
            {
                Vector2Int dest = currentPlayerState.position;
                var boardCenter = BoardCenter(levelSettings);
                var newPosition = new Vector3(dest.x - boardCenter.x,
                    dest.y - boardCenter.y, transform.position.z);

                transform.DOComplete();
                transform.DOMove(newPosition, levelSettings.PlayerTileMoveTime);
            }

            //hurt
            if (currentPlayerState.HP < previousPlayerState.HP)
            {
                if (currentPlayerState.HP > 0)
                {
                    frontAnimator.SetTrigger("hurt");
                    backAnimator.SetTrigger("hurt");
                    sideAnimator.SetTrigger("hurt");
                    SFXmanager.playHurt();

                    hearts[previousPlayerState.HP - 1].LoseHeart();
                }
                else
                {
                    hearts[0].LoseHeart();
                    bodySide.SetActive(false);
                    bodyBack.SetActive(false);
                    bodyFront.SetActive(false);
                    topAnimator.SetTrigger("Die");
                    
                    //workaround to not being centered right
                    transform.DOComplete();
                    transform.DOMove(transform.position + new Vector3(0f,1f,0f),0f);
                    SFXmanager.playDie();
                }
            }

            //shoot
            if(currentPlayerState.TimeSinceLastAttack < previousPlayerState.TimeSinceLastAttack)
            {
                SFXmanager.playShoot();
                flamethrowerAnimator.SetTrigger("Burn");
            }

            //ammo
            if(currentPlayerState.Ammo != previousPlayerState.Ammo)
            {
                UpdateAmmo(currentPlayerState.Ammo);
            }
            
        }

        private Vector2 BoardCenter(LevelSettings settings)
        {
            return new Vector2(settings.GridWidth / 2.0f,
                settings.GridHeight / 2.0f);
        }

    }
}