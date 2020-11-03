#!/bin/sh
#užrašykite filtrą, kuris ieškotų eilučių, kuriose nėra jokio skaičiaus.
grep -v "[[:digit:]]"

#užrašykite filtrą, kuris ieškotų eilutėse 
#bet kokios raidės(tiek didžiosios, tiek mažosios).
grep "[[:alpha:]]"