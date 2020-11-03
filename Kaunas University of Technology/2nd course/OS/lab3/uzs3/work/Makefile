all: rw01 seek01 frw01 keaio01 mmap02

rw01: ../src/evamik3_rw01.c
	gcc -g -Wall -pedantic ../src/evamik3_rw01.c -o rw01

seek01: ../src/evamik3_seek01.c
	gcc -g -Wall -pedantic ../src/evamik3_seek01.c -o seek01

frw01: ../src/evamik3_frw01.c
	gcc -g -Wall -pedantic ../src/evamik3_frw01.c -o frw01

keaio01: ../src/kespaul_aio01.c
	gcc -lrt -g -Wall -pedantic ../src/kespaul_aio01.c -o keaio01

mmap02: ../src/evamik3_mmap02.c
	gcc -g -Wall -pedantic ../src/evamik3_mmap02.c -o mmap02