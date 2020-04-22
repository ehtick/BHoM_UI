﻿/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using System.Text;
using System.Threading.Tasks;

namespace BHoM_UI
{
    partial class Program
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static void CopyDatasets(string sourceFolder, string targetFolder)
        {
            foreach (string path in Directory.GetDirectories(sourceFolder).Where((x => x.EndsWith(@"_Datasets") || x.EndsWith(@"_Toolkit"))))
            {
                string datasetPath = Path.Combine(path, "DataSets");

                if (Directory.Exists(datasetPath))
                    CopyJsonInFoldersRecursively(datasetPath, targetFolder);
            }
        }


        /***************************************************/
        /**** Helper Methods                            ****/
        /***************************************************/

        private static void CopyJsonInFoldersRecursively(string sourceFolder, string targetFolder)
        {
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            //Copy all files
            string[] files = Directory.GetFiles(sourceFolder, "*.json");
            foreach (string file in files)
                File.Copy(file, Path.Combine(targetFolder, Path.GetFileName(file)), true);

            //Copy all sub folders
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
                CopyJsonInFoldersRecursively(folder, Path.Combine(targetFolder, Path.GetFileName(folder)));

        }

        /***************************************************/
    }
}
