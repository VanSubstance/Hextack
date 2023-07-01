using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.Editor
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class RectPollMesh : MonoBehaviour
    {
        [SerializeField]
        private float h = 1, r = 1;
        Mesh mesh;
        Vector3[] vertices;
        int[] triangles;

        void Awake()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            mesh.name = "RectMesh";
            setMeshData();
            createProceduralMesh();
        }

        void setMeshData()
        {
            vertices = new Vector3[] {
            new Vector3(0, h, r / 2f),
            new Vector3(r / 2f, h, 0),
            new Vector3(0, h, -r / 2f),
            new Vector3(-r / 2f, h, 0),

            new Vector3(0, 0, r),
            new Vector3(r, 0, 0),
            new Vector3(0, 0, -r),
            new Vector3(-r, 0, 0),
            };

            triangles = new int[] {
                0, 1, 2,
                0, 2, 3,

                0, 4 + 0, 1, 4 + 0, 4+ 1, 1,
                1, 4 + 1, 2, 4 + 1, 4+ 2, 2,
                2, 4 + 2, 3, 4 + 2, 4+ 3, 3,
                3, 4 + 3, 0, 4 + 3, 4+ 0, 0,

                4 + 2, 4 + 1, 4,
                4 + 3, 4 + 2, 4,
            };
        }

        void createProceduralMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            gameObject.AddComponent<MeshCollider>();
        }

#if UNITY_EDITOR
        [ContextMenu("SaveMesh")]
        void saveMesh()
        {
            string path = "Assets/Resources/Meshs/MyMesh.asset";
            AssetDatabase.CreateAsset(mesh, AssetDatabase.GenerateUniqueAssetPath(path));
            AssetDatabase.SaveAssets();
        }
#endif
    }
}