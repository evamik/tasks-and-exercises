echo "Įveskite savo gimimo metus, mėnesį ir dieną (skaičius) atskirdami tarpais:"
read s1 s2 s3
echo gimimo data: $s1 $s2 $s3
dal1=$((s1/s2))
liek1=$((s1%s2))
dal2=$((s1/s3))
liek2=$((s1%s3))
echo $s1/$s2=$dal1 $s1/$s3=$dal2
echo dalybos rezultatų suma: $dal1+$dal2=$((dal1+dal2))
echo liekanų sandauga: $liek1*$liek2=$((liek1*liek2))