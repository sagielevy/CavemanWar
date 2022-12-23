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
        private Heart[] hearts;
        
        [Header("Body parts")]
        [SerializeField] private GameObject bodySide;
        [SerializeField] private GameObject bodyFront;
        [SerializeField] private GameObject bodyBack;
        [SerializeField] private Transform sideTrans;
        [SerializeField] private Transform flameTrans;
        //[SerializeField] private SpriteRenderer frontSprite;
        //[SerializeField] private SpriteRenderer backSprite;

        private Logic.Player lastPlayerState;
        private LevelLogicManager logicManager;
        private bool isInIFrame;
        private float stepSfxTimer = 0f;
        private bool isWalking = false;
        private float flamethrowerLocalDistance;

        public void IFrame(bool isInIFrame)
        {
            this.isInIFrame = isInIFrame;
        }

        public void Setup(Heart[] hearts)
        {
            this.hearts = hearts;
        }

        public void Start()
        {
            SFXmanager = GameObject.FindWithTag("SFX").GetComponent<SFXmanager>();
            //playerSprite = GetComponent<SpriteRenderer>();
            playerAnimator = GetComponent<Animator>();

            playerAnimator.SetBool("IsWalking",false);

            flamethrowerLocalDistance = flameTrans.localPosition.magnitude;

            updateAmmo(0);
            updateDirection(Direction.Down);
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

        private void updateAmmo(int ammoCount)
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
            Logic.Player currentPlayerState, LevelSettings levelSettings, LevelLogicManager manager)
        {
            logicManager = manager;
            //Debug.Log(isWalking);
            
            //direction
            if(currentPlayerState.orientation != previousPlayerState.orientation)
            {
                updateDirection(currentPlayerState.orientation);
            }

            //walking or not
            playerAnimator.SetBool("IsWalking",true);
            if (manager.IsPlayerMoving(currentPlayerState) != manager.IsPlayerMoving(previousPlayerState))
            {
                playerAnimator.SetBool("IsWalking", manager.IsPlayerMoving(currentPlayerState));
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
                    playerAnimator.SetTrigger("hurt");
                    SFXmanager.playHurt();

                    //update hearts
                    for(var i = levelSettings.InitialHP - 1; i >= currentPlayerState.HP; i--)
                    {
                        hearts[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    hearts[0].gameObject.SetActive(false);
                    playerAnimator.SetTrigger("die");
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
                updateAmmo(currentPlayerState.Ammo);
            }

        }

        private Vector2 BoardCenter(LevelSettings settings)
        {
            return new Vector2(settings.GridWidth / 2.0f,
                settings.GridHeight / 2.0f);
        }
    }
}