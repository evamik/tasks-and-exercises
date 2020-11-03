#!/bin/sh
#užrašykite filtrą, kuris iš failo stud2001 išrinktų eilutes, 
#kuriose po A raidės seka raidė n arba raidė u, o po jų seka raidės d ir r.
cat /data/ld/ld2/stud2001 | grep "\(A[nu]dr\)"