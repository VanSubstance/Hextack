using Assets.Scripts.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Choice
{
    public class ChoiceManager : MonoBehaviour
    {
        [SerializeField]
        private ChoiceController choicePrefab;
        private ChoiceController[] choices;

        private void Awake()
        {
            gameObject.SetActive(false);
            choices = new ChoiceController[2];
            for (int i = 0; i < 2; i++)
            {
                choices[i] = Instantiate(choicePrefab, transform);
            }
        }

        /// <summary>
        /// 덱에서 두개 뽑아서 띄워주기 함수
        /// </summary>
        public void PickRandom()
        {
            HashSet<int> idxPicked = new HashSet<int>();
            int idx;
            foreach (ChoiceController choice in choices)
            {
                UnitInfo cur;
                while (true)
                {
                    idx = Random.Range(0, ServerData.User.Deck.Length);
                    if (!idxPicked.Contains(idx))
                    {
                        idxPicked.Add(idx);
                        // 신규 인덱스
                        if ((cur = ServerData.User.Deck[idx]) != null)
                        {
                            // 벤 안됨
                            choice.Init(cur);
                            break;
                        }
                    }
                }
            }
            gameObject.SetActive(true);
        }
    }
}
