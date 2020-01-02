﻿using System.Linq;
using UnityEngine;

public abstract class Action
{
    protected Rat rat;
    protected RatController ratController;

    public Action(Rat rat)
    {
        this.rat = rat;
        ratController = rat.GetComponent<RatController>();
    }

    public abstract void OnStart();
    public abstract void OnEnd();
    public abstract void Update();

    protected void SearchForAviableEnemy()
    {
        if (rat.IsRanged())
        {
            RangedBehaviour();
        }
        else
        {
            MeeleBehaviour();
        }
    }

    private void RangedBehaviour()
    {
        if (rat.fieldOfView.GetEnemyRatsInRange().Count > 0)
        {
            ratController.SetActionTo(new Shoot(rat, rat.fieldOfView.GetEnemyRatsInRange().OrderByDescending(r => Vector3.Distance(rat.transform.position, r.transform.position)).FirstOrDefault()));
        }
    }

    private void MeeleBehaviour()
    {
        foreach (Rat enemy in rat.fieldOfView.GetEnemyRatsInRange())
        {
            RatController ratControllerOfEnemy = enemy.GetComponent<RatController>();

            if (enemy.IsRanged())
            {
                ratController.SetActionTo(new ApproachRanged(rat, enemy));
            }
            else
            {
                if (!ratControllerOfEnemy.IsFighting())
                {
                    ratController.SetActionTo(new ApproachMeele(rat, enemy));
                    ratControllerOfEnemy.SetActionTo(new ApproachMeele(enemy, rat));
                }
            }
        }
    }
}
