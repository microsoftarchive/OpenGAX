using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Setup.Configuration;

namespace Microsoft.Practices.RecipeFramework
{
    public static class VisualStudioInstance
    {
        //public static string GetCurrentInstance
        //{
        //    get { return "aea5cb0f"; }
        //}
        public static string GetCurrentInstance
        {
            get
            {
                var query = new SetupConfiguration();
                var query2 = (ISetupConfiguration2)query;
                var e = query2.EnumAllInstances();
                var helper = (ISetupHelper)query;
                int fetched;
                var instances = new ISetupInstance[1];
                e.Next(1, instances, out fetched);
                if (fetched > 0)
                    return GetInstance(instances[0], helper);
                return String.Empty;
            }
        }
        public static String GetInstance(ISetupInstance instance, ISetupHelper helper)
        {
            var instance2 = (ISetupInstance2)instance;
            var state = instance2.GetState();
            return instance2.GetInstanceId();
        }
    }
}
