/// <summary>
/// Condition reaction.
/// Reaction to change the SatisfiedState of Condition.
/// The Condition is reference to one on AllConditions asset.
/// By changing the Condition here, GlobalGame Condition will change.
/// Since Reaction decisions are based on Conditions,
///  change must be Immediate and therefore is Reaction rather than DelayedReaction.
/// </summary>

public class ConditionReaction : Reaction {
	//
}
