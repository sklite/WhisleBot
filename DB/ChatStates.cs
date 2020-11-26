using System;
using System.Collections.Generic;
using System.Text;

namespace WhisleBotConsole.DB
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
