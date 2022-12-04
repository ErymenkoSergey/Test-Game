using Morkwa.Interface;
using Morkwa.Mechanics.CommonBehaviours;
using UnityEngine;

public class BaceCharacter : CommonBehaviour
{
    protected float Speed;
    protected float AlarmClearTimer;
    protected float TimeWait;
    protected float Acceleration;
    [SerializeField] protected Animator _animator;

    protected IGame IGame;
    protected IAudio IAudio;
    protected ISpawning ISpawner;

    [SerializeField] protected AudioClip StepsSound;
    [SerializeField] protected AudioSource StepsSource;

    protected Color32 DefaultColor;
    protected Color32 HunterColor;

    public virtual void SetInfo(IGame game) { }

    public virtual void SetConfiguration() { }

    public virtual void Move() { }

    public virtual void SetAnimatorStatus(string name, int value) { }
}
