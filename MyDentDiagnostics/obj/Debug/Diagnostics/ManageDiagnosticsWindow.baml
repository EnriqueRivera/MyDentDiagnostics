   M S B A M L   `   `   `  ����  MyDentDiagnostics/    )MyDentDiagnostics.ManageDiagnosticsWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      -   5          �Name$  Window��5       � Title$ Administrar diagnósticos��5      $��528.06��5      $��391.701��6    � Icon$1 +/MyDentDiagnostics;component/Images/app.ico��6"    � WindowStartupLocation$ CenterScreen=�6U   .��5      � -   5         �Name$ 
LayoutRoot��5      .+�5	      &   System.Windows.Controls.DataGrid  -   5          Name$ dgDiagnostics��5	      $��4��6$      SelectionMode$ Single=�61   $��10,90.58,10,44q�6H      
IsReadOnly .    CanUserAddRows .  	   CanUserSortColumns	 . 
   CanUserDeleteRows
 .     AutoGenerateColumns .  ��6`   �� *ObservableData+   Columns 5
      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Nombre del diagnóstico��62      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.3* 6R   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6   �� *Name+6R     5      $ Fecha y hora de creación��6%   $
 0.3* 6g    6G   �� *CreatedDate+6g     5      $ Usuario��6%   $
 0.3* 66    5      `� 5       �� StringFormat$  {0} {1}��6'   .1�5      ��  �� Path$ User.FirstName�6&   �� 5      $ User.LastName�6&   5      5      5      �� 5      $	��113��6
   $	��Top=�6   $��10,49.393,0,0q�6.   $��27��6E   $
��Left=�6Q   $��Buscar diagnóstico:��6l   �� 5      -   5         ��Name$ txtSearchTerm��5      $
��Left=�6#   $��25.02��6>   $��128,51.373,0,0q�6M    �� TextWrapping$
 Wrap=�6e   $	��Top=�6y   $	��166��6�   
���   �$��1��6�   �� 5      -   5         ��Name$ 	btnSearch��5      $��299,49.062,0,0q�6    � Cursor$
 Hand��66   $
��Left=�6D   $��30��6_   $��30��6j   $	��Top=�6v   $��2��6�   .��5      �� $7��1/MyDentDiagnostics;component/Images/find_icon.png��6   $
��Fill=�6F   5      �� 5      -   5        $ 
btnViewAll��5      $��63.333��6   $	��Top=�6.   $��0,52.062,10,0q�6F   $��25��6]   $��Right=�6i   $��Ver todo��6�   $
 Hand��6�   $��3��6�   6�   �� 5      -   5        $ 	btnDelete��5      $��Eliminar��6   $��	0,0,10,10q�61   $
 Hand��6D   $��7��6R   $��29��6_   $��Bottom=�6k   $��Right=�6�   $��91��6�   6�   �� 5      -   5        $ btnEdit��5      $��Editar��6   $��
0,0,106,10q�6-   $
 Hand��6A   $��6��6O   $��29��6\   $��Bottom=�6h   $��Right=�6�   $��91��6�   6�   �� 5      -	   5        $ btnAdd��5      $��Agregar��6   $��
0,0,202,10q�6-   $
 Hand��6A   $��5��6O   $��29��6\   $��Bottom=�6h   $��Right=�6�   $��91��6�   6�   �� 5       -
   5         ��Name$ lblAreaName��5    
   $	��Top=�6   $��10,7.062,10,0q�67   $��27��6N   $�� ��6Z   $��Center=�6e   $
��Bold"�6�   5!      5"      