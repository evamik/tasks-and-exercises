/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_sablonas.c */

#include <stdio.h>
#include <stdlib.h>
#include <errno.h>

void fun1();
void fun1(){
	puts("fun1()");
}

void fun2();
void fun2(){
	puts("fun2()");
}

void fun3();
void fun3(){
	puts("fun3()");
}

int main( int argc, char * argv[] ){
   printf( "(C) 2020 Evaldas Mikalauskas, %s\n", __FILE__ );
   
   if(argc != 2){
	   printf("Naudojimas:\n %s (1 | 2 | 3 | 4) --- argumentas atitinka (_Exit()|exit()|abort()|return)\n", argv[0]);
	   exit(255); 
   }
   
   unsigned int i;
   sscanf(argv[1], "%ud", &i);
   if(errno != 0){
	   perror("sscanf() error");
	   exit(1);
   }
   if(i < 1 || i > 4){
	   printf("Naudojimas:\n %s (1 | 2 | 3 | 4) --- argumentas atitinka (_Exit()|exit()|abort()|return)\n", argv[0]);
	   exit(255); 
   }
   
   
   if(atexit(fun1) != 0){
	   puts("atexit(fun1) fail");
   }
   if(atexit(fun2) != 0){
	   puts("atexit(fun2) fail");
   }
   if(atexit(fun3) != 0){
	   puts("atexit(fun3) fail");
   }
   
   switch(i){
		case 1:
			puts("_Exit()");
			_Exit(0);
			break;
		case 2:
			puts("exit()");
			exit(0);
			break;
		case 3:
			puts("abort()");
			abort();
			break;
		case 4:
			puts("return");
			break;
   }
   
   return 0;
}