#!/bin/sh
#iš katalogo /usr/sbin išrinkite failus, kurių vardai prasideda 
#re ir surūšiuokite juos atvirkščia tvarka;
ls /usr/sbin | grep -E "^re[:alnum]*" | sort -r