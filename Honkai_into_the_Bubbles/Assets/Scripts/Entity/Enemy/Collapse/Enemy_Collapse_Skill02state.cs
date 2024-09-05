

public class Enemy_Collapse_Skill02 : Enemy_State
{
    private Enemy_Collapse enemy;
    public Enemy_Collapse_Skill02(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Collapse _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (finishTriggerCalled)
        {
            enemy.StartCoroutine(enemy.SkillCooltime(enemy.SkillArray[1]));
            enemy.stateMachine.ChangeState(enemy.AggroMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}