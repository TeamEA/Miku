   M S B A M L   `   `   `  ����F  BMiku.Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:Miku.Client.Views.ActionViewsMiku.Client.Views.ActionViews  5   	   h dMicrosoft.Expression.Interactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions'Microsoft.Expression.Interactivity.Core 5   	   )clr-namespace:Miku.ClientMiku.Client  5	   	   .    (Miku.Client.Views.ActionViews.ActionView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   DlocalControl+clr-namespace:Miku.Client.Views.ActionViews     5   	   oic`clr-namespace:Microsoft.Expression.Interactivity.Core;assembly=Microsoft.Expression.Interactions     5   	   Bei9http://schemas.microsoft.com/expression/2010/interactions  5   	   =ee4http://schemas.microsoft.com/expression/2010/effects  5   	   %localclr-namespace:Miku.Client  5	   	   -   5          � Title$  
ActionView��5   	   $	��440��6   $	��640��6)      	主界面#��[  65    � WindowStyle$
 None=�6W    � WindowStartupLocation$ Manual=�5	   3    � Icon$6 0/Miku.Client;component/Assets/Images/Actions.ico��5   	    � 
ResizeMode$ CanMinimize=�6A    � Left 5
   	   �� %   Miku.Client.Properties.Settings   Default �� Source# Z  �� Path$ MainWndLeft�	 �� Mode$	 TwoWay=�

 � Top
 5   	   �� # Z $ 
MainWndTop�$	 TwoWay=� � TaskbarItemInfo 5      *  $System.Windows.Shell.TaskbarItemInfo  5   
   -   5        taskbarInfo��   Description$ 点控动作录制��5   0      Overlay$7 1/Miku.Client;component/Assets/Images/M_image1.png��6E      ProgressState$ Indeterminate=�6�      ProgressValue$ 50Q�6�   .��5      � -   5         �Name$ 
LayoutRoot��5      '  !System.Windows.VisualStateManager   VisualStateGroups 5   
   %  System.Windows.VisualStateGroup  5      -   5        VisualStateGroup��H  BMicrosoft.Expression.Interactivity.Core.ExtendedVisualStateManager   UseFluidLayout .    States. 5         System.Windows.VisualState  -   5        
Select��   
Storyboard. 5      �� .��5      T�  �� TargetProperty$ (UIElement.Visibility)�6<    �� 
TargetName$ RecordWindow��6o   ..�5   "   i�  R� KeyTime$ 0:0:0��69     System.Windows.Visibility   Hidden R� Value# Z 6I   5      T� 5      $ (UIElement.Visibility)�68   $ mainTipPanel��6k   ..�5      i� $ 0:0:0��65      Visible# Z 6E   5      5      5        5      -   5        
Record��. 5      �� .��5       T� $ (UIElement.Visibility)�68   $ RecordWindow��6k   ..�5!      i� $ 0:0:0��65   # Z 6E   5"      T� 5#      $ (UIElement.Visibility)�68   $ mainTipPanel��6k   ..�5$   "   i� $ 0:0:0��69   # Z 6I   5%      5&      5'      5B       � RowDefinitions 5D   
   �� 5E      $	��25* �6   �� 5F      $	��55* �6   �� 5G      $
��321* �6   .+�5I   
   �� $��Right=�6   $��0,0,10,0q�61   $	��Top=�6D   $��52��6\    �� Orientation$ 
Horizontal=�6g   .+�5J      �� -   5         ��Name$ 
btnMinSize��5J      $��23��6*   $��26��66     MinStyle#���  6A   6d   �� 5K      -   5        $ btnExit��5K      $��23��6'   $��26��63     	ExitStyle#���  6>   6b   5L      0   *Miku.Client.Views.ActionViews.MainTipPanel  5M   
   -	   5          Name$ mainTipPanel��5M   $   $��2��6;   6K   1	   +Miku.Client.Views.ActionViews.MouseRecorder	  5N   
   -
   5         	 Name$ RecordWindow��5N   %   $��2��6;   6J   5O      5P      