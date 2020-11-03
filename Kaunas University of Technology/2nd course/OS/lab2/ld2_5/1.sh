#!/bin/sh
lines=`grep -o "\\033[^m]*m" <<< '\033[01;32maaa\033[01;34m bbb \033[00mccc'`
for line in $lines
do
	echo -e \\$line'\\'$line
done
