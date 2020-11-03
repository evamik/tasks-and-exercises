echo "poziciniai argumentai="$*
a=$1$2
echo "a=$a" 
$a
b=`$3$4`   
echo "b=$b" 
c=`expr 2 + 2`
d=`expr 2+2`
echo $c + 2
echo $d + 2
echo $c $d