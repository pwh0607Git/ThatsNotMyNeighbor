using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class InteractionManager : BehaviourSingleton<InteractionManager>
{
    protected override bool IsDontDestroy() => false;

    [SerializeField] GateOpener gateOpener;

    [SerializeField] private ResidentController currentResident;

    public IDCardController idCardController;
    public EntryRequestController entryRequestController;

    public bool isOpen { get; private set; }

    void Start()
    {
        TryGetComponent(out idCardController);
        TryGetComponent(out entryRequestController);

        gateOpener.OnGateUpdated += UpdateGate;

        isOpen = false;
    }

    public void SetCurrentResident(ResidentController resident)
    {
        if (resident is DDDController) return;

        this.currentResident = resident;

        idCardController.SetData(resident.profile);
        entryRequestController.SetData(resident.profile);

        DOVirtual.DelayedCall(0.2f, () =>
        {
            idCardController.SetActiveButton(true);
            entryRequestController.SetActiveButton(true);
        });
    }

    public UnityAction<Profile> OnExitResident;

    public void ExitResident()
    {
        if (currentResident == null) return;

        idCardController.SetActiveButton(false);
        entryRequestController.SetActiveButton(false);

        Profile profile = currentResident.profile;
        OnExitResident?.Invoke(profile);

        currentResident = null;
    }

    public void UpdateGate(bool on)
    {
        isOpen = on;

        if (currentResident == null) return;

        currentResident.Exit();
        ExitResident();
    }

    public void CleanDoppel()
    {
        currentResident.gameObject.SetActive(false);
        currentResident = null;
        
        idCardController.SetActiveButton(false);
        entryRequestController.SetActiveButton(false);
    }
}