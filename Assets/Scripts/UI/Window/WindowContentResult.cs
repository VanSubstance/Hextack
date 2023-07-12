using UnityEngine;

namespace Assets.Scripts.UI.Window
{
    public class WindowContentResult : AbsWindowContent<ResultInfo>
    {
        public override AbsWindowContent<ResultInfo> CloseExtra()
        {
            return this;
        }

        public override AbsWindowContent<ResultInfo> Init(ResultInfo parameter)
        {
            gameObject.SetActive(true);
            return this;
        }
    }
}
