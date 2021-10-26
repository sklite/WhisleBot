namespace Wbcl.Core.Models.Database
{
    public enum ChatState
    {
        NotUsed,
        /// <summary>
        /// User is not in any bot chat dialogues
        /// </summary>
        Standrard,
        /// <summary>
        /// User is about to add group
        /// </summary>
        NewGroupToAdd,
        /// <summary>
        /// User has added group, now adding keywords
        /// </summary>
        NewWordToGroupAdd,
        /// <summary>
        /// User is in editing chat preferences dialogue
        /// </summary>
        EditExistingGroup,
        /// <summary>
        /// Select group for removing subscriptions
        /// </summary>
        RemoveSettingsStep1
    }
}
