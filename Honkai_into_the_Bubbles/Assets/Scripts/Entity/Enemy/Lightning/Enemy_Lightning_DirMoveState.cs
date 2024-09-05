using UnityEngine;

public class Enemy_Lightning_DirMoveState : Enemy_State
{
    private int movementMultiplyer = 1;
    private StrictMoveNode SMN;

    private Enemy_Lightning enemy;
    public Enemy_Lightning_DirMoveState(Enemy _enemyBase, Enemy_StateMachine _stateMachine, string _animBoolName, Enemy_Lightning _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        SMN = enemy.StrictMoveDirections[enemy.strictMoveProcess % enemy.StrictMoveDirections.Count];
        movementMultiplyer = SMN.movementMultiplyer;
    }

    public override void Update()
    {
        base.Update();
        if (Vector3.Distance(enemy.Mover.position, enemy.MovePoint.transform.position) >= .05f)
        {
            enemy.Mover.position = Vector3.MoveTowards
            (
                enemy.Mover.position,
                enemy.MovePoint.transform.position,
                enemy.MoveSpeed * Time.deltaTime
            );
        }
        else
        {
            enemy.Mover.position = enemy.MovePoint.transform.position; // make position accurate
            enemy.MovePoint.prevPos = enemy.Mover.position; // used in external movepoint control

            if (enemy.AreaDetectTarget(enemy.followRadius).Length > 0)
            {
                enemy.stateMachine.ChangeState(enemy.AggroIdleState);
            }
            else if (movementMultiplyer < 0 || enemy.MovepointAdjustCheck())
            {
                enemy.stateMachine.ChangeState(enemy.IdleState);
            }
            else
            {
                enemy.MovePoint.transform.position += enemy.moveDir;
                enemy.SetAnimDir(enemy.moveDir);
            }

            movementMultiplyer--;
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Mover.position = enemy.MovePoint.transform.position;
    }
}
