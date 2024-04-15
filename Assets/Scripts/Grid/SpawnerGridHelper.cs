using System.Collections.Generic;
using Grid.Blocks;
using Grid.Interface;
using UnityEngine;

namespace Grid
{
    public class SpawnerGridHelper:GridHelper
    {
        private List<Type> _validTypes;
        public SpawnerGridHelper(Vector2Int position, List<Type> types) : base(position)
        {
            _validTypes = types;
        }

        //TODO pas de spawn sur player
        public override bool IsValidCell(Vector2Int position)
        {
            foreach (var type in _validTypes)
            {
                int translatedType = BlockType.Translate(type);
                Cell cell = TilingGrid.grid.GetCell(position);
                if (cell.Has(translatedType)) 
                    return true;
            }
            return false;
        }

        public override Vector2Int GetHelperPosition()
        {
            return currentCell.position;
        }

        public override void SetHelperPosition(Vector2Int position)
        {
            currentCell.position = position;
        }

    }
}