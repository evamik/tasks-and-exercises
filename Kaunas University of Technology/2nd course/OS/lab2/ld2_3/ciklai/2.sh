#!/bin/sh
set `ls`
	for f in $*
	do
		file $f
	done 