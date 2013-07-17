using System;
using System.Windows.Forms.Design;
using Microsoft.Practices.RecipeFramework;

namespace $PackageNamespace$.Actions
{
	public class HelloWorldAction : Microsoft.Practices.RecipeFramework.Action
	{
        
        #region Input Properties

        [Input(Required=true)]
        public string HelloMessage
        {
            get 
			{ 
				return helloMessage; 
			}
            set 
			{ 
				helloMessage = value; 
			}
        }

		private string helloMessage;

        #endregion

		#region Output Properties

		[Output]
		public int OutputValue
		{
			get
			{
				return outputValue;
			}
			set
			{
				outputValue = value;
			}
		}

		private int outputValue;

		#endregion

		#region IAction Members

        public override void Execute()
        {
            IUIService uiService = GetService<IUIService>(true);
            if (uiService !=null )
            {
                uiService.ShowMessage(helloMessage, "Hello Action");
            }
            else
            {
               System.Windows.Forms.MessageBox.Show(helloMessage, "Hello Action");
            }
        }

        public override void Undo()
        {
        }

		#endregion
    }
}