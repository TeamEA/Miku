﻿#pragma checksum "..\..\..\..\..\..\Views\ActionViews\SubViews\MainTipPanel.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "03CA44291A1EE8D1481832131CDF6E1A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Miku.Client.Helpers;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Miku.Client.Views.ActionViews {
    
    
    /// <summary>
    /// MainTipPanel
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class MainTipPanel : System.Windows.Controls.StackPanel, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\..\..\Views\ActionViews\SubViews\MainTipPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox 录制对象;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\..\..\Views\ActionViews\SubViews\MainTipPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdbMouseAndKeyboard;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\..\..\Views\ActionViews\SubViews\MainTipPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdbMouse;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\..\..\Views\ActionViews\SubViews\MainTipPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdbKeyboard;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Miku.Client;component/views/actionviews/subviews/maintippanel.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\Views\ActionViews\SubViews\MainTipPanel.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.录制对象 = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 2:
            this.rdbMouseAndKeyboard = ((System.Windows.Controls.RadioButton)(target));
            
            #line 20 "..\..\..\..\..\..\Views\ActionViews\SubViews\MainTipPanel.xaml"
            this.rdbMouseAndKeyboard.Click += new System.Windows.RoutedEventHandler(this.rdb_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.rdbMouse = ((System.Windows.Controls.RadioButton)(target));
            
            #line 21 "..\..\..\..\..\..\Views\ActionViews\SubViews\MainTipPanel.xaml"
            this.rdbMouse.Click += new System.Windows.RoutedEventHandler(this.rdb_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.rdbKeyboard = ((System.Windows.Controls.RadioButton)(target));
            
            #line 22 "..\..\..\..\..\..\Views\ActionViews\SubViews\MainTipPanel.xaml"
            this.rdbKeyboard.Click += new System.Windows.RoutedEventHandler(this.rdb_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

