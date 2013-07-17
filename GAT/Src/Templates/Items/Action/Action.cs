#region Using Directives

using System;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;

#endregion

namespace $rootnamespace$
{
	class $ActionName$Action: Microsoft.Practices.RecipeFramework.Action
    {
        #region Input Properties

        [Input]
        public string RecipeArgument1
        {
            get { return recipeArgument1; }
            set { recipeArgument1 = value; }
        } string recipeArgument1;

        #endregion

        #region Output Properties

        [Output]
        public string ActionOutput1
        {
            get { return actionOutput1; }
            set { actionOutput1 = value; }
        } string actionOutput1;

        #endregion

        #region IAction Members

        public override void Execute()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Undo()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
