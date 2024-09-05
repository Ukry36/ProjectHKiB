using UnityEngine;

public class Enemy_Rusher_DirIdleState : Enemy_State
{
    private Collider2D[] colliders;

    private Enemy_Rusher enemy;
    public Enemy_Rusher_DirIdleState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Rusher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.strictMoveProcess++;
        StrictMoveNode SMD = enemy.StrictMoveDirections[enemy.strictMoveProcess % enemy.StrictMoveDirections.Count];
        stateTimer = SMD.waitTime <= 0 ? Random.Range(0.5f, 3f) : SMD.waitTime;

        enemy.SetAnimDir(SMD.gazeDir);
        enemy.moveDir = SMD.gazeDir;
    }

    public override void Update()
    {
        base.Update();
        if (!enemy.isDetectCooltime)
        {
            colliders = enemy.AreaDetectTarget(enemy.followRadius);
            enemy.StartCoroutine(enemy.DetectCooltime());
            if (colliders != null && colliders.Length > 0)
            {
                colliders = null;
                enemy.stateMachine.ChangeState(enemy.AggroMoveState);
            }
        }
        else if (stateTimer < 0)
        {
            enemy.stateMachine.ChangeState(enemy.DirMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
