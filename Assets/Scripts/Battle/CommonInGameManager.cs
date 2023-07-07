using Assets.Scripts.UI.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class CommonInGameManager : MonoBehaviour
    {

        private void Start()
        {
            UIInGameManager.Instance.Init(() =>
            {
                Debug.Log("라운드 끝남");
            });
        }
    }
}
