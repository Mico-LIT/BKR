   M S B A M L   `   `   `  ����?  ;WKR2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WKR2WKR2  5   	       WKR2.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:WKR2     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5	      � -   5        $��
GLABN_GRID��5	       � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      �� 5      $
��0.4* �6    � ColumnDefinitions 5   
   �� 5      �� 5      $	��100 �6   .+�5   
   o� .@�5      m� $��Файл��6   .@�5      m� -   5        $��Открть��5      6:   m� 5      $��
Выход��6   6,   5      m� 5      -   5        $��Предпросмотр��5      6D   m� 5      -   5        $��Печать��5      67   m� 5      -   5        $
��Save��5      68   m� 5      -   5        $
��Json��5      65   5      �� 5!   
   -   5        $��image��5!      $��1��6   6C   �� 5#   
   -   5        $��	grid_imag��5#      $��1��6   ��6/   ��  �� ElementName$ image�� �� Path$ ActualHeight���5$      �� $ image��$ ActualWidth�5%      �� 5'   
   -	   5        $��point��5'   9   $��1��6   $��1��6   $	��200��6-   6H   &   System.Windows.Controls.DataGrid  5)   
   -
   5        $	��d12��5)       $��2��6   .@�6G       Columns 5+      4  .System.Windows.Controls.DataGridTemplateColumn  5,      ,  &System.Windows.Controls.DataGridColumn   Header$ Печать��6)      CellTemplate 5-      �� 5.      .R�5/      �� .+�50   "   �� -   5        $��Печать��50   )   6:   51      52      54        55      $ Печать��6)    56      �� 57      .R�58      �� $��15,2,0,0q�6'   59      5;      5=      5V      5W      