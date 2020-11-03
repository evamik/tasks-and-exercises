/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_misc02.c */

#include <stdio.h>
#include <stdlib.h>
#include <sys/stat.h>

int vp_test();

int vp_test(){
   return 0;
}

int main( int argc, char * argv[] ){
   printf( "(C) 2020 Evaldas Mikalauskas, %s\n", __FILE__ );
   
   if( argc != 2 ){
      printf( "Naudojimas:\n %s naujas_katalogas\n", argv[0] );
      exit( 255 );
   }
   
   if(mkdir(argv[1], S_IRWXG|S_IRWXO|S_IRWXU) == -1){
	   perror("mkdir() error");
   }
   else {
	   printf("Sukurtas naujas katalogas: %s\n", argv[1]);
   }
   
   return 0;
}