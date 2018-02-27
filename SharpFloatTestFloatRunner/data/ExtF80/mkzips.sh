#!/bin/bash
md=le
rmodes="max min minMag near_even near_maxMag odd"
declare -A fnames

fnames[max]=max
fnames[min]=min
fnames[minMag]=min_mag
fnames[near_even]=near_even
fnames[near_maxMag]=near_max_mag
fnames[odd]=odd

for rmode in $rmodes; do
 fname=${fnames[$rmode]}
 ./testfloat_gen -r"$rmode" extF80_$md > ./"$md"_"$fname".txt
 zip ./"$md"_"$fname".zip ./"$md"_"$fname".txt
done