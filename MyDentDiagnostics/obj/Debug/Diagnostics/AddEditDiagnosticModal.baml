   M S B A M L   `   `   `  ����  MyDentDiagnostics.    (MyDentDiagnostics.AddEditDiagnosticModal   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      -   5          �Name$  Window��5       � Title$ Agregar diagnóstico��5      $	��650��5      $	��483��6   $	��650��6    � Icon$1 +/MyDentDiagnostics;component/Images/app.ico��6*    � WindowStartupLocation$ CenterScreen=�6]   $	��483��6�   $	��650��6�   .��5	      � -   5         �Name$ 
LayoutRoot��5	       � ColumnDefinitions 5
      �� 5      $	��15* �6   �� 5      $��8* �6   .+�5      �� -   5         ��Name$ lblReminderMessage��5   
   $��Descripción:��6&   $��25.672��6=   $��12,81.03,10,0q�6M   $	��Top=�6d   $
��Bold"�6|   �� 5      -   5         ��Name$ txtDiagnosticDescription��5      $��10.5,107.702,9.5,54.963q�6.   $��Italic$�6O   " �� VerticalScrollBarVisibility$
 Auto=�6b   	 �� TextWrapping$
	 Wrap=�6�   
 �� AcceptsReturn
 . $��1q�6�   
���   �6�   �� 5      -   5        $ lblTreatmentName��5   
   $��Nombre del diagnóstico:��6$   $��27��6F   $��9.5,10,10,0q�6R   $	��Top=�6g   $
��Bold"�6   �� 5      -   5        $ txtDiagnosticName��5      $��27��6'   $��9.5,37,9.5,0q�63   $
	 Wrap=�6I   $	��Top=�6]   $��1��6u   $��1q�6�   
���   � �� TextAlignment$ Center=�6�   �� 5      -   5         ��Name$ btnAddUpdate��5      $��Agregar��6!   $��
0,0,106,10q�63    � Cursor$
 Hand��6G   $��2��6U   $��29��6b   $��Bottom=�6n   $��Right=�6�   $��91��6�   $��1��6�   �� 5      -   5        $ 	btnCancel��5      $��Cancelar��6   $��	0,0,10,10q�61   $
 Hand��6D   $��3��6R   $��29��6_   $��Bottom=�6k   $��Right=�6�   $��91��6�   $��1��6�   �� 5      -	   5        $ lblTreatmentName_Copy��5   
   $��Fotografías��6)   $��27��6?   $��77,19.05,0,0q�6K   $	��Top=�6a   $��Center=�6y   $
��Left=�6�   $��80��6�   $��1��6�   $
��Bold"�6�   �� 5      
���   �$��1q�6   $��27,67.5,0,0q�63   $
��Left=�6H   $	��130��6c   $	��130��6o   $	��Top=�6|   $��1��6�   ._�5      �� -
   5         ��Name$ diagnosticImage1��5      $��	0,0,-1,-1q�6%   $
 Hand��68   6F   5      �� 5      
���   �$��1q�6   $
��Left=�63   $��27,239.642,0,0q�6N   $	��130��6f   $	��130��6r   $	��Top=�6   $��1��6�   ._�5      �� -   5        $ diagnosticImage2��5      $��	0,0,-1,-1q�6%   $
 Hand��68   6F   5      �� 5      -   5        $ btnSearchImage1��5      $��162,91.03,0,0q�6$   $
 Hand��6;   $
��Left=�6I   $��40��6d   $��2��6o   $��40��6|   $	��Top=�6�   $��1��6�    � ToolTip$ Examinar...��6�   .��5      �� $@��:/MyDentDiagnostics;component/Images/folder_images_icon.png��6   $
��Fill=�6O   5      �� 5      -   5        $ btnRemoveImage1��5      $��162,136.03,0,0q�6$   $
 Hand��6<   $
��Left=�6J   $��40��6e   $��40��6p   $	��Top=�6|   $��2��6�   $��1��6�   $ Remover fotografía��6�   .��5      �� $9��3/MyDentDiagnostics;component/Images/remove_icon.ico��6   $
��Fill=�6H   5       �� 5!      -   5        $ btnSearchImage2��5!      $��162,264.642,0,0q�6$   $
 Hand��6=   $
��Left=�6K   $��40��6f   $��40��6q   $	��Top=�6}   $��2��6�   $��1��6�   $ Examinar...��6�   .��5"      �� $@��:/MyDentDiagnostics;component/Images/folder_images_icon.png��6   $
��Fill=�6O   5#      �� 5$      -   5        $ btnRemoveImage2��5$      $��162,309.642,0,0q�6$   $
 Hand��6=   $
��Left=�6K   $��40��6f   $��40��6q   $	��Top=�6}   $��2��6�   $��1��6�   $ Remover fotografía��6�   .��5%      �� $9��3/MyDentDiagnostics;component/Images/remove_icon.ico��6   $
��Fill=�6H   5&      5'      5(      