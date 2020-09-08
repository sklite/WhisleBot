using System;
using System.Collections.Generic;
using System.Text;

namespace WhisleBotConsole.DB
{
    public enum ChatState
    {
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
