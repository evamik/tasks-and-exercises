/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_getcwd02.c */

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h>

int em_getcwd(char **cwd);

int em_getcwd(char **cwd){
	*cwd = getcwd(NULL, pathconf(".", _PC_PATH_MAX));
	puts(*cwd);
	
	return 1;
}

int main( int argc, char * argv[] ){
	char *cwd;
	
   printf( "(C) 2020 Evaldas Mikalauskas, %s\n", __FILE__ );
   
   em_getcwd(&cwd);
   
   int dskr = open(cwd, O_RDONLY);
   printf("%s: %d\n", cwd, dskr);
   free(cwd);
   
   chdir("/tmp");
   em_getcwd(&cwd);
   
   if(fchdir(dskr) == 0)
	em_getcwd(&cwd);
   else perror("fchdir");
   close(dskr);
   
   return 0;
}