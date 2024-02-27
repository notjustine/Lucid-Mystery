using UnityEngine;

public abstract class BossState
{
    protected BossController controller;

    public BossState(BossController controller)
    {
        this.controller = controller;
    }

    public abstract void Tick();
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
}

public class SlamAttackState : BossState
{
    public SlamAttackState(BossController controller) : base(controller) { }

    public override void Tick()
    {
        controller.PerformSlamAttack();
        // Transition logic can be added here or in the controller's Tick based on conditions
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        // Setup for Slam Attack
    }
}

public class SteamAttackState : BossState
{
    public SteamAttackState(BossController controller) : base(controller) { }

    public override void Tick()
    {
        controller.PerformSteamAttack();
        // Transition logic can be added here or in the controller's Tick based on conditions
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        // Setup for Steam Attack
    }
}
