using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Battle.Area
{
    [CustomEditor(typeof(AreaController))]
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
            }
            catch (System.NullReferenceException)
            {

            }
        }
    }
}
