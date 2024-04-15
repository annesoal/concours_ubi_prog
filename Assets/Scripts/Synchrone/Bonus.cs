using System;
using Grid;
using Grid.Interface;
using Sound;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Synchrone
{
    public class Bonus : NetworkBehaviour, ITopOfCell
    {
        [field: SerializeField] private float _multiplier;
        [SerializeField] protected AudioClip bonusAudioClip;

        public static float Multiplier;

        private TypeTopOfCell _currentType;

        public void Awake()
        {
            _currentType = TypeTopOfCell.Bonus;
            Multiplier = _multiplier;
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                TowerDefenseManager.Instance.AddBonus(this.gameObject);
            }
        }

        public TypeTopOfCell GetType()
        {
            return _currentType;
        }

        public GameObject ToGameObject()
        {
            return this.gameObject;
        }

        public void OnDestroy()
        {
            //SoundFXManager.instance.PlaySoundFXCLip(bonusAudioClip, transform,1f);
            
            Vector2Int gridPosition = TilingGrid.LocalToGridPosition(transform.position);
            
            Cell toUpdate = TilingGrid.grid.GetCell(gridPosition);

            toUpdate.ObjectsTopOfCell.Remove(this);
            
            TilingGrid.grid.UpdateCell(toUpdate);
            
            TowerDefenseManager.Instance.RemoveBonus(this.gameObject);
        }
    }
}