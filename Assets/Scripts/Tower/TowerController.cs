using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerController : AbsPoolingContent
    {
        private MeshFilter filter;
        private MeshRenderer meshRenderer;
        private TowerInfo towerInfo;
        public override void Clear()
        {
            throw new System.NotImplementedException();
        }

        protected override bool InitExtra(AbsPoolingContent.Info _info)
        {
            if (_info is not Info info)
            {
                return false;
            }
            towerInfo = ServerData.Tower.data[info.Code].Clone();
            // 메쉬 + 메테리얼 연결
            filter.mesh = GlobalDictionary.Mesh.Tower.data[towerInfo.Code];
            meshRenderer.materials = towerInfo.Materials.Select((code) => { return GlobalDictionary.Materials.data[code]; }).ToArray();
            transform.position = info.Position;
            return true;
        }

        public new class Info : AbsPoolingContent.Info
        {
            public string Code;
            public Vector3 Position;
        }
    }
}
