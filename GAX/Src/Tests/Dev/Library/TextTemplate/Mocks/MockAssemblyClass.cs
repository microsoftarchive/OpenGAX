using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.RecipeFramework.Library.TextTemplate.Tests.Mocks
{
    /// <summary>
    /// Assembly by the templateClass generated.
    /// </summary>
    public static class MockAssemblyClass
    {
        static IDictionary<string, decimal> clientAmount;

        public static string GetName()
        {
            return "MockAssemblyClass";
        }

        public static IDictionary<string, decimal> ClientAmounts
        {
            get
            {
                if (clientAmount == null)
                {
                    clientAmount = new Dictionary<string, decimal>();
                    clientAmount.Add("Mike", (decimal)101.11);
                    clientAmount.Add("John", (decimal)102.22);
                    clientAmount.Add("Charly", (decimal)103.35);
                }
                return clientAmount;
            }
        }
    }
}
