using DG.Tweening;
using UnityEngine;

public class InteractionManager : BehaviourSingleton<InteractionManager>
{
    protected override bool IsDontDestroy() => false;

    [SerializeField] private ResidentController currentResident;

    public IDCardController idCardController;
    public EntryRequestController entryRequestController;

    void Start()
    {
        TryGetComponent(out idCardController);
        TryGetComponent(out entryRequestController);
    }

    public void SetCurrentResident(ResidentController resident)
    {
        this.currentResident = resident;

        idCardController.SetData(resident.profile);
        entryRequestController.SetData(resident.profile);

        DOVirtual.DelayedCall(0.2f, () =>
        {
            idCardController.SetActiveButton(true);
            entryRequestController.SetActiveButton(true);
        });
    }

    public void ExitResident()
    {
        idCardController.SetActiveButton(false);
        entryRequestController.SetActiveButton(false);
    }
}