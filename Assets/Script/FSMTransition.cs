public class FSMTransition
{
    public string NextState;
    public float TransitionWeight;

    public FSMTransition(string nextState, float weight)
    {
        NextState = nextState;
        TransitionWeight = weight;
    }
}
