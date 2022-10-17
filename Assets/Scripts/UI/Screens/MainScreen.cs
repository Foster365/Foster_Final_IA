using UADE.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyGameExample.UI.Screens
{
    class MainScreen: AbstractScreen
    {

        public void Back()
        {
            StackNavigator.Instance.Pop();
        }
        
        
        public void Play()
        {
            StackNavigator.Instance.Push<HUD>();
        }
        private void Update() 
        {
            if(Input.GetButton("Cancel")) 
            {
                this.Back();
            }
        }
    }

}
