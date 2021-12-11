using System;
using UnityEngine;

public enum GamePhases 
{ 
    AnchorSetup,
    UnitsSetup,
    Waiting,
    Active,
    Passive
}

[Serializable]
public class GamePhasesDictionary : SerializableDictionary<GamePhases, GameObject> { }

public class PhaseController : MonoBehaviour
{
    [SerializeField]
    private GamePhases phase;

    public void SwitchPhase()
    {
        switch (phase)
        {
            case GamePhases.AnchorSetup:
                AnchorManagement();
                break;
            case GamePhases.UnitsSetup:
                UnitsManagement();
                break;
            case GamePhases.Waiting:
                WaitForPlayer();
                break;
            case GamePhases.Active:
                ActivePhase();
                break;
            case GamePhases.Passive:
                PassivePhase();
                break;
        }
    }

    private void AnchorManagement()
    {

    }

    private void UnitsManagement()
    {

    }

    private void WaitForPlayer()
    {

    }

    private void ActivePhase()
    {

    }

    private void PassivePhase()
    {

    }
}