using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;

namespace UADE.UI
{
	public class StackNavigator : MonoBehaviour
	{
		public EventSystem EventSystem;

		private static StackNavigator _instance = null;
		private Stack<IScreen> m_ActiveScreens = new Stack<IScreen>();
		[SerializeField]
		public IScreen[] m_AvailableScreens;



		public static StackNavigator Instance
		{
			get
			{
				return _instance;
			}
		}

		public StackNavigator()
        {
			

        }


		private void Awake()
		{
			if (_instance)
			{
				Destroy(_instance.gameObject);
			}

			_instance = this;
			Initialize();
		}


		public void Initialize()
		{
			m_AvailableScreens = GetComponentsInChildren<IScreen>(true);
			foreach (IScreen screen in m_AvailableScreens)
			{
				screen.GetGameObject().SetActive(false);
			}
		}


		public T Push<T>() where T : IScreen
		{
			T screen = default(T);

			for (int i = 0; i < m_AvailableScreens.Length; i++)
			{
				if (m_AvailableScreens[i].GetType() == typeof(T))
				{
					screen = (T)m_AvailableScreens[i];
					if (!m_ActiveScreens.Contains(screen))
					{
						if(m_ActiveScreens.Count > 0)
						{
							IScreen currentScreen = m_ActiveScreens.Peek();

							if (currentScreen != null)
							{
								currentScreen.Close();
								currentScreen.GetGameObject().SetActive(false);
							}
						}

						m_ActiveScreens.Push(screen);
						screen.GetGameObject().SetActive(true);
						screen.Open();
					}
					else
					{
						Debug.LogWarning("ScreenManager.Push(): The screen is already in the stack: " + screen);
					}
					break;
				}
			}
			return screen;
		}


		public IScreen Pop()
		{
			if (m_ActiveScreens.Count == 0)
			{
				return null;
			}

			IScreen screen = m_ActiveScreens.Pop();

			if (screen != null)
			{
				screen.Close();
				screen.GetGameObject().SetActive(false);
			}

			if (m_ActiveScreens.Count > 0)
			{
				IScreen currentScreen = m_ActiveScreens.Peek();

				if (currentScreen != null)
				{
					currentScreen.Open();
					currentScreen.GetGameObject().SetActive(true);
				}
			}

			return screen;
		}


		public IScreen CurrentScreen
		{
			get
			{
				return m_ActiveScreens.Count > 0 ? m_ActiveScreens.Peek() : null;
			}
		}


		public T Get<T>() where T : IScreen
		{
			T screen = default(T);
			for (int i = 0; i < m_AvailableScreens.Length; i++)
			{
				if (m_AvailableScreens[i].GetType() == typeof(T))
				{
					screen = (T)m_AvailableScreens[i];
					break;
				}
			}
			return screen;
		}


		private void Update()
		{
			
		}
	}
}
