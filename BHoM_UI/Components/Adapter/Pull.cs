/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.Adapter;
using BH.oM.Base;
using BH.oM.Adapter;
using BH.oM.Data.Requests;
using BH.oM.Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Base.Components
{
    public class PullCaller : Caller
    {
        /*************************************/
        /**** Properties                  ****/
        /*************************************/

        public override System.Drawing.Bitmap Icon_24x24 { get; protected set; } = Properties.Resources.Pull;

        public override Guid Id { get; protected set; } = new Guid("B25011DD-5F30-4279-B9D9-0F9C169D6685");

        public override string Category { get; protected set; } = "Adapter";


        /*************************************/
        /**** Constructors                ****/
        /*************************************/

        // The wrapping of the Caller is needed in order to specify the `active` boolean input
        public PullCaller() : base(typeof(PullCaller).GetMethod("Pull")) { }


        /*************************************/
        /**** Override Method             ****/
        /*************************************/

        public override object Run(List<object> inputs)
        {
            if (inputs.Count > 0)
            {
                BHoMAdapter adapter = inputs[0] as BHoMAdapter;
                Guid id = adapter.AdapterGuid;
                if (id != m_AdapterId)
                {
                    m_AdapterId = id;
                    adapter.DataUpdated += (sender, e) => ExpireSolution();
                }
            }

            return base.Run(inputs);
        }

        /*************************************/

        protected override bool ShouldCalculateNewResult(List<object> inputs, ref object result)
        {
            return (bool)inputs.Last() == true;
        }


        /*************************************/
        /**** Public Method               ****/
        /*************************************/

        [Description("Adapter Action 'Pull': imports objects from an external software. \nAlso allows for querying through Requests.")]
        [Input("adapter", "Adapter to the external software")]
        [Input("request", "Input a Request to pull only objects that satisfy a certain rule. \n" +
            "Input a Type to pull only objects of that type.")]
        [Input("pullType", "Pull type. Connect the enum PullType for all the alternatives.")]
        [Input("actionConfig", "Configuration for this Action. You can input an ActionConfig (it contains the configs common to all Toolkits); \n" +
            "consider that Toolkits may have a custom ActionConfig (e.g. GSAConfig, SpeckleConfig).")]
        [Input("active", "Execute the pull")]
        [Output("objects", "Objects pulled")]
        public static IEnumerable<object> Pull(BHoMAdapter adapter, object request = null,
            PullType pullType = PullType.AdapterDefault, ActionConfig actionConfig = null,
            bool active = false)
        {
            if (!active)
                return new List<object>();

            IRequest actualRequest = null;
            if (!adapter.SetupPullRequest(request, out actualRequest))
            {
                BH.Engine.Base.Compute.RecordError($"Invalid `{nameof(request)}` input.");
                return null;
            }

            ActionConfig pullConfig = null;
            if (!adapter.SetupPullConfig(actionConfig, out pullConfig))
            {
                BH.Engine.Base.Compute.RecordError($"Invalid `{nameof(actionConfig)}` input.");
                return null;
            }

            return adapter.Pull(actualRequest, pullType, pullConfig);
        }

        /*************************************/
        /**** Public Method               ****/
        /*************************************/

        private Guid m_AdapterId;

        /*************************************/
    }
}



