using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FSM
{
    public Dictionary<string, Dictionary<string, FSMTransition>> transitions = new();

    public void AddTransition(string state, string input, FSMTransition transition)
    {
        if (!transitions.ContainsKey(state))
            transitions[state] = new Dictionary<string, FSMTransition>();

        transitions[state][input] = transition;
    }

    public FSMTransition GetTransition(string state, string input)
    {
        if (transitions.ContainsKey(state) && transitions[state].ContainsKey(input))
            return transitions[state][input];
        return null; // no valid transition (infection spreads)
    }
}
