using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Ayarları")]
    public GameObject linePrefab;
    public int lineCount = 30; 
    public float spacing = 2f; 
    public float widthLine  = 0.05f; 
    void Start()
    {
        float length = lineCount * spacing * 2;

        for (int i = -lineCount; i <= lineCount; i++)
        {
            GameObject horizontalLine = Instantiate(linePrefab, new Vector2(0, i * spacing), Quaternion.identity, transform);
            horizontalLine.transform.localScale = new Vector3(length, widthLine, 1f);

            GameObject verticalLine = Instantiate(linePrefab, new Vector2(i * spacing, 0), Quaternion.Euler(0, 0, 90f), transform);
            verticalLine.transform.localScale = new Vector3(length, widthLine, 1f);
        }
    }
}