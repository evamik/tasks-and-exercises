#!/bin/sh
while [ $? = 0 ] && [ $# != 0 ]
do
	echo $1
	shift 1;
done 