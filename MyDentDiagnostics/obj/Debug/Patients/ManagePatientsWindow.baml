   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5       MyDentDiagnostics,   &MyDentDiagnostics.ManagePatientsWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      9System&clr-namespace:System;assembly=mscorlib     5      -   5          �Name$  Window��5       � Title$ Administrar pacientes��5      $	��652��5      $��504.955��6    � Icon$1 +/MyDentDiagnostics;component/Images/app.ico��6    � WindowStartupLocation$ CenterScreen=�6R   .��5
      � -   5         �Name$ 
LayoutRoot��5
      .+�5      �� -   5         ��Name$ 
tcPatients��5      $��10,55.353,10,44q�6#    �� SelectedIndex$ 0��6<   .@�5      �� $��Nota inicial dental��6   .��5      � ���	#FFE5E5E5.+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ 
dgPatients��5      $��10q�6$      
IsReadOnly . 	   CanUserSortColumns	 . 
   AutoGenerateColumns
 .     CanUserAddRows .     CanUserDeleteRows .  $��4��5   !      SelectionMode$ Single=�6.   ��5      �� *ObservableData+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Expediente No.��61      Width5  /System.Windows.Controls.DataGridLengthConverter$ 0.17* 6o   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6I   �� *AssignedPatientId+6o     5   	   $ Nombre��6   $
 0.3* 6I    6,   �� *
FullName+6I     5      $ Fecha y hora de creación��61   $
 0.3* 6s    6S   �� *CreatedDate+6s     5      $ Usuario��61   $
 0.3* 6B    5   "   `� 5   &    �� StringFormat$  {0} {1}��63   .1�5   *   ��  �� Path$ User.FirstName�62   �� 5   *   $ User.LastName�62   5   '   5      5      5      5       �� 5!      $��Nota inicial DCM��6   .��5"      � ���	#FFE5E5E5.+�5#        -   5        $ dgPatientsDcm��5#      $��10q�6'    . 	 . 
 .   .   .  $��4��5%   !   ��5$      �� *ObservableData+ 5&        5'   	   $ Nombre��6   $
 0.3* 6I    6,   �� *
FullName+6I     5(      $ Fecha y hora de creación��61   $
 0.3* 6s    6S   �� *CreatedDate+6s     5)      $ Usuario��61   $
 0.3* 6B    5*   "   `� 5+   &   $  {0} {1}��63   .1�5,   *   �� $ User.FirstName�62   �� 5-   *   $ User.LastName�62   5.   '   50      52      53      54      55      �� 56      -   5         ��Name$ txtSearchTerm��56      $
��Left=�6#   $��25.02��6>   $��189.5,12.98,0,0q�6M    �� TextWrapping$
 Wrap=�6f   $	��Top=�6z   $	��166��6�   
���   �$��1��6�   �� 57      -   5         ��Name$ 	btnSearch��57      $��360.5,10.5,0,0q�6    � Cursor$
 Hand��66   $
��Left=�6D   $��30��6_   $��30��6j   $	��Top=�6v   $��2��6�   .��58      �� $7��1/MyDentDiagnostics;component/Images/find_icon.png��6   $
��Fill=�6F   59      �� 5:      -   5        $ 
btnViewAll��5:      $��63.333��6   $	��Top=�6.   $��0,14.5,10,0q�6F   $��25��6[   $��Right=�6g   $��Ver todo��6�   $
 Hand��6�   $��3��6�   6�   �� 5;   
   -	   5        $ btnAdd��5;      $��Agregar��6!   $��	0,0,10,10q�63   $
 Hand��6F   $��6��6T   $��29��6a   $��Bottom=�6m   $��Right=�6�   $��91��6�   6�   �� 5<      -
   5        $ btnViewNotes��5<      $��	Ver notas��6!   $��	10,0,0,10q�65   $
 Hand��6H   $��6��6V   $��29��6c   $��Bottom=�6o   $
��Left=�6�   $��91��6�   6�   �� 5=      -   5        $ btnExportToPdf��5=      $��Exportar a PDF��6#   $��
106,0,0,10q�6<   $
 Hand��6P   $��6��6^   $��29��6k   $��Bottom=�6w   $
��Left=�6�   $	��104��6�   6�   �� 5>      -   5         ��Name$ cbFilter��5>      $��26��6   $��	84,12,0,0q�6+   $	��Top=�6>   $��Normal"�6V   $��1��6j   $
��Left=�6w   $��100.5��6�   $ 0��6�   .@�5?      ��
Nombre��6   5@      �� 5A      $��69��6
   $	��Top=�6   $��	10,10,0,0q�6-   $��27��6@   $
��Left=�6L   $��Buscar por:��6g   5B      5C      