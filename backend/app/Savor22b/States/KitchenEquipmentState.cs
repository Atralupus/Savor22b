namespace Savor22b.States;

using Savor22b.Util;
using Bencodex.Types;
using Libplanet.Headless.Extensions;

public class KitchenEquipmentState : State
{
    public KitchenEquipmentState(Guid stateID, int kitchenEquipmentID, int kitchenEquipmentCategoryID, long cookingStartedBlockIndex, long cookingDurationBlock)
    {
        StateID = stateID;
        KitchenEquipmentID = kitchenEquipmentID;
        KitchenEquipmentCategoryID = kitchenEquipmentCategoryID;
        CookingStartedBlockIndex = cookingStartedBlockIndex;
        CookingDurationBlock = cookingDurationBlock;
    }

    public KitchenEquipmentState(Guid stateID, int kitchenEquipmentID, int kitchenEquipmentCategoryID)
    {
        StateID = stateID;
        KitchenEquipmentID = kitchenEquipmentID;
        KitchenEquipmentCategoryID = kitchenEquipmentCategoryID;
        CookingStartedBlockIndex = null;
        CookingDurationBlock = null;
    }

    public KitchenEquipmentState(Dictionary encoded)
    {
        StateID = encoded[nameof(StateID)].ToGuid();
        KitchenEquipmentID = encoded[nameof(KitchenEquipmentID)].ToInteger();
        KitchenEquipmentCategoryID = encoded[nameof(KitchenEquipmentCategoryID)].ToInteger();
        CookingStartedBlockIndex = encoded[nameof(CookingStartedBlockIndex)].ToNullableLong();
        CookingDurationBlock = encoded[nameof(CookingDurationBlock)].ToNullableLong();
    }

    public Guid StateID { get; private set; }

    public int KitchenEquipmentID { get; private set; }

    public int KitchenEquipmentCategoryID { get; private set; }

    public long? CookingStartedBlockIndex { get; private set; }

    public long? CookingDurationBlock { get; private set; }

    public IValue Serialize()
    {
        var pairs = new[]
        {
            new KeyValuePair<IKey, IValue>((Text)nameof(StateID), StateID.Serialize()),
            new KeyValuePair<IKey, IValue>((Text)nameof(KitchenEquipmentID), KitchenEquipmentID.Serialize()),
            new KeyValuePair<IKey, IValue>((Text)nameof(KitchenEquipmentCategoryID), KitchenEquipmentCategoryID.Serialize()),
            new KeyValuePair<IKey, IValue>((Text)nameof(CookingStartedBlockIndex), CookingStartedBlockIndex.Serialize()),
            new KeyValuePair<IKey, IValue>((Text)nameof(CookingDurationBlock), CookingDurationBlock.Serialize()),
        };
        return new Dictionary(pairs);
    }

    public bool IsInUse(long currentBlockIndex)
    {
        return BlockUtil.CalculateIsInProgress(currentBlockIndex, CookingStartedBlockIndex ?? 0, CookingDurationBlock ?? 0);
    }

    public KitchenEquipmentState StartCooking(long currentBlockIndex, long cookingDurationBlock)
    {
        return new KitchenEquipmentState(
            StateID, KitchenEquipmentID, KitchenEquipmentCategoryID, currentBlockIndex, cookingDurationBlock);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        KitchenEquipmentState other = (KitchenEquipmentState)obj;
        return StateID == other.StateID && KitchenEquipmentID == other.KitchenEquipmentID;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = (hash * 23) ^ StateID.GetHashCode();
            hash = (hash * 23) ^ KitchenEquipmentID.GetHashCode();
            return hash;
        }
    }

}
