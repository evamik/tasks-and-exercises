/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_sablonas.c */

#include <stdio.h>
#include <unistd.h>
#include <dirent.h>

int vp_test();

int vp_test(){
   return 0;
}

int main(){
   printf( "(C) 2020 Evaldas Mikalauskas, %s\n", __FILE__ );

	char *cwd;
	struct dirent *rd;
	
	cwd = getcwd(NULL, pathconf(".", _PC_PATH_MAX));
	DIR *dir = opendir(cwd);
	
	while((rd = readdir(dir)) != NULL){
		printf("%ld %s\n", rd->d_ino, rd->d_name);
	}
	
	closedir(dir);
   
   return 0;
}