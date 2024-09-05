using UnityEngine;

public class Enemy_Collapse_Skill02Enter : Enemy_State
{
    private float tinkerTimer = 0;

    private Enemy_Collapse enemy;
    public Enemy_Collapse_Skill02Enter(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Collapse _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        if (enemy.SetPath() < 2)
        {
            enemy.stateMachine.ChangeState(enemy.IdleState);
        }
        enemy.GazePoint.position = enemy.target.position;
        enemy.moveDir = enemy.SetVectorOne(enemy.GazePoint.position - enemy.Mover.position);
        if (enemy.moveDir.x != 0)
            enemy.moveDir.y = 0;
        tinkerTimer = enemy.SkillArray[1].animationPlayTime - enemy.SkillArray[1].Delay;
        stateTimer = enemy.SkillArray[1].animationPlayTime;
    }

    public override void Update()
    {
        base.Update();
        tinkerTimer -= Time.deltaTime;
        if (tinkerTimer < 0)
        {
            enemy.BeforeAttackTinker(Vector3.zero);
            tinkerTimer += 20f;
        }
        if (stateTimer < 0)
        {
            enemy.stateMachine.ChangeState(enemy.Skill02State);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}