   M S B A M L   `   `   `  ���� F  BMiku.Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Miku.ClientMiku.Client  4    .Miku.Client.Views.ActionViews.ActionRecordView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     +localclr-namespace:Miku.Client     -     � Title$  ActionRecordView��$	��440��$	��640��   	主界面#��[   � WindowStyle$
 None=� � WindowStartupLocation$ Manual=� � 
ResizeMode$ NoResize=� � Left �� %   Miku.Client.Properties.Settings   Default �� Source# Z  �� Path$ MainWndLeft� �� Mode$ TwoWay=�
	 � Top	 �� # Z $ 
MainWndTop�$ TwoWay=�.��� .+��� $��26��$��Right=�$��0,0,10,0q�$	��Top=�$��78��
 �� Orientation$
 
Horizontal=�.+��� -    ��Name$ 
btnMinSize��$��23��$��26��  MinStyle#���  �� -   $ 
btnMaxSize��$��23��$��26��  MaxStyle#���  �� -   $ btnExit��$��23��$��26��  	ExitStyle#���  � $	��408��$	��620��$��10,0,0,0q�$
��Left=�.+��� -   $��录制对象��$��录制对象��$	��184��$
��Left=�$��0,74,0,0q�$	��Top=�$	��352��$��20(�$��迷你简小标宋)�����  �� EndPoint$ 1,0.5&� �� 
StartPoint$ 0,0.5&�.���  � Color$ 	OrangeRed�� � Offset$ 0Q�� $ White��$ 1Q�.��� $	��137�� � ColumnDefinitions �� $	��44* ��� $
��296* �.+�	� -   $��rdbMouseAndKeyboard��$��  Mouse and Keyboard�� d� 	IsChecked$
 TrueY� 	� 	GroupName$ gbActionStrategy��$��6,88,0,6q�$��2��	� -   $��rdbMouse��$��  Mouse ��$ gbActionStrategy��$��	6,6,-6,90q�$��2��	� -   $��rdbKeyboard��$��  Keyboard ��$ gbActionStrategy��$��
6,45,-6,55q�$��2���� -	   $ btnHelp��$��80��$
��Left=�$��382,152,0,0q� � ToolTip$ 帮助��$	��Top=�$��81��  HelperStyle#���   � Cursor$
 Help���� -
   $ 
btnContrct��$��81��$
��Left=�$��12,0,0,0q�$ 联系我们��$	��Top=�$��81��  ContrctStyle#���  �� -   $��功能按钮��$��功能按钮��$	��153��$
��Left=�$��	0,256,0,0q�$	��Top=�$	��620��$��20(�$��迷你简小标宋)����� $ 1,0.5&�$ 0,0.5&�.��� $ 	OrangeRed��$ 0Q�� $ White��$ 1Q�.���� $	��123��$	��610��$
 
Horizontal=�.+��� -    ��Name$ 	Recording��$��57��$	��200��.+��� -   $��btnStartRecord��$��	4,0,2,154q�  RecordingStyle#���  $ 动作录制�� J� 	AllowDrop . $	��126�� �� Left$ 53��
 �� Top$ 0��$��49���� $��55��$
��Left=�$��2,0,0,0q�
������ $	��Top=�$��55��  RecordLable#��[ $ Cross���� -   $ Playback��$��57��$	��200��$��0,30,0,0q�.+��� -   $��btnSaveFile��$��	6,147,0,0q�  	SaveStyle#���  $ 动作保存��$ 48��$
 -147��$	��126���� $��55��$
��Left=�
������ $	��Top=�$��55�� 	 	SaveLable#��[	 $ Cross���� -   $ 
SaveRecord��$��57��$	��200��$��0,60,0,0q�.+��� -   $��btnPlayBack��$��	6,74,0,80q� 
 PlayingStyle#��� 
 $ 动作回放��$ 51��$	 -74��$	��126���� $��55��
������ $��55��  	PlayLable#��[ $
��Left=�$	��Top=�$ Cross��$ 2���� -   $ btnrightExpand��$
��Left=�$��580,177,0,0q�$	��Top=�  
rightStyle#���  � -    �Name$ Expand��$	��200��$��0,0,0,0q�$��Right=�$��Bottom=�$	��380��.+��� -   $��记录列表��$��记录列表��$��Stretch=�$��	10,0,0,50q�$��Stretch=�$��20(�$��迷你简小标宋)� 
 Expand#���  ���� $ 1,0.5&�$ 0,0.5&�.��� $ 	OrangeRed��$ 0Q�� $ White��$ 1Q�.���� -    ��Name$ lbxRecordList��$��Stretch=�$��Stretch=�� $��Stretch=�$��Bottom=�.+��� -   $ btnleftExpand��$��Center=�$��Bottom=�  	leftStyle#���  