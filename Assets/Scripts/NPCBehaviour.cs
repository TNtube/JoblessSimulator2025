using System;
using DG.Tweening;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");
    private static readonly int Sit1 = Animator.StringToHash("Sit");
    private static readonly int Fail = Animator.StringToHash("Fail");
    private static readonly int Sad = Animator.StringToHash("Sad");

    [SerializeField]
    private Animator animator;

    public bool TryingToSit = false;
    public bool Sitting = false;
    public bool Lost = false;
    
    public ChairsManager chairsManager;

    [SerializeField]
    private float animationSpeed = .7f;
    
    private Sequence _sittingSequence;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator.SetFloat(WalkSpeed, animationSpeed);
    }

    private void OnDrawGizmos()
    {
    }

    public void SetTarget(Vector3 target)
    {
        if (TryingToSit || Sitting)
            return;
        target.y = 0;
        transform.LookAt(target);
        transform.position = target;
    }
    
    public void StartSitting(Chair chair)
    {
        if (Sitting || TryingToSit)
            return;

        var direction = chair.NpcTarget.position;
        var chairPosition = chair.transform.position;
        
        _sittingSequence = DOTween.Sequence();
        _sittingSequence.Append(transform.DOLookAt(chair.NpcTarget.position, 0.3f));
        _sittingSequence.Append(transform.DOMove(chair.NpcTarget.position, 1f));
        _sittingSequence.Join(transform.DOLookAt(direction * 100, 1f));
        _sittingSequence.AppendCallback(() =>
        {
            animator.SetTrigger(Sit1);
            Sitting = true;
            TryingToSit = false;
        });

        _sittingSequence.OnUpdate(() =>
        {
            if (chair.IsOccupied)
            {
                Lost = true;
                TryingToSit = false;
                Sitting = false;
                _sittingSequence.Kill();
                _sittingSequence = DOTween.Sequence();
                _sittingSequence.Append(transform.DOLookAt(chairPosition, 0.3f));
                _sittingSequence.AppendCallback(() => animator.SetTrigger(Fail));
                _sittingSequence.AppendInterval(4.183f);
                _sittingSequence.AppendCallback(() =>
                {
                    chairsManager.RemoveOneChair(chair);
                });
                _sittingSequence.Append(transform.DOLookAt(direction * 3, 0.3f));
                _sittingSequence.Append(transform.DOMove(direction * 3, 5f));
                _sittingSequence.AppendCallback(() =>
                {
                    animator.SetTrigger(Sad);
                });
                _sittingSequence.Append(transform.DOLookAt(direction, 0.3f));
            }
        });

        _sittingSequence.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Sit()
    {
        animator.SetTrigger(Sit1);
    }
}
