   M S B A M L   `   `   `  ����L  HMyDentDiagnostics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullk:clr-namespace:MyDentDiagnostics.Patients.ProcedureControls,MyDentDiagnostics.Patients.ProcedureControls  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      M    GMyDentDiagnostics.Patients.ProcedureControls.ProtesisParcialFijaControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Llocal:clr-namespace:MyDentDiagnostics.Patients.ProcedureControls     5      9System&clr-namespace:System;assembly=mscorlib     5      $	��635��5   3   $	��256��6?   .��5	      � -   5          �Name$  	gdContent��5	      .+�5   
   �� -   5         ��Name$ lblFullName_Copy227��5      $#��Con el paciente en posición ��6-   $
��Left=�6T   $	��Top=�6o   $��25.96��6�   $��156.804��6�   �� 5   
   -   5         ��Name$ 
cbPosicion��5      $��26��6'   $	��Top=�63   $��Normal"�6K   $
��Left=�6_   $��138.838��6z    �� SelectedIndex$ 0��6�   $��156.804,2,0,0q�6�   .@�5      ��decúbito dorsal��6-   ��5      
Fowler��6$   ��5      semi Fowler��6)   ��5      Trendelenburg��6+   ��5      Trendelenburg invertida��65   5      �� 5   
   -   5        $ lblFullName_Copy��5      $"��, bajo anestesia mucosa con ��6*   $	��Top=�6Q   $��25.96��6i   $��295.642,0,177.554,0q�6x   �� 5   
   -   5         ��Name$ txtAnestesia��5      $��27��6(    �� TextWrapping$
 Wrap=�64   $	��Top=�6H   $��1q�6`   
���   � �� TextAlignment$ Center=�6�   $��176.467��6�   $��Right=�6�   $��0,1,1.087,0q�6�   �� 5   
   -   5        $ lblFullName_Copy1��5      $��, y bajo anestesia local ��6+   $
��Left=�6O   $	��Top=�6j   $��25.96��6�   $��129.804��6�   $��0,33,0,0q�6�   �� 5   
   -   5        $ cbAnestesia��5      $��26��6(   $	��Top=�64   $��Normal"�6L   $
��Left=�6`   $��138.838��6{   $ 0��6�   $��129.804,35,0,0q�6�   .@�5      ��infiltrativa��6*   ��5      regional/troncular��60   ��5      subperióstica��6+   ��5      supraperióstica��6-   ��5      intrapulpar��6)   ��5      intraligamentaria��6/   5      �� 5   
   -   5        $ lblFullName_Copy2��5      $�� con ��6+   $	��Top=�6;   $��25.96��6S   $��266.642,33,0,0q�6b   $
��Left=�6z   $��33.804��6�   �� 5    
   -	   5        $ cbAnestesiaCon��5       $��26��6+   $	��Top=�67   $��Normal"�6O   $ 0��6c   $��300.446,35,195.716,0q�6v   .@�5!      ��#lidocaína con epinefrina al 2%��6<   ��5"      %!mepivacaína con epinefrina al 2%��6>   ��5#      *&mepivacaína al 3% sin vasoconstrictor��6C   ��5$      #articaína con epinefrina al 4%��6<   5%      �� 5&   
   -
   5        $ lblFullName_Copy3��5&      $�� a una dosis de ��6+   $	��Top=�6F   $��25.96��6^   $��0,33,102.912,0q�6m   $��Right=�6�   $��92.804��6�   �� 5'   
   -   5        $ txtDosis��5'      $��27��6$   $
 Wrap=�60   $	��Top=�6D   $��1q�6\   
���   �$ Center=�6�   $��60.108��6�   $��Right=�6�   $��0,31.96,42.804,0q�6�   �� 5(   
   -   5        $ lblFullName_Copy4��5(      $
�� mg,��6+   $	��Top=�6:   $��25.96��6R   $��	0,33,10,0q�6a   $��Right=�6t   $��32.804��6�   �� 5)   
   -   5        $ lblFullName_Copy5��5)      $�� con técnica de ��6+   $
��Left=�6F   $	��Top=�6a   $��25.96��6y   $��92.804��6�   $��0,67.04,0,0q�6�   �� 5*   
   -   5        $ 
txtTecnica��5*      $��27��6&   $
 Wrap=�62   $	��Top=�6F   $��1q�6^   
���   �$ Center=�6�   $��60.108��6�   $
��Left=�6�   $��92.804,66,0,0q�6�   �� 5+   
   -   5        $ lblFullName_Copy6��5+      $��, con aplicación de ��6+   $
��Left=�6J   $	��Top=�6e   $��25.96��6}   $��108.471��6�   $��152.912,67.04,0,0q�6�   �� 5,   
   -   5        $ cbAplicacion��5,      $��26��6)   $	��Top=�65   $��Normal"�6M   $ 0��6a   $��261.383,66,242.779,0q�6t   .@�5-      ��aguja corta��6)   ��5.      aguja larga��6)   5/      �� 50   
   -   5        $ lblFullName_Copy17��50      $�� ��6,   $��Right=�68   $	��Top=�6T   $��25.96��6l   $��8��6{   $��0,64,234.779,0q�6�   �� 51   
   -   5        $ txtAplicacion��51      $��27��6)   $
 Wrap=�65   $	��Top=�6I   $��1q�6a   
���   �$ Center=�6�   $��181.467��6�   $��Right=�6�   $��0,65,53.312,0q�6�   �� 52   
   -   5        $ lblFullName_Copy7��52      $:��4, se realiza preparación de pilar protésico en OD ��6+   $
��Left=�6h   $��268.642��6�   $��0,98,0,76.04q�6�   �� 53   
   -   5        $ txtAplicacion_Copy��53      $��27��6.   $
 Wrap=�6:   $	��Top=�6N   $��1q�6f   
���   �$ Center=�6�   $��266.642,98,286.224,0q�6�   �� 54   
   -   5        $ lblFullName_Copy8��54      $1��+, realizando reducción axial con fresa de ��6+   $��Right=�6`   $��221.975��6|   $��0,98,64.249,76.04q�6�   �� 55   
   -   5        $ txtAplicacion_Copy1��55      $��27��6/   $
 Wrap=�6;   $	��Top=�6O   $��1q�6g   
���   �$ Center=�6�   $��2.001,128.96,0,0q�6�   $
��Left=�6�   $��172.134��6�   �� 56   
   -   5        $ lblFullName_Copy9��56      $*��$, y reducción oclusal con fresa de ��6+   $��174.135,128.96,274.89,0q�6Y   $��25.96��6z   $	��Top=�6�   �� 57   
   -   5        $ txtAplicacion_Copy2��57      $��27��6/   $
 Wrap=�6;   $	��Top=�6O   $��1q�6g   
���   �$ Center=�6�   $��0,127.92,192.756,0q�6�   $��Right=�6�   $��82.134��6�   �� 58   
   -   5        $ lblFullName_Copy10��58      $-��', a alta velocidad y bajo irrigación. ��6,   $��0,128.96,1.087,0q�6]   $��25.96��6w   $	��Top=�6�   $��Right=�6�   $��191.669��6�   �� 59   
   -   5        $ lblFullName_Copy11��59      $~��xSe biselan angulos cavos, se prepara el bisel de la cúspide funcional. Finalmente, se elabora provisional de acrílico ��6,   $��2.001,159.92,10,0q�6�   $��25.96��6�   $	��Top=�6�   �� 5:   
   -   5        $ lblFullName_Copy12��5:      $��rápido con técnica de ��6,   $��2.001,190.88,0,0q�6M   $��25.96��6g   $	��Top=�6v   $
��Left=�6�   $��127.803��6�   �� 5;   
   -   5        $ txtAplicacion_Copy3��5;      $��27��6/   $
 Wrap=�6;   $	��Top=�6O   $��1q�6g   
���   �$ Center=�6�   $��129.804,190.88,0,0q�6�   $
��Left=�6�   $��172.134��6�   �� 5<   
   -   5        $ lblFullName_Copy13��5<      $C��=. Se recortan excedentes de material, se pule con borla y se ��6,   $��301.938,190.88,1.087,0q�6t   $��25.96��6�   $	��Top=�6�   �� 5=   
   -   5        $ lblFullName_Copy14��5=      $;��5sella con barníz, y se cementa con cemento temporal ��6,   $��0,222.88,0,0q�6k   $��25.96��6�   $	��Top=�6�   $
��Left=�6�   $��295.642��6�   �� 5>   
   -   5        $ txtAplicacion_Copy4��5>      $��27��6/   $
 Wrap=�6;   $	��Top=�6O   $��1q�6g   
���   �$ Center=�6�   $��295.642,221.84,167.224,0q�6�   �� 5?   
   -    5        $ lblFullName_Copy15��5?      $��.��6,   $��Bottom=�68   $��25.96��6S   $��0,0,122.42,7.16q�6b   $��Right=�6{   $��39.804��6�   5A      5B      