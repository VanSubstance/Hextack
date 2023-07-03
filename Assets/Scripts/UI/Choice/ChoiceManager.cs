using Assets.Scripts.Unit;
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
            foreach (ChoiceController choice in choices)
            {
                UnitInfo cur;
                while ((cur = GlobalStatus.Deck[Random.Range(0, GlobalStatus.Deck.Length)]) == null)
                {
                }
                choice.Init(cur);
            }
            gameObject.SetActive(true);
        }
    }
}
