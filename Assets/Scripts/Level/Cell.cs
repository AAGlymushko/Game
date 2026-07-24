public class Cell
{
    public bool isVisited;

    public int x;
    public int z;

    public bool up;
    public bool down;
    public bool left;
    public bool right;

    public Cell(bool isVisited, int x, int z)
    {
        this.isVisited = isVisited;
        this.x = x;
        this.z = z;
        up = down = left = right = true;
    }
}