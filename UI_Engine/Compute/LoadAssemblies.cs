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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.UI
{
    public static partial class Compute
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public static void LoadAssemblies()
        {
            if (!m_AssemblyLoaded)
            {
                m_AssemblyLoaded = true;
                IEnumerable<Assembly> uiAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Split(',').First() == "BHoM_UI");
                if (uiAssemblies.Count() == 1)
                {
                    string folder = Path.GetDirectoryName(uiAssemblies.First().Location);
                    BH.Engine.Base.Compute.LoadAllAssemblies(folder);
                }
            }
        }


        /*************************************/
        /**** Static Fields               ****/
        /*************************************/

        private static bool m_AssemblyLoaded = false;
    }
}



