﻿#pragma checksum "..\..\..\..\..\Views\UserControl\YesNoMessageBox.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FC7B93B8956FF15D33E2F886A4C828B9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Windows.Themes;
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


namespace Miku.Client.Views.UserControl {
    
    
    /// <summary>
    /// YesNoMessageBox
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class YesNoMessageBox : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\..\..\Views\UserControl\YesNoMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Miku.Client.Views.UserControl.YesNoMessageBox UserControl;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\..\Views\UserControl\YesNoMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\..\Views\UserControl\YesNoMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button yesButton;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\..\Views\UserControl\YesNoMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button noButton;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\..\Views\UserControl\YesNoMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock messageTextBlock;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\..\Views\UserControl\YesNoMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button closeButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Miku.Client;component/views/usercontrol/yesnomessagebox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\UserControl\YesNoMessageBox.xaml"
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
            this.UserControl = ((Miku.Client.Views.UserControl.YesNoMessageBox)(target));
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.yesButton = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\..\..\Views\UserControl\YesNoMessageBox.xaml"
            this.yesButton.Click += new System.Windows.RoutedEventHandler(this.yesButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.noButton = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\..\..\Views\UserControl\YesNoMessageBox.xaml"
            this.noButton.Click += new System.Windows.RoutedEventHandler(this.noButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.messageTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.closeButton = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

