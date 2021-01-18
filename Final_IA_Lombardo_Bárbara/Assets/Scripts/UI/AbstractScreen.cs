using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace UADE.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class AbstractScreen : MonoBehaviour, IScreen
	{

		public GameObject GetGameObject()
		{ 
			return gameObject;
		}

		protected virtual void Awake()
		{
			
		}

		public virtual void Open()
		{
			
		}

		public virtual void Close()
		{
			
		}


	}
}
