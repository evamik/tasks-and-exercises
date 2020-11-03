/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_frw01.c */

#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <errno.h>

int em_close(FILE *file);
int em_close(FILE *file){
	int cl = 0;
	if((cl = fclose(file)) != 0){
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
   
   FILE *file1 = fopen(argv[1], "r");
   if(file1 == NULL){
	   perror(argv[1]);
	   exit(255);
   }
   FILE *file2 = fopen(argv[2], "w");
   if(file2 == NULL){
	   perror(argv[2]);
	   exit(255);
   }
   
   char buf[size];
   int bytes;
   if((bytes = fread(&buf, size, 1, file1)) < 0){
	   perror("read() error");
	   exit(1);
   }
   
   if(fwrite(&buf, size, 1, file2) < 0){ 
	   perror("write() error");
	   exit(1);
   }
   
   em_close(file1);
   em_close(file2);
   
   return 0;
}