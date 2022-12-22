using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace UI
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            DOTween.Init();
        }
    }
}