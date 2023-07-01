using Assets.Scripts.Unit;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class TestManager : MonoBehaviour
    {
        [SerializeField]
        private UnitInfo[] testDeck;

        private void Awake()
        {
            GlobalStatus.deck = new UnitInfo[testDeck.Length];
            for (int i = 0; i < testDeck.Length; i++)
            {
                if (testDeck[i] == null) continue;
                GlobalStatus.deck[i] = Instantiate(testDeck[i]);
            }
        }
    }
}
