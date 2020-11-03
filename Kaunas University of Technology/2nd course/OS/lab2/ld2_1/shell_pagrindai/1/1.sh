#!/bin/sh
#Kaip paleisti ls / /neratokio komandą, kad jos 
#rezultatai būtų spausdinami į vieną failą, o klaidos į kitą?
ls / /neratokio > rez.txt 2> err.txt
