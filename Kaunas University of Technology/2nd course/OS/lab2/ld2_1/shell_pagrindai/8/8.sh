#!/bin/sh
#iš katalogo /usr/bin išrinkite failus, kurių vardai sudaryti iš dviejų simbolių;
#iš jų atrinkite tuos, kurių pavadinime yra skaičių.
ls /usr/bin | grep -E "^.{2}$" | grep -E "[[:digit:]]"