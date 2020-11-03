#!/bin/sh
#Kaip paleisti ls / /neratokio komandą, kad jos 
#klaidos būtų perduodamos komandai cksum, o rezultatai būtų išvedami į failą?
ls / /neratokio 2>&1 > rez.txt | cksum
