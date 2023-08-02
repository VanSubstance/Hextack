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
            float cnt = 1;
            foreach (PathManager.Path path in cont.PathList)
            {
                cnt -= .2f;
                prev = null;
                Handles.color = new Color(cnt, cnt, cnt, 1);
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
