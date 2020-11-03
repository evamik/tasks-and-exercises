#!/bin/sh
for f in $*
do
	arg1=neskaitomas
	arg2=nevykdomas
	if [ -r "$f" ]; then arg1=skaitomas; fi;
	if [ -x "$f" ]; then arg2=vykdomas; fi;
	echo "$f >> $arg1/$arg2"
done 