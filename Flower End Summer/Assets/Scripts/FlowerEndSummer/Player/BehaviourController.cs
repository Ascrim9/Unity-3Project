using System;
using FlowerEndSummer.Player;
using UnityEngine;

namespace FlowerEndSummer.Player
{
    public class BehaviourController
    {
        
    }
}


public abstract class GenericBehaviour : MonoBehaviour
{
    protected int speedFloat;
    protected BehaviourController behaviourController;
    protected int behaviourCode;
    protected bool iscanSpr;

    private void Awake()
    {
        // this.behaviourController = GetComponent<BehaviourController>();
        // speedFloat = Anima
    }
}