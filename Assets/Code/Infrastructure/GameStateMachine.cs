public class GameStateMachine : StateMachine
{
    public IState ActiveState => current.State;
}
