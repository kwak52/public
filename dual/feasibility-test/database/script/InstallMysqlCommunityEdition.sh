#!/bin/sh

yum install http://dev.mysql.com/get/mysql-community-release-el7-5.noarch.rpm
# yum repolist enabled | grep "mysql.*-community.*"

yum install mysql-community-server

exit



# requires below
cat << EOF > /dev/null
[root@localhost mysql]# yum list |grep mysql
mysql-community-client.x86_64             5.7.13-1.el7                 @mysql57-community
mysql-community-common.x86_64             5.7.13-1.el7                 @mysql57-community
mysql-community-libs.x86_64               5.7.13-1.el7                 @mysql57-community
mysql-community-libs-compat.x86_64        5.7.13-1.el7                 @mysql57-community
mysql-community-server.x86_64             5.7.13-1.el7                 @mysql57-community
mysql57-community-release.noarch          el7-8                        @/mysql57-community-release-el7-8.noarch

EOF
