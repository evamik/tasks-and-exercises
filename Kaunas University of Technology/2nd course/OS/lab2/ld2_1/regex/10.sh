#!/bin/sh
#užrašykite filtrą, kuris faile stud2001 ieškotų eilučių su 
#įrašais apie 9/1 arba 9/2 grupių studentus.
cat /data/ld/ld2/stud2001 | grep -E "((9/1)|(9/2))[^0-9]"