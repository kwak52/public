#!/bin/sh

systemctl start mariadb

cat << EOF | mysql
-- DROP USER 'kwak'@'localhost';
-- CREATE USER 'kwak'@'localhost' IDENTIFIED BY 'kwak';
CREATE USER 'kwak'@'localhost';
GRANT ALL PRIVILEGES ON * . * TO 'kwak'@'localhost';
FLUSH PRIVILEGES;

EOF


systemctl stop mariadb
yum erase mariadb
yum install http://www.percona.com/downloads/percona-release/redhat/0.1-3/percona-release-0.1-3.noarch.rpm
# yum list | grep percona
yum install Percona-Server-server-57
systemctl start mysql

# mysqldump 를 이용하기 위해서 사전에 반드시 필요
# 큰 table 이 만들어지고 난 후에 수행하면 시간이 매우 오래 걸림.
mysql_upgrade --force

cat << EOF | mysql
CREATE FUNCTION fnv1a_64 RETURNS INTEGER SONAME 'libfnv1a_udf.so'
CREATE FUNCTION fnv_64 RETURNS INTEGER SONAME 'libfnv_udf.so'
CREATE FUNCTION murmur_hash RETURNS INTEGER SONAME 'libmurmur_udf.so'
EOF

# For TokuDB install
# yum install Percona-Server-tokudb-57.x86_64

# For Percona XtraBackup
yum install percona-xtrabackup-24.x86_64

