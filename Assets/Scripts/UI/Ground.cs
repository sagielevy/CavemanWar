using UnityEngine;
using System.Collections;
using System;

namespace UI
{
	public class Ground : MonoBehaviour
	{
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer fireRenderer;

        public void Start()
        {
            fireRenderer.enabled = false;
        }

        public void Burn()
        {
            throw new NotImplementedException();
        }
    }
}