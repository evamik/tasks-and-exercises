/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_cpulimit01.c */

#include <stdio.h>
#include <sys/time.h>
#include <sys/resource.h>

int main(){
   printf( "(C) 2020 Evaldas Mikalauskas, %s\n", __FILE__ );
   
   struct rlimit rl;
   
   rl.rlim_cur = 0;
   rl.rlim_max = 1;
   
   setrlimit(RLIMIT_CPU, &rl);
   
   int c = 0;
   
   while(c != -1){
	   c++;
   }
   
   return 0;
}