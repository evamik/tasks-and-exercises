/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_stat01.c */

#include <stdio.h>
#include <unistd.h>
#include <sys/stat.h>

void em_stat();

void em_stat(){
	struct stat info;
   
   if(stat(getcwd(NULL, pathconf(".", _PC_PATH_MAX)), &info) !=0){
	   perror("stat() error");
   }
   else {
	   printf("%15s %d\n", "savininko ID:", info.st_uid);
	   printf("%15s %ld\n", "dydis:", info.st_size);
	   printf("%15s %ld\n", "i-node numeris:", info.st_ino);
	   printf("%15s %08x\n", "leidimai:", info.st_mode);
	   char * type;
	   switch (info.st_mode & S_IFMT) {
        case S_IFREG:  
            type = "file";
            break;
        case S_IFDIR:
            type = "directory";
            break;
        case S_IFCHR:        
            type = "character device";
            break;
        case S_IFBLK:        
            type = "block device";
            break;
        case S_IFLNK: 
            type = "symbolic link";
            break;
        case S_IFIFO: 
            type = "pipe";
            break;
        case S_IFSOCK:
            type = "socket";
            break;
        default:
            type = "unknown";
	   }
	   printf("%15s %s\n", "failo tipas:", type);
   }
}

int main(){
   printf( "(C) 2020 Evaldas Mikalauskas, %s\n", __FILE__ );
   
   em_stat();
   
   return 0;
}