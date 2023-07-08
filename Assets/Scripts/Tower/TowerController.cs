﻿using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerController : AbsPoolingContent
    {
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
            GetComponent<MeshFilter>().mesh = GlobalDictionary.Mesh.Tower.data[towerInfo.Code];
            GetComponent<MeshRenderer>().materials = towerInfo.Materials.Select((code) => { return GlobalDictionary.Materials.data[code]; }).ToArray();
            transform.position = new Vector3(info.Position.x, 0, info.Position.z);
            gameObject.SetActive(true);
            return true;
        }

        public new class Info : AbsPoolingContent.Info
        {
            public string Code;
            public Vector3 Position;
        }
    }
}
