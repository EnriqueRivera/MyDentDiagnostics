   M S B A M L   `   `   `  ����L  HMyDentDiagnostics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:MyDentDiagnosticsMyDentDiagnostics  5   	   4    .MyDentDiagnostics.AddSpecificProgressNoteModal   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:MyDentDiagnostics     5   	   $	��665��5      $��1444.5��6     � Icon$1  +/MyDentDiagnostics;component/Images/app.ico��6    � WindowStartupLocation$ CenterScreen=�6Q    � WindowState$ 	Maximized=�6v   $	��665��6�   .��5	      � -   5         �Name$ 
LayoutRoot��5	      .+�5
   
   �� $��0,0,2,65.123q�6   $
��Auto=�6-   .��5      � .+�5      �� -   5         ��Name$ lblTitle��5      $��Titulo��6*   $��27��6;   $��305.941,10,305.941,0q�6G   $	��Top=�6e   $
��Bold"�6}   $��Center=�6�   �� 5      -   5        $ lblTitle_Copy��5      $��Procedimiento realizado:��6/   $��27��6R   $��
10,42,10,0q�6^   $	��Top=�6r   $
��Bold"�6�   �� 5      -   5        $ lblTitle_Copy1��5      $5��/Se dan las siguientes indicaciones al paciente:��60   $��27��6j   $��10,345.5,10,0q�6v   $	��Top=�6�   $
��Bold"�6�   �� 5      -   5        $ lblTitle_Copy2��5      $��Medicamentos:��60   $��27��6H   $��10,616.542,10,0q�6T   $	��Top=�6m   $
��Bold"�6�   �� 5      -   5         ��Name$ txtMedicaments��5      $��10,643.542,10,601.125q�62   $��Italic$�6Q   " �� VerticalScrollBarVisibility$
 Auto=�6d    �� TextWrapping$
 Wrap=�6�    �� AcceptsReturn . $��1q�6�   
���   �6�   �� 5      -   5        $ lblTitle_Copy3��5      $��
Hallazgos:��60   $��10,0,10,554.835q�6E   $
��Bold"�6^   $��27��6p   $��Bottom=�6|   �� 5      -   5        $ txtFoundOut��5      $��10,793.542,10,0q�6/   $��Italic$�6H   $
 Auto=�6[   $
 Wrap=�6~    . $��1q�6�   
���   �$��101.335��6�   $	��Top=�6�   �� 5      -	   5        $ lblTitle_Copy4��5      $��Pronósticos:��60   $��27��6G   $��10,913.542,10,0q�6S   $	��Top=�6l   $
��Bold"�6�   �� 5      -
   5        $ txtPronostics��5      $��10,940.542,10,0q�61   $��Italic$�6J   $
 Auto=�6]   $
 Wrap=�6�    . $��1q�6�   
���   �$��101.335��6�   $	��Top=�6�   �� 5      -   5        $ lblTitle_Copy5��5      $��Diagnóstico:��60   $��27��6G   $��10,1062.042,10,0q�6S   $	��Top=�6m   $
��Bold"�6�   �� 5      -   5        $ txtDiagnostic��5      $��10,1089.042,10,0q�61   $��Italic$�6K   $
 Auto=�6^   $
 Wrap=�6�    . $��1q�6�   
���   �$��101.335��6�   $	��Top=�6�   �� 5      -   5        $ lblTitle_Copy6��5      $/��)Tratamientos a realizar la próxima cita:��60   $��27��6c   $��10,1210.042,10,0q�6o   $	��Top=�6�   $
��Bold"�6�   �� 5      -   5        $ txtNextTreatment��5      $��10,1237.042,10,0q�64   $��Italic$�6N   $
 Auto=�6a   $
 Wrap=�6�    . $��1q�6�   
���   �$��101.335��6�   $	��Top=�6�   �� 5      $��261.667��6   $��
10,69,10,0q�60   $	��Top=�6D   $
��Auto=�6\   .��5      � -   5        $ grdProcedure��5      5      �� 5      $��239.042��6   $��10,372.5,10,0q�60   $	��Top=�6G   $
��Auto=�6_   .��5      � -   5        $ grdIndications��5      5      5      5       �� 5!   
   -   5        	 ��Name$	 btnAdd��5!      $��Agregar��6!   $��
0,0,106,10q�63   
 � Cursor$

 Hand��6G   $
��1000��6U   $��Right=�6e   $��91��6�   $��29.93��6�   $��Bottom=�6�   �� 5"   
   -   5        $	 	btnCancel��5"      $��Cancelar��6$   $��	0,0,10,10q�67   $

 Hand��6J   $
��1001��6X   $��Right=�6h   $��91��6�   $��29.93��6�   $��Bottom=�6�   5#      5%      