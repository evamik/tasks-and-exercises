#!/bin/sh
#užrašykite filtrą, kuris ieškotų eilučių besibaigiančių seka andr arba mari.
cat /data/ld/ld2/stud2001 | grep -E "((andr)|(mari))$"