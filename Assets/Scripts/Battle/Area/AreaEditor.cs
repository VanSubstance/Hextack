using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Battle.Area
{
    [CustomEditor(typeof(AreaController))]
    [CanEditMultipleObjects]
    public class AreaEditor : UnityEditor.Editor
    {
        private AreaController cont;
        private void OnSceneGUI()
        {
            try
            {
                cont = target as AreaController;
                Handles.color = Color.white;
                Handles.DrawWireArc(cont.transform.position, Vector3.up, Vector3.right, 360, cont.Range);

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
