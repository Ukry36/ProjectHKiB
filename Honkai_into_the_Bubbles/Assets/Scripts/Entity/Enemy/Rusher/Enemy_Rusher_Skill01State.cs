
public class Enemy_Rusher_Skill01State : Enemy_State
{
    private Enemy_Rusher enemy;
    public Enemy_Rusher_Skill01State(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Rusher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        for (int i = 0; i < 3; i++)
            if (!enemy.MovepointAdjustCheck())
            {
                enemy.MovePoint.transform.position += enemy.moveDir;
                enemy.Mover.position = enemy.MovePoint.transform.position;
            }
            else break;

    }

    public override void Update()
    {
        base.Update();
        if (finishTriggerCalled)
        {
            enemy.StartCoroutine(enemy.Skill01Cooltime());
            enemy.stateMachine.ChangeState(enemy.AggroMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.BackStep(3f);
    }
}
