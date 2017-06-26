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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MARC.EHRS.Visualization.Client.Silverlight.UI.Config;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace MARC.EHRS.Visualization.Client.Silverlight.UI
{
    public partial class MainPage : UserControl
    {
       
        // Configuration object
        private Configuration m_configuration;

       
        /// <summary>
        /// Create a new instance of the page
        /// </summary>
        public MainPage(IDictionary<String,String> parms)
        {
            InitializeComponent();

            // Process parameters
            string config;
            if (!parms.TryGetValue("config", out config))
                throw new ArgumentException("Could not initialize the visualizer application");

            // Assign handler
            Configuration.LoadComplete += new EventHandler<ConfigurationLoadedEventArgs>(Configuration_LoadComplete);
            Configuration.LoadAsync(config);
        }

        /// <summary>
        /// The configuration has been loaded
        /// </summary>
        void Configuration_LoadComplete(object sender, ConfigurationLoadedEventArgs e)
        {
            this.m_configuration = e.Configuration;

            foreach (var splogo in this.m_configuration.Sponsors)
                SponsorLogos.Children.Add(new Image()
                {
                    Source = new BitmapImage(new Uri(splogo)),
                    Stretch = Stretch.UniformToFill, 
                    MinHeight = 64
                });

            this.visContent.Configuration = e.Configuration;
        }

        private void visContent_Loaded(object sender, RoutedEventArgs e)
        {

        }

      

       

   

    }
}
