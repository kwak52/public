#!/bin/sh


count=0
while [ $count -lt 50 ]; do
	uuid=$(uuidgen | tr '[a-z]' '[A-Z]')
	echo "	[Guid(\"$uuid\")]"
	((count++))
done

exit


#sed -e "s/\\[Guid\\(\\"[^)]*\\)\\]/GGGGGG/" < IApplication.sh
sed -e "s/\\[Guid([^)]*)/\\[Guid(\"`uuidgen`\")\\]/" < Frameworks/IApplication.cs
exit


for f in *.cs; do
	sed -e "s/\\[Guid(\".*\)\]/\\[GGGGGG()\\]" < $f
done
