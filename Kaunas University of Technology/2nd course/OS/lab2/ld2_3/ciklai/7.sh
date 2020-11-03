#!/bin/sh
length=60
seconds=$length
interval=3
while [ ! $seconds -gt $length ]
do
	if [ $seconds -eq $length ]
	then
		for u in $*
		do
			nr=`w $u | awk 'END{print FNR}'`
			if [ $nr -eq 3 ]
			then
				echo $u šiuo metu yra prisijungęs!
				exit 0
			fi
		done
		seconds=0
	fi
	
	format="0"
	dots=0
	while [ $dots -lt $length ]
	do
		if [ $dots -lt $seconds ]; then format=$format"+"; 
		else format=$format"-"; fi;
		dots=$((dots+1))
	done
	format=$format"$length seconds"
	echo $format
	
	sleep $interval
	seconds=$((seconds+interval))
done 