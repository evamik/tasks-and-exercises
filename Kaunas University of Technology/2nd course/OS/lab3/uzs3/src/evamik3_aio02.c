/* Evaldas Mikalauskas IFF-8/11 evamik3 */
/* Failas: evamik3_aio02.c */

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <aio.h>
#include <string.h>

#define BUFFLEN 1048576

int em_aioread(int dskr, char *buf, long int offset);
int em_aioread(int dskr, char *buf, long int offset){
	struct aiocb myaiocb;
	struct aiocb *myaiocbp = &myaiocb;

	memset( (void *)myaiocbp, 0, sizeof(struct aiocb));
	myaiocbp->aio_fildes = dskr;
	myaiocbp->aio_buf = &buf[offset];
	myaiocbp->aio_nbytes = BUFFLEN-offset;
	myaiocbp->aio_offset = 0;

	if(aio_read(myaiocbp) != 0){
	   perror("aio_read() error");
	   abort();
	}

	const struct aiocb *aioptr[1];
	aioptr[0] = myaiocbp;
	if(aio_suspend(aioptr, 1, NULL) != 0){
	   perror("aio_suspend() error");
	   abort();
	}
	
	int rb = aio_return(myaiocbp);
	
	return rb;
}

void prnt_buf(char *buf, int size);
void prnt_buf(char *buf, int size){
	int i, cnt = 0;
	for(i = 0; i < size; i++)
		if(buf[i] == '\0') cnt++;
	printf("Number of '0' in buffer: %d\n", cnt);
}

int main(){
   printf( "(C) 2020 Evaldas Mikalauskas, %s\n", __FILE__ );
   
   int dskr = open("/dev/random", O_RDONLY);
   if(dskr == -1){
	   perror("dskr() error");
	   exit(1);
   }
   
   char buf[BUFFLEN];
   prnt_buf(buf, sizeof(buf));
   
   int bytes = 0;
   while(bytes < BUFFLEN){
	   bytes += em_aioread(dskr, buf, bytes);
   }
   printf("Nuskaityta %d baitų\n", bytes);
   
   prnt_buf(buf, sizeof(buf));
   
   return 0;
}