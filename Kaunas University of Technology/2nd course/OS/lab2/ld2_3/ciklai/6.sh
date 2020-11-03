#!/bin/sh
re='^[1-9]+$'
if [ $# = 1 ] && [[ $1 =~ $re ]]
then
	if [ $1 -gt 0 ]
	then
		sk=0
		while [ ! $sk -gt $1 ]
		do
			echo $sk
			sk=$((sk+1))
		done
		exit 0
	fi 
	echo skaičius negali būti mažiau už 0!
	exit 1
fi 
echo blogai nurodytas argumentas! Turi būti skaičius.
exit 1