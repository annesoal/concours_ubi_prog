namespace Grid.Interface
{
    public interface ITopOfCell
    {
        public TypeTopOfCell GetType();
    }

    public enum TypeTopOfCell
    {
        Enemy,
        Player,
        Obstacle,
        Resource
    }
}