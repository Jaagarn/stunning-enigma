using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    public bool canGoPlusZ;

    [SerializeField]
    public bool canGoMinusZ;

    [SerializeField]
    public bool canGoPlusX;

    [SerializeField]
    public bool canGoMinusX;

    public bool CanGoDirection(Vector3 direction)
    {
        if (Vector3.right.Equals(direction))
            return canGoPlusX;

        if (Vector3.left.Equals(direction))
            return canGoMinusX;

        if (Vector3.forward.Equals(direction))
            return canGoPlusZ;

        if (Vector3.back.Equals(direction))
            return canGoMinusZ;

        return false;
    }
}
