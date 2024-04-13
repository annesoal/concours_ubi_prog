using Systemre;
using UnityEngine;

namespace Grid.Interface
{
    public interface ITopOfCell 
    {
        public TypeTopOfCell GetType();
        public GameObject ToGameObject();
    }

    public enum TypeTopOfCell
    {
        Enemy,
        Player,
        Obstacle,
        Resource,
        Building,
        Bonus,
        Malus
    }
}