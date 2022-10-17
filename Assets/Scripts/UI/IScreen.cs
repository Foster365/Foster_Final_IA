using UnityEngine;
using System.Collections;


namespace UADE.UI
{
	public interface IScreen
    {
		void Open();
		void Close();

		GameObject GetGameObject();
	}
}