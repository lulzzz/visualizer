﻿/*
 * Copyright 2012-2017 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 2012-6-15
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MARC.EHRS.VisualizationServer.Notifier.Configuration
{
    /// <summary>
    /// Handles the configuration for the notification service
    /// </summary>
    public class ConfigurationHandler : IConfigurationSectionHandler
    {

        /// <summary>
        /// Specifies the address to bind to
        /// </summary>
        public IPAddress BindAddress { get; set; }

        /// <summary>
        /// Specifies the port to bind to
        /// </summary>
        public int BindPort { get; set; }

        /// <summary>
        /// Enables the CAP Server
        /// </summary>
        public bool EnableCAPServer { get; set; }

        /// <summary>
        /// The policy file for the CAP server
        /// </summary>
        public string CapServerPolicyFile { get; set; }

        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Create the configuration section
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {

            if (section.Attributes["address"] != null)
                BindAddress = IPAddress.Parse(section.Attributes["address"].Value);
            else
            {
                Trace.TraceWarning("Couldn't find configuration for address, binding to *");
                BindAddress = IPAddress.Any;
            }

            if (section.Attributes["port"] != null)
                BindPort = Int32.Parse(section.Attributes["port"].Value);
            else
            {
                Trace.TraceWarning("Couldn't find configuration for bind port, default to 4530");
                BindPort = 4530;
            }

            if(section.Attributes["enableCAPServer"] != null)
                EnableCAPServer = Boolean.Parse(section.Attributes["enableCAPServer"].Value);

            if (section.Attributes["capServerPolicyFile"] != null)
            {
                CapServerPolicyFile = section.Attributes["capServerPolicyFile"].Value;
                if (!File.Exists(CapServerPolicyFile))
                    CapServerPolicyFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), CapServerPolicyFile);
            }

            // Sanity check
            if (BindPort < 4502 || BindPort > 4532)
                Trace.TraceWarning("Binding port '{0}' may prohibit Silverlight applications from connecting");

            return this;
        }

        #endregion
    }
}
