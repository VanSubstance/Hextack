using Assets.Scripts.Battle.Projectile;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    [CustomEditor(typeof(TowerController))]
    [CanEditMultipleObjects]
    public class TowerEditor : UnityEditor.Editor
    {
        private TowerController cont;

        private void OnSceneGUI()
        {
            try
            {
                cont = target as TowerController;
                Handles.color = Color.white;
                foreach (ProjectileInfo prj in cont.TowerInfo.projectileInfo)
                {
                    Handles.DrawWireArc(cont.transform.position, Vector3.up, Vector3.right, 360, (.5f + prj.Range) * (1 + (ServerData.Saving.GoldUpgradeLevel[cont.TowerInfo.towerType][TowerUpgradeType.Range] * .05f)));
                }

                Handles.color = Color.red;
                Collider[] cols;
                if ((cols = Physics.OverlapSphere(cont.transform.position, cont.Range, GlobalDictionary.Layer.Monster)).Length > 0)
                {
                    foreach (Collider col in cols)
                    {
                        Handles.DrawLine(cont.transform.position, col.transform.position);
                    }
                }
            }
            catch (System.NullReferenceException)
            {

            }
        }
    }
}
