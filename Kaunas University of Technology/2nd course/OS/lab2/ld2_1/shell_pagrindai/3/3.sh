#!/bin/sh
#Kaip paleisti ls / /neratokio komandą, kad jos 
#rezultatai būtų perduodami komandai cksum, o klaidos būtų išvedamos į failą?
ls / /neratokio 2> err.txt | cksum 