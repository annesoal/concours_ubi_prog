using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Classe d'exemple d'une tour de base.
 *
 * Cette classe est destinée à être héritée par des tours plus spécifiques.
 * Elle contient tous les comportements communs aux tours.
 */
public abstract class BaseTower : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;
}
