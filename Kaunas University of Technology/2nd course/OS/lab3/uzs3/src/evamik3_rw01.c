/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_rw01.c */

#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <errno.h>

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
   
   if(argc != 4) {
	   printf("Naudojimas:\n %s failas1 failas2 baitu_skaicius\n", argv[0]);
	   exit(255);
   }
   
   long unsigned int size;
   sscanf(argv[3], "%lud", &size);
   if(errno != 0){
	   perror("sscanf() error");
	   exit(1);
   }
   
   int dskr1 = open(argv[1], O_RDONLY);
   if(dskr1 == -1){
	   perror(argv[1]);
	   exit(255);
   }
   int dskr2 = open(argv[2], O_WRONLY | O_CREAT | O_TRUNC, 0644 );
   if(dskr2 == -1){
	   perror(argv[2]);
	   exit(255);
   }
   
   char buf[size];
   int bytes;
   if((bytes = read(dskr1, &buf, size)) < 0){
	   perror("read() error");
	   exit(1);
   }
   
   if(write(dskr2, &buf, bytes) == -1){ 
	   perror("write() error");
	   exit(1);
   }
   
   em_close(dskr1);
   em_close(dskr2);
   
   return 0;
}