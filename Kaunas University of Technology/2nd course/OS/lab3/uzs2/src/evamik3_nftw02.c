/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_nftw02.c */

#include <stdio.h>
#include <stdlib.h>
#include <sys/stat.h>
#include <ftw.h>

int em_ftwinfo(const char *p, const struct stat *st, int fl, struct FTW *fbuf);

int em_ftwinfo(const char *p, const struct stat *st, int fl, struct FTW *fbuf){
    if(fl == FTW_F){
		printf("%s\n", p);
	}
	return 0;
}

int main(){
   printf( "(C) 2020 Evaldas Mikalauskas, %s\n", __FILE__ );
   
   nftw("/home/evamik3", em_ftwinfo, 20, FTW_PHYS);
   
   return 0;
}