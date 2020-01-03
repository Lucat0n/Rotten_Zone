﻿public class Move : Action
{
    public Move(Rat rat) : base(rat)
    {
        OnStart();
    }

    public override void OnEnd()
    {
        
    }

    public override void OnStart()
    {
        rat.agent.SetDestination(ratController.GetDestinationOfNextWaypoint());
        rat.ChangeAnimationTo("walking");
    }

    public override void Update()
    {
        ChangeWaypointWhenCloseToCurrent();
        SearchForAviableEnemy();
        CheckForCapturePoint();
    }

    private void ChangeWaypointWhenCloseToCurrent()
    {
        if ((rat.agent.remainingDistance < rat.agent.stoppingDistance) && (ratController.stepOnPath != ratController.path.Count - 1))
        {
            ratController.stepOnPath++;
            rat.agent.SetDestination(ratController.GetDestinationOfNextWaypoint());
        }
    }

    private void CheckForCapturePoint()
    {
        if (rat.capturePoint != null && (rat.capturePoint.owner != rat.team))
        {
            ratController.SetActionTo(new Capture(rat));
        }
    }
}
