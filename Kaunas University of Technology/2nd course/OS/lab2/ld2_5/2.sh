#!/bin/sh
awk '{if ($9 == 403) print $0}' "/data/ld/ld1/Solaris_access_log" | 
grep -o "\"[^\"]*\"" |
sort | uniq -c | sort -k1,1n
