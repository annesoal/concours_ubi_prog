using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
{
    public ulong clientId;
    public CharacterSelectUI.CharacterId characterSelection;

    public bool Equals(PlayerData other)
    {
        return this.clientId == other.clientId &&
               this.characterSelection == other.characterSelection;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref characterSelection);
    }
}
