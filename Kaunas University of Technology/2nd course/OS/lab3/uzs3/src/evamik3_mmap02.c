/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_mmap02.c */

#include <stdio.h>
#include <stdlib.h>
#include <sys/mman.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <fcntl.h>
#include <string.h>
#include <unistd.h>

int em_munmap(void *addr, size_t size);
int em_munmap(void *addr, size_t size){
	if(munmap(addr, size) == -1){
		perror("munmap() error");
		exit(1);
	}
	return 0;
}

int em_close(int dskr);
int em_close(int dskr){
	int cl = 0;
	if((cl = close(dskr)) == -1){
		perror("close() error");
		exit(255);
	}
	return cl;
}

int main( int argc, char * argv[] ){
   printf( "(C) 2020 Evaldas Mikalauskas, %s\n", __FILE__ );
   
   if(argc != 3) {
	   printf("Naudojimas:\n %s failas1 failas2\n", argv[0]);
	   exit(255);
   }
   
   int dskr1 = open(argv[1], O_RDONLY);
   if(dskr1 == -1){
	   perror("open() error");
	   exit(255);
   }
   
   struct stat stat1;
   if(fstat(dskr1, &stat1) == -1){
	   perror("fstat() error");
	   exit(255);
   }
   long unsigned int size = stat1.st_size;
   
   void *addr1 = mmap(NULL, size, PROT_READ, MAP_SHARED, dskr1, 0);
   if(addr1 == MAP_FAILED){
	   perror("mmap() error");
	   abort();
   }
   
   int dskr2 = open(argv[2], O_RDWR | O_CREAT | O_TRUNC, 0644 );
   if(dskr2 == -1){
	   perror(argv[2]);
	   exit(255);
   }
   
   if(ftruncate(dskr2, size) == -1){
	   perror("ftruncate() error");
	   exit(255);
   }
   
   void *addr2 = mmap(NULL, size, PROT_WRITE, MAP_SHARED, dskr2, 0);
   if(addr2 == MAP_FAILED){
	   perror("mmap() error");
	   abort();
   }
   
   memcpy(addr2, addr1, size);
   
   em_munmap(addr1, size);
   em_munmap(addr2, size);
   
   em_close(dskr1);
   em_close(dskr2);
   
   return 0;
}