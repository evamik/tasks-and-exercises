/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_seek01.c */

#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <errno.h>
#define SIZE 1048576

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
   
   if(argc != 2) {
	   printf("Naudojimas:\n %s failas1\n", argv[0]);
	   exit(255);
   }
   
   int dskr = open(argv[1], O_WRONLY | O_CREAT | O_TRUNC, 0644 );
   if(dskr == -1){
	   perror(argv[1]);
	   exit(255);
   }
   
   if(lseek(dskr, SIZE, SEEK_SET) == -1){
		perror("lseek() error");
		exit(0);
   }
   if(write(dskr, &dskr, 1) == -1){ 
	   perror("write() error");
	   exit(1);
   }
   
   em_close(dskr);
   
   return 0;
}