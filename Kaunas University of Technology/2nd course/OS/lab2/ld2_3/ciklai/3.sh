#!/bin/sh
echo "įveskite 10 skaičių atskirtų tarpais"
read skaiciai
arPirmas=0
for sk in $skaiciai
do
	if [ $arPirmas -eq 0 ]
	then 
		arPirmas=1
		pirmas=$sk
		echo pirmas skaičius = $pirmas
		continue
	fi
	if [ $sk -lt $pirmas ]; then mazesni=$mazesni$sk" "; fi;
	if [ $sk -eq $pirmas ] || [ $sk -gt $pirmas ]; then didesni=$didesni$sk" "; fi;
done 
echo mažesni už pirmą: $mazesni
echo didesni arba lygūs pirmam: $didesni