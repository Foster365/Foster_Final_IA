using UADE.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyGameExample.UI.Screens
{
    class SplashScreen: AbstractScreen
    {
        public void Next()
        {
            StackNavigator.Instance.Push<MainScreen>();
        }

        private void Update() 
        {
            if(Input.GetButton("Skip")) 
            {
                this.Next();
            }
        }

    }


}
