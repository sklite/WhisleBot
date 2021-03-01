namespace Wbcl.Core.Models.Database
{
    public enum ChatState
    {
        NotUsed,
        Standrard,
        NewGroupToAdd,
        NewWordToGroupAdd,
        EditExistingGroup,
        /// <summary>
        /// Select group for removing subscriptions
        /// </summary>
        RemoveSettingsStep1
    }
}
