   M S B A M L   `   `   `  ����F  BMiku.Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Miku.ClientMiku.Client  5   	   4    .Miku.Client.Views.ActionViews.ActionRecordView   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   +localclr-namespace:Miku.Client     5   	   -   5          � Title$  ActionRecordView��5   	   $	��440��6"   $	��640��6/      	主界面#��[  6;    � WindowStyle$
 None=�6]    � WindowStartupLocation$ Manual=�5   3    � 
ResizeMode$ NoResize=�5   k    � Left 5   	   �� %   Miku.Client.Properties.Settings   Default �� Source# Z  �� Path$ MainWndLeft� �� Mode$ TwoWay=�
	 � Top	 5   	   �� # Z $ 
MainWndTop�$ TwoWay=�.��5      � .+�5	   
   �� $��26��6   $��Right=�6!   $��0,0,10,0q�6=   $	��Top=�6P   $��78��6h   
 �� Orientation$
 
Horizontal=�6s   .+�5
      �� -   5         ��Name$ 
btnMinSize��5
      $��23��6*   $��26��66     MinStyle#���  6A   6d   �� 5      -   5        $ 
btnMaxSize��5      $��23��6*   $��26��66     MaxStyle#���  6A   6d   �� 5      -   5        $ btnExit��5      $��23��6'   $��26��63     	ExitStyle#���  6>   6b   5      � 5   
   $	��408��6   $	��620��6   $��10,0,0,0q�6(   $
��Left=�6:   .+�5      �� -   5        $��录制对象��5   _   $��录制对象��6   $	��184��6%   $
��Left=�62   $��0,74,0,0q�6M   $	��Top=�6k   $	��352��6�   $��20(�6�   $��迷你简小标宋)�6�   ��5      �� 5       �� EndPoint$ 1,0.5&�6*    �� 
StartPoint$ 0,0.5&�6;   .��5      �  � Color$ 	OrangeRed��6'    � Offset$ 0Q�69   � 5      $ White��6'   $ 1Q�65   5      .��5      � $	��137��6    � ColumnDefinitions 5      �� 5      $	��44* �6+   �� 5      $
��296* �6+   .+�5      	� -   5        $��rdbMouseAndKeyboard��5   A   $��  Mouse and Keyboard��6"    d� 	IsChecked$
 TrueY�6]    	� 	GroupName$ gbActionStrategy��6n   $��6,88,0,6q�6�   $��2��6�   	� 5      -   5        $��rdbMouse��5   5   $��  Mouse ��6"   $ gbActionStrategy��6G   $��	6,6,-6,90q�6v   $��2��6�   	� 5      -   5        $��rdbKeyboard��5   8   $��  Keyboard ��6"   $ gbActionStrategy��6M   $��
6,45,-6,55q�6|   $��2��6�   5      5      �� 5       -	   5        $ btnHelp��5       $��80��6&   $
��Left=�62   $��382,152,0,0q�6M    � ToolTip$ 帮助��6b   $	��Top=�5!      $��81��6)     HelperStyle#���  64    � Cursor$
 Help��6p   �� 5"      -
   5        $ 
btnContrct��5"      $��81��6)   $
��Left=�65   $��12,0,0,0q�6P   $ 联系我们��6c   $	��Top=�5#      $��81��6)     ContrctStyle#���  64   6[   �� 5$      -   5        $��功能按钮��5$   `   $��功能按钮��6   $	��153��6%   $
��Left=�62   $��	0,256,0,0q�6M   $	��Top=�6l   $	��620��6�   $��20(�6�   $��迷你简小标宋)�6�   ��5%      �� 5&      $ 1,0.5&�6*   $ 0,0.5&�6;   .��5'      � $ 	OrangeRed��6'   $ 0Q�69   � 5(      $ White��6'   $ 1Q�65   5)      .��5+      �� $	��123��6   $	��610��6*   $
 
Horizontal=�66   .+�5,      �� -   5         ��Name$ 	Recording��5,      $��57��61   $	��200��6=   .+�5-      �� -   5        $��btnStartRecord��5-   !   $��	4,0,2,154q�67     RecordingStyle#���  5.      $ 动作录制��6F    J� 	AllowDrop . $	��126��6f    �� Left$ 53��6r   
 �� Top$ 0��6�   $��49��6�   �� 5/      $��55��6$   $
��Left=�60   $��2,0,0,0q�6K   
������ $	��Top=�50       $��55��68     RecordLable#��[ 6C   $ Cross��6g   51      �� 52      -   5        $ Playback��52      $��57��6/   $	��200��6;   $��0,30,0,0q�6G   .+�53      �� -   5        $��btnSaveFile��53   !   $��	6,147,0,0q�64     	SaveStyle#���  54      $ 动作保存��6A   $ 48��6P   $
 -147��6a   $	��126��6s   �� 55      $��55��6$   $
��Left=�60   
������ $	��Top=�56       $��55��68    	 	SaveLable#��[	 6C   $ Cross��6e   57      �� 58      -   5        $ 
SaveRecord��58      $��57��61   $	��200��6=   $��0,60,0,0q�6I   .+�59      �� -   5        $��btnPlayBack��59   !   $��	6,74,0,80q�64    
 PlayingStyle#��� 
 6a   $ 动作回放��6�   $ 51��6�   $	 -74��6�   $	��126��6�   �� 5:      $��55��6$   
������ $��55��6E     	PlayLable#��[ 6P   $
��Left=�6r   $	��Top=�6�   $ Cross��6�   $ 2��6�   5;      5<      5=      �� 5>      -   5        $ btnrightExpand��5>      $
��Left=�6-   $��580,177,0,0q�6H   $	��Top=�5?        
rightStyle#���  6*   6O   5@      � 5A   
   -   5         �Name$ Expand��5B      $	��200��5A      $��0,0,0,0q�6   $��Right=�6,   $��Bottom=�6I   $	��380��6d   .+�5C      �� -   5        $��记录列表��5C   X   $��记录列表��6   $��Stretch=�6&   $��	10,0,0,50q�6D   $��Stretch=�6d   $��20(�5D      $��迷你简小标宋)�6%    
 Expand#���  69   ��5E      �� 5F      $ 1,0.5&�6*   $ 0,0.5&�6;   .��5G      � $ 	OrangeRed��6'   $ 0Q�69   � 5H      $ White��6'   $ 1Q�65   5I      .��5K      �� -   5         ��Name$ lbxRecordList��5K      $��Stretch=�61   $��Stretch=�6M   5M      5O      � 5P      $��Stretch=�6   $��Bottom=�61   .+�5Q      �� -   5        $ btnleftExpand��5Q      $��Center=�60   $��Bottom=�6M     	leftStyle#���  5R      6[   5S      5T      5U      5V      