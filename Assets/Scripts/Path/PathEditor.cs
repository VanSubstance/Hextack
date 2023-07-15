using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Path
{
    [CustomEditor(typeof(PathManager))]
    [CanEditMultipleObjects]
    public class PathEditor : UnityEditor.Editor
    {
        private PathManager cont;
        private void OnSceneGUI()
        {
            cont = target as PathManager;
            Transform prev;
            foreach (PathManager.Path path in cont.PathList)
            {
                prev = null;
                Handles.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
                foreach (Transform tr in path.TargetTr)
                {
                    if (prev != null)
                    {
                        Handles.DrawLine(prev.position, tr.position, 2);
                    }
                    prev = tr;
                }
            }
        }
    }
}
