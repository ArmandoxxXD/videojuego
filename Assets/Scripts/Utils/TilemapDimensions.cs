using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDimensions : MonoBehaviour
{
    public Tilemap tilemap;

    void Start()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap no asignado.");
            return;
        }

        // Obtener los límites del Tilemap
        BoundsInt bounds = tilemap.cellBounds;

        // Calcular el ancho y el alto en bloques ocupados
        int ancho = bounds.size.x;
        int alto = bounds.size.y;

        Debug.Log($"Ancho del Tilemap: {ancho} bloques");
        Debug.Log($"Alto del Tilemap: {alto} bloques");
    }
}
