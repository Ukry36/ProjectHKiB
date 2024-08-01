public class Enemy_Collapse_StateMachine
{
    public Enemy_Collapse_State currentState { get; private set; }

    public void Initialize(Enemy_Collapse_State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(Enemy_Collapse_State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
