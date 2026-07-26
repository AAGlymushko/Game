public static class Constants
{
    public const float UNIT = 1f;

    public const float CELL_SIZE = UNIT;
    public const float WALL_THICKNESS = 0.2f * UNIT;
    public const float WALL_HEIGHT = 2f * UNIT;     
    public const float WALL_LENGTH = CELL_SIZE + WALL_THICKNESS; 
    public const float HALF_CELL = CELL_SIZE / 2f;
    public const float HALF_HEIGHT = WALL_HEIGHT / 2f;
    public const float PLAYER_SIZE = UNIT / 5f;

    public const int LENGTH = 21;
    public const int REFERENCE_POINTS_COUNT = 17;
    public const int INTERSECTIONS_COUNT = 123;
}