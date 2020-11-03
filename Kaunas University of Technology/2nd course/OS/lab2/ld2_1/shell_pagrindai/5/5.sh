#!/bin/sh
#Kaip paleisti ls / /neratokio komandą, kad jos 
#rezultatai ir klaidos būtų perduodami komandai cksum?
ls / /neratokio 2>&1 | cksum