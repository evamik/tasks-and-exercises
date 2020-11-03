/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_pathconf.c */

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>


int main( int argc, char * argv[] ){
   printf( "(C) 2020 Evaldas Mikalauskas, %s\n", __FILE__ );
   
   if( argc != 2 ){
      printf( "Naudojimas:\n %s failas_ar_katalogas\n", argv[0] );
      exit( 255 );
   }
   
   int n = pathconf(argv[1], _PC_NAME_MAX);
   int p = pathconf(argv[1], _PC_PATH_MAX);
   if(n == -1 || p == -1) perror("pathconf() error");
   else{
	   printf("Maksimalus failo vardo ilgis: %d\n", n);
	   printf("Maksimalus kelio ilgis: %d\n", p);
   }
   
   return 0;
}