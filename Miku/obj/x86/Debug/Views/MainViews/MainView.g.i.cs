﻿#pragma checksum "..\..\..\..\..\Views\MainViews\MainView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1A4FC46F4169309620A0255843446690"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Miku.Client;
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


namespace Miku.Client.Views.MainViews {
    
    
    /// <summary>
    /// MainView
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class MainView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\..\..\Views\MainViews\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMinSize;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\..\Views\MainViews\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMaxSize;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\..\Views\MainViews\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExit;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\..\Views\MainViews\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Viewport3D viewPort3D;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\..\Views\MainViews\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.Media3D.Model3DGroup model3DGroup;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\..\Views\MainViews\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.Media3D.GeometryModel3D center;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\..\Views\MainViews\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.Media3D.GeometryModel3D right;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\..\Views\MainViews\MainView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.Media3D.GeometryModel3D left;
        
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
            System.Uri resourceLocater = new System.Uri("/Miku.Client;component/views/mainviews/mainview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\MainViews\MainView.xaml"
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
            
            #line 5 "..\..\..\..\..\Views\MainViews\MainView.xaml"
            ((Miku.Client.Views.MainViews.MainView)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\..\..\Views\MainViews\MainView.xaml"
            ((Miku.Client.Views.MainViews.MainView)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\..\..\Views\MainViews\MainView.xaml"
            ((Miku.Client.Views.MainViews.MainView)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Window_MouseMove);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnMinSize = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\..\..\Views\MainViews\MainView.xaml"
            this.btnMinSize.Click += new System.Windows.RoutedEventHandler(this.btnMinSize_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnMaxSize = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\..\..\Views\MainViews\MainView.xaml"
            this.btnMaxSize.Click += new System.Windows.RoutedEventHandler(this.btnMaxSize_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnExit = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\..\..\Views\MainViews\MainView.xaml"
            this.btnExit.Click += new System.Windows.RoutedEventHandler(this.btnExit_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.viewPort3D = ((System.Windows.Controls.Viewport3D)(target));
            return;
            case 6:
            this.model3DGroup = ((System.Windows.Media.Media3D.Model3DGroup)(target));
            return;
            case 7:
            this.center = ((System.Windows.Media.Media3D.GeometryModel3D)(target));
            return;
            case 8:
            this.right = ((System.Windows.Media.Media3D.GeometryModel3D)(target));
            return;
            case 9:
            this.left = ((System.Windows.Media.Media3D.GeometryModel3D)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
