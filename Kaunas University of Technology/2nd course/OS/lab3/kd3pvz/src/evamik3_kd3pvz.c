/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_kd3pvz.c */

#include <stdio.h>
#include <stdlib.h>
#include <sys/stat.h>
#include <ftw.h>
#include <sys/mman.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h>
#include <fcntl.h>
#include <errno.h>

int em_ftwinfo(const char *p, const struct stat *st, int fl, struct FTW *fbuf);

int em_ftwinfo(const char *p, const struct stat *st, int fl, struct FTW *fbuf){
	if(fl == FTW_F && st -> st_size > 4096){
		printf("%s %ld\n", p, st -> st_size);
		int dskr = open(p, O_RDWR);
		if(dskr == -1) {
			perror("open() error");
			exit(1);
		}
		return dskr;
	}
	return 0;
}

int main( int argc, char * argv[] ){
   printf( "(C) 2020 Evaldas Mikalauskas, %s\n", __FILE__ );
   
   int add_size = 10;
   
   if(argc != 3) {
	   printf("Naudojimas:\n %s katalogas sveikas_skaicius\n", argv[0]);
	   exit(255);
   }
   
   long unsigned int byte;
   
   sscanf(argv[2], "%lud", &byte);
   if(errno != 0){
	   perror("sscanf() error");
	   exit(1);
   }
   
   int dskr;
   if((dskr = nftw(argv[1], em_ftwinfo, 20, FTW_PHYS)) == -1){
	   perror("nftw() error");
	   exit(1);
   }
   
   struct stat mystat;
   if(fstat(dskr, &mystat) == -1){
	   perror("fstat() error");
	   exit(255);
   }
   
   printf("%ld\n", mystat.st_size);
   int size = mystat.st_size;
   
   if(ftruncate(dskr, size+add_size) == -1){
	   perror("ftruncate() error");
	   exit(255);
   }
   
   void *addr = mmap(NULL, size+add_size, PROT_READ, MAP_SHARED, dskr, 0);
   if(addr == MAP_FAILED){
	   perror("addr mmap() error");
	   abort();
   }
   
   char *buf;
   
   memcpy(&buf, addr, add_size);
   if(lseek(dskr, size, SEEK_SET) == -1){
		perror("lseek() error");
		exit(0);
   }
   memcpy(addr2, addr, 10);
   
   /*if(write(dskr, &buf, add_size) == -1){ 
	   perror("write() error");
	   exit(1);
   }*/
   
   if(byte >= size+add_size){
		printf("Nurodytas sveikas skaičius %ld išeina už failo ribų\n", byte);
		exit(255);
   }
   
   return 0;
}