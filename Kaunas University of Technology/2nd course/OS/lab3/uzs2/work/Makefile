all: pathconf getcwd readdir01 stat01 statvfs01 nftw02 misc02

pathconf: ../src/evamik3_pathconf.c
	gcc -Wall -pedantic ../src/evamik3_pathconf.c -o pathconf
	
getcwd: ../src/evamik3_getcwd02.c
	gcc -Wall -pedantic ../src/evamik3_getcwd02.c -o getcwd
	
readdir01: ../src/evamik3_readdir01.c
	gcc -Wall -pedantic ../src/evamik3_readdir01.c -o readdir01
	
stat01: ../src/evamik3_stat01.c
	gcc -Wall -pedantic ../src/evamik3_stat01.c -o stat01
	
statvfs01: ../src/evamik3_statvfs01.c
	gcc -Wall -pedantic ../src/evamik3_statvfs01.c -o statvfs01
	
nftw02: ../src/evamik3_nftw02.c
	gcc -D _XOPEN_SOURCE=500 -Wall -pedantic ../src/evamik3_nftw02.c -o nftw02
	
misc02: ../src/evamik3_misc02.c
	gcc -Wall -pedantic ../src/evamik3_misc02.c -o misc02