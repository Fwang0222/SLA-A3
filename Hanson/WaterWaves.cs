using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterWaves : MonoBehaviour {
    [Header("Master wave setting")]
    public float waveSpeed = 1.5f;
    public float waveHeight = 0.3f;
    public float waveFrequency = 2f;
    
    [Header("Secondary wave setting")] 
    public float secondaryWaveSpeed = 2f;
    public float secondaryWaveHeight = 0.1f;
    
    private Vector3[] _baseVertices;
    private Mesh _mesh;

    void Start() {
        _mesh = GetComponent<MeshFilter>().mesh;
        _baseVertices = _mesh.vertices;
    }

    void Update() {
        Vector3[] vertices = new Vector3[_baseVertices.Length];
        float time = Time.time;
        
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 vertex = _baseVertices[i];
            
            float wave1 = Mathf.Sin(time * waveSpeed + vertex.x * waveFrequency) * waveHeight;
            
            float wave2 = Mathf.Cos(time * secondaryWaveSpeed * 0.7f + vertex.z * waveFrequency * 1.3f) * secondaryWaveHeight;
            
            vertex.y += wave1 + wave2;
            vertices[i] = vertex;
        }
        
        _mesh.vertices = vertices;
        _mesh.RecalculateNormals();
    }
}